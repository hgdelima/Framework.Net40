using Alcoa.Framework.Common.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate Files operations
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Get files at path
        /// </summary>
        public static ICollection<FileInfo> GetFiles(string directoryPath)
        {
            return GetFiles(directoryPath, string.Empty);
        }

        /// <summary>
        /// Get files at path using search pattern and search options (Current only or All folders)
        /// </summary>
        public static ICollection<FileInfo> GetFiles(string directoryPath, string searchPattern, SearchOption searchOptions = SearchOption.TopDirectoryOnly, int numberFilesToTake = int.MaxValue)
        {
            return (string.IsNullOrEmpty(searchPattern))
                ? Directory.EnumerateFiles(directoryPath).Take(numberFilesToTake).Select(file => new FileInfo(file)).ToList()
                : Directory.EnumerateFiles(directoryPath, searchPattern, searchOptions).Take(numberFilesToTake).Select(file => new FileInfo(file)).ToList();
        }

        /// <summary>
        /// Get files at path using network credentials
        /// </summary>
        public static ICollection<FileInfo> GetFilesUsingCredential(string directoryPath, string userName, string password, string domain)
        {
            var files = new List<FileInfo>();

            using (new NetworkConnection(directoryPath, new NetworkCredential(userName, password, domain)))
            {
                files.AddRange(Directory.GetFiles(directoryPath).Select(file => new FileInfo(file)));
            }

            return files;
        }

        /// <summary>
        /// Save text content into file at specific path
        /// </summary>
        public static bool SaveAsFile(this string text, string filePathAndName)
        {
            var wrote = true;

            try
            {
                File.WriteAllText(filePathAndName, text);
            }
            catch (Exception ex)
            {
                wrote = false;
            }

            return wrote;
        }

        /// <summary>
        /// Save content into file as text at specific path
        /// </summary>
        public static bool SaveAsFile(this Stream input, string filePathAndName)
        {
            var wrote = true;

            try
            {
                using (var sr = new StreamReader(input, true))
                {
                    wrote = sr.ReadToEnd().SaveAsFile(filePathAndName);
                }
            }
            catch (Exception ex)
            {
                wrote = false;
            }

            return wrote;
        }

        /// <summary>
        /// Reads files contents given an specific path
        /// </summary>
        public static ICollection<string> OpenFiles(string directoryPath)
        {
            return OpenFiles(directoryPath, string.Empty);
        }

        /// <summary>
        /// Reads files contents using search pattern and search options (Current only or All folders)
        /// </summary>
        public static ICollection<string> OpenFiles(string directoryPath, string searchPattern, SearchOption searchOptions = SearchOption.TopDirectoryOnly, int numberFilesToTake = int.MaxValue)
        {
            return (string.IsNullOrEmpty(searchPattern))
                ? Directory.EnumerateFiles(directoryPath).Take(numberFilesToTake).Select(File.ReadAllText).ToList()
                : Directory.EnumerateFiles(directoryPath, searchPattern, searchOptions).Take(numberFilesToTake).Select(File.ReadAllText).ToList();
        }

        /// <summary>
        /// Delete a file at a given path, and returns success or fail
        /// </summary>
        public static bool DeleteFile(string filePathAndName)
        {
            var deleted = true;

            try
            {
                if (File.Exists(filePathAndName))
                    File.Delete(filePathAndName);
            }
            catch (Exception ex)
            {
                deleted = false;
            }

            return deleted;
        }

        /// <summary>
        /// Compact a file using Zip format given a inputPath and an outputPath
        /// </summary>
        public static void CompactAsZip(string inputPath, string outputPath)
        {
            using (var inputFile = new FileStream(inputPath, FileMode.Open))
            {
                var outputFile = new FileStream(outputPath, FileMode.Create);

                using (var streamDeCompactacao = new GZipStream(outputFile, CompressionMode.Compress))
                {
                    int currentByte = inputFile.ReadByte();

                    while (currentByte != -1)
                    {
                        streamDeCompactacao.WriteByte((byte)currentByte);
                        currentByte = inputFile.ReadByte();
                    }
                }
            }
        }

        /// <summary>
        /// Searchs for a File Full Path given a root path, a file name and a subfolder as optional
        /// </summary>
        public static string SearchFileFullPath(string rootPath, string fileName, string subFolderName = "")
        {
            var directories = Directory.EnumerateDirectories(rootPath);

            //Filters by subfolder name if it's provided
            if (!string.IsNullOrEmpty(subFolderName))
                directories = directories.Where(di => di.Contains(subFolderName)).Select(di => di);

            return directories.SelectMany(di => Directory.EnumerateFiles(di, fileName, SearchOption.AllDirectories)).FirstOrDefault();
        }
    }
}