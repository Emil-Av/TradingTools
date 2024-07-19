using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Models.ViewModels;
using Shared;
using System.Diagnostics;

namespace Utilities
{
    /// <summary>
    ///  Provides static methods used in the app.
    /// </summary>
    public static class AppHelper
    {
        public static async Task<List<string>> SaveFiles<T>(string webRootPath, T vm, object newTrade, IFormFile[] files)
        {
            // /Screenshots
            string screenshotsDir = Path.Combine(webRootPath, "Screenshots");
            List<string> screenshotsPaths = new List<string>();
            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            if (vm is NewTradeVM viewModel)
            {
                string dirToSaveFiles = string.Empty;
                string tradeType = MyEnumConverter.TradeTypeFromEnum(viewModel.TradeType);
                dirToSaveFiles = Path.Combine(screenshotsDir, tradeType);
                if (!Directory.Exists(dirToSaveFiles))
                {
                    // /Screenshots/Research(e.g.)
                    Directory.CreateDirectory(dirToSaveFiles);
                }
                if (tradeType == MyEnumConverter.TradeTypeFromEnum(SharedEnums.Enums.TradeType.Research))
                {
                    // /Screenshots/Research/FirstBarPullback(e.g.)
                    dirToSaveFiles = Path.Combine(dirToSaveFiles, newTrade.GetType().Name);
                    if (!Directory.Exists(dirToSaveFiles))
                    {
                        Directory.CreateDirectory(dirToSaveFiles);
                    }
                }
                
                // /Screenshots/Research/(typeResearch/)Sample Size 1(e.g.)
                string[] dirToSaveFilesFiles = Directory.GetFiles(dirToSaveFiles);
                if (dirToSaveFilesFiles.Length > 0)
                {
                    int lastSampleSizeDir = dirToSaveFilesFiles.Length - 1;
                    // Trade directories in the sample size e.g. Screenshots/Research/FirstBarPullback/Sample Size 1/Trade 2
                    string[] sampleSizeDirectories = Directory.GetFiles(dirToSaveFilesFiles[lastSampleSizeDir]);
                    // Check the number of trades of the last sample size
                    if (sampleSizeDirectories.Length < 100)
                    {
                        // create a folder for the trade
                        dirToSaveFiles = Path.Combine(Path.Combine(dirToSaveFiles, $"Trade {sampleSizeDirectories.Length + 1}"));
                        Directory.CreateDirectory(dirToSaveFiles);
                    }
                    else
                    {
                        dirToSaveFiles = Path.Combine(dirToSaveFiles, $"Sample Size {dirToSaveFilesFiles.Length + 1}");
                        // last sample size is full, create new one
                        Directory.CreateDirectory(dirToSaveFiles);
                    }
                }
                else
                {
                    // Create the 1st sample size and the first trade directories
                    dirToSaveFiles = Path.Combine(dirToSaveFiles, "Sample Size 1", "Trade 1");
                    Directory.CreateDirectory(dirToSaveFiles);
                }

                try
                {
                    foreach (IFormFile file in files)
                    {
                        string filePath = dirToSaveFiles + file.FileName;
                        using (Stream stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            screenshotsPaths.Add(filePath);
                        }
                    }
                }
                catch (Exception ex) 
                {
                    Debug.WriteLine($"Error in saving uploaded files: {ex.Message}");
                }
            }

            return screenshotsPaths;
        }

        /// <summary>
        ///  Creates the folders in wwwroot\Screenshots for the screenshots when uploading a .zip file for PaperTrades or Research
        /// </summary>
        /// <param name="tradeInfo"></param>
        /// <param name="currentFolder"></param>
        /// <param name="entryFullName"></param>
        /// <param name="wwwRootPath"></param>
        /// <param name="numberFolderToCreate"></param>
        /// <returns></returns>
        public static string CreateScreenshotFolders(string[] tradeInfo, string currentFolder, string entryFullName, string wwwRootPath, int numberFolderToCreate)
        {
            List<string> folders = new List<string>();
            // wwwroot\Screenshots
            string screenshotsFolder = Path.Combine(wwwRootPath, "Screenshots");
            if (!Directory.Exists(screenshotsFolder))
            {
                // Create wwwroot\Screenshots
                Directory.CreateDirectory(screenshotsFolder);
            }
            currentFolder = Path.Combine(screenshotsFolder, tradeInfo[0]);
            if (!Directory.Exists(currentFolder))
            {
                // Create View folder (e.g. wwwroot\Screenshots\PaperTrades
                Directory.CreateDirectory(currentFolder);
            }
            // Get all subfolders
            for (int i = 1; i <= numberFolderToCreate; i++)
            {
                // No need for "Reviews" folder (when the method is called from PapersView)
                if (!tradeInfo[i].Contains("Reviews"))
                {
                    folders.Add(tradeInfo[i]);
                }
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

            return currentFolder;
        }
    }
}
