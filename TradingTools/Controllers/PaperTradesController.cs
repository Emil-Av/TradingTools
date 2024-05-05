using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using NuGet.Protocol;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace TradingTools.Controllers
{
    public class PaperTradesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PaperTradesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<PaperTrade> objPaperTrades = _unitOfWork.PaperTrade.GetAll().ToList();
            return View(objPaperTrades);
        }

        /// <summary>
        ///  Method to process the uploaded .zip file. The .zip file has to have the structure mainFolder\Strategy\TimeFrame\SampleSize\TradeNumber\files
        /// </summary>
        /// <param name="zipFile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadTradesAsync(IFormFile zipFile)
        {
            // Check if the submitted file is a zip file
            if (zipFile == null || Path.GetExtension(zipFile.FileName).ToLower() != ".zip")
            {
                // Show error notification
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    zipFile.CopyTo(memoryStream);

                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        bool canCreateNewTrade = true;
                        string currentFolder = string.Empty;
                        PaperTrade? trade = null;
                        Journal? journal = null;
                        List<ZipArchiveEntry> sortedEntries = archive.Entries.OrderBy(e => e.FullName, new NaturalStringComparer()).ToList();
                        List<string> folders = new List<string>();

                        foreach (var entry in sortedEntries)
                        {
                            // Entry is a folder
                            if (entry.FullName.EndsWith('/') || entry.FullName.EndsWith("\\"))
                            {
                                if (canCreateNewTrade)
                                {
                                    //_unitOfWork.PaperTrade.Add(trade);
                                    journal = new Journal();
                                    trade = new PaperTrade();
                                    int id = _unitOfWork.PaperTrade.GetAll().Select(x => x.Id).OrderByDescending(id => id).FirstOrDefault();
                                    journal.PaperTrade.Id = id;
                                    folders.Clear();
                                }
                                canCreateNewTrade = false;
                                continue;
                            }
                            // Entry is either a screenshot or the .odt journal file
                            else if (!entry.FullName.Contains("Reviews"))
                            {
                                if (!canCreateNewTrade)
                                {
                                    string[] tradeInfo = entry.FullName.Split('/');
                                    // wwwroot\Trades folder
                                    currentFolder = Path.Combine(wwwRootPath, tradeInfo[0]);
                                    // Get all subfolders
                                    for (int i = 1; i <= 4; i++)
                                    {
                                        folders.Add(tradeInfo[i]);
                                    }
                                    // Create the wwwroot\Trades folder
                                    if (!Directory.Exists(currentFolder))
                                    {
                                        Directory.CreateDirectory(currentFolder);
                                    }
                                    // Create all subfolders
                                    for (int i = 0; i < folders.Count; i++)
                                    {
                                        currentFolder = Path.Combine(currentFolder, folders[i]);
                                        if (!Directory.Exists(currentFolder))
                                        {
                                            Directory.CreateDirectory(currentFolder);
                                        }
                                    }
                                }

                                try
                                {
                                    // Save the file
                                    entry.ExtractToFile(Path.Combine(currentFolder, entry.Name));
                                    if (entry.FullName.EndsWith(".png"))
                                    {
                                        string screenshotName = entry.FullName.Split('/').Last();
                                        trade.ScreenshotsUrls.Add(Path.Combine(currentFolder, screenshotName));
                                    }
                                    else if (entry.FullName.EndsWith(".odt"))
                                    {
                                        using (ZipArchive odtFile = ZipFile.OpenRead(Path.Combine(currentFolder, entry.Name)))
                                        {
                                            ZipArchiveEntry contentEntry = odtFile.GetEntry("content.xml");
                                            if (contentEntry != null)
                                            {
                                                using (StreamReader reader = new StreamReader(contentEntry.Open()))
                                                {
                                                    string xmlContent = reader.ReadToEnd();
                                                    XDocument xmlDoc = XDocument.Parse(xmlContent);
                                                    List<XElement> nodes = xmlDoc.Descendants().Where(e => e.Name.LocalName == "p").ToList();
                                                    string lastJournal = string.Empty;
                                                    foreach (XElement node in nodes)
                                                    {
                                                        XElement element = XElement.Parse(node.ToString());
                                                        if (element.Value.Contains("[Pre]"))
                                                        {
                                                            lastJournal = "[Pre]";
                                                            continue;

                                                        }
                                                        else if (element.Value.Contains("[During]"))
                                                        {
                                                            lastJournal = "[During]";
                                                            continue;

                                                        }
                                                        else if (element.Value.Contains("[Exit]"))
                                                        {
                                                            lastJournal = "[Exit]";
                                                            continue;

                                                        }
                                                        else if (element.Value.Contains("[Post]"))
                                                        {
                                                            lastJournal = "[Post]";
                                                            continue;

                                                        }

                                                        if (lastJournal.Equals("[Pre]"))
                                                        {
                                                            journal.Pre += element.Value + "\n";
                                                        }
                                                        else if (lastJournal.Equals("[During]"))
                                                        {
                                                            journal.During += element.Value+ "\n";
                                                        }
                                                        else if (lastJournal.Equals("[Exit]"))
                                                        {
                                                            journal.Exit += element.Value+ "\n";
                                                        }
                                                        else if (lastJournal.Equals("[Post]"))
                                                        {
                                                            journal.Post += element.Value+ "\n";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    // Display error message
                                }
                                canCreateNewTrade = true;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }

            return RedirectToAction("Index");
        }
    }

    public class NaturalStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            string[] xParts = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
            string[] yParts = Regex.Split(y.Replace(" ", ""), "([0-9]+)");

            int minLength = Math.Min(xParts.Length, yParts.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (xParts[i] != yParts[i])
                {
                    if (int.TryParse(xParts[i], out int xNum) && int.TryParse(yParts[i], out int yNum))
                    {
                        return xNum.CompareTo(yNum);
                    }
                    else
                    {
                        return string.Compare(xParts[i], yParts[i], StringComparison.Ordinal);
                    }
                }
            }

            return xParts.Length.CompareTo(yParts.Length);
        }
    }
}
