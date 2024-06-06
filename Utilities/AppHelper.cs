using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    /// <summary>
    ///  Provides static methods used in throghout the app.
    /// </summary>
    public static class AppHelper
    {
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
