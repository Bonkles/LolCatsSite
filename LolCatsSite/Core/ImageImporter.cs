using System;
using System.Linq;
using System.Collections;
using System.IO;
using System.Collections.Generic; 

namespace DataImporters
{

    /// <summary>
    /// The DataImporters namespace contains classes that import data into our databases automatically.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    /// <summary>
    /// The ImageImporter class 
    /// </summary>
    public class ImageImporter
    {
        /// <summary>
        /// This is the hard coded path to the application's project file. 
        /// <remarks>You MUST change this directory path depending on where you are running the application!</remarks>
        /// </summary>
        static string hardCodedDirectoryPath = "C:\\Users\\beclark\\workspace\\LolCatsSiteExample\\LolCatsSite";

        /// <summary>
        /// The directory, relative to the hardCodedDirectoryPath, in which we should recursively search for 
        /// lolCat images to load. 
        /// </summary>
        static string imgInboxString = "lolcat_inbox";

        public static string FullPathToImageDirectory
        {
        get {return hardCodedDirectoryPath + "\\" + imgInboxString + "\\";}
        }
        /// <summary>
        /// Internal variable for the file paths. 
        /// Not populated until <see cref="lookThroughInbox"/> is run. 
        /// </summary>
        private IEnumerable<string> lolCatFilePaths;

        /// <summary>
        /// An enumeration of all the filepaths that the importer knows about. 
        /// </summary>
        public IEnumerable<string> LolCatFilePaths
        {
            get { return lolCatFilePaths; }
        }

        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public ImageImporter()
        {
        }
                
        /// <summary>
        /// A new pics property- this determines if any new pictures were found by the image importer. 
        /// </summary>
        public Boolean NewPics
        {
            get 
            {
                //Do, or do not. There is no.... oh, wait. 
                try
                {//Do we have any file paths in our enumeration? 
                    return (lolCatFilePaths.Count() > 0);
                }
                //If we encounter any kind of exception while evaluating the count, 
                //just return false. 
                catch (Exception)
                {
                    return false; 
                }
            }
        }

        /// <summary>
        /// lookThroughInbox looks through the the directory specified 
        /// by the combination of  <see cref="hardCodedDirectoryPath"/> 
        /// and <see cref="imgInboxString"/>. It creates a list of the of the image paths
        /// and stores them in <see cref="lolCatFilePaths"/>, but only if they end in .jpg, .gif, or .png!.
        /// <remarks> If the app does not appear to be loading
        /// images to the database, make sure that these directories are set correctly, the imageImporter might be 
        /// pointed to the wrong location.</remarks>
        /// </summary>
        public void lookThroughInbox()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            Directory.SetCurrentDirectory(hardCodedDirectoryPath);
            //Check if the source directory exists. 
            if (Directory.Exists(imgInboxString))
            {
                //Get the directory info on the image source dir- 
                //this is so we can iterate over its subfolders/files. 
                DirectoryInfo dirInfo = new DirectoryInfo(imgInboxString);
                foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
                {
                    var dirFileResults = from fileName in subDir.GetFiles()
                                      //Don't discriminate against animated gifs, or portable network graphics! 
                                      //Of course, import the jpgs. 
                                    where fileName.Extension == ".jpg" ||
                                          fileName.Extension == ".gif" ||
                                          fileName.Extension == ".png" 
                                    orderby fileName.FullName //Not necessary. But fun! 
                                    select fileName.FullName;

                    //This will add the current directory's image path results to the existing results query. 
                    if (dirFileResults != null)
                    {
                        //if lolCatFilePaths is uninitialized, just set it equal to the contents
                        //of the current query. 
                        if (lolCatFilePaths == null)
                            lolCatFilePaths = dirFileResults;
                        //Otherwise, we already have some valid query results, so add them to the 
                        //existing results. 
                        else
                            lolCatFilePaths = lolCatFilePaths.Union(dirFileResults).ToList();
                    }
                }

            }

        }

    }
}