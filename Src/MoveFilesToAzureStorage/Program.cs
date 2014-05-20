using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Management.Storage;
using System.IO;
using System.Configuration;

namespace MoveFilesToAzureStorage
{
    public class Program
    {
        private static AzureStorageHelper storageHelper;

        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing AzureStorageHelper!");

            storageHelper = new AzureStorageHelper();

            string path = Environment.GetEnvironmentVariable("APPSETTING_FilesLocation");
            
            Console.WriteLine("Loading files from " + path);

            TraverseDirectory(new DirectoryInfo(path));

            Console.WriteLine("AzureStorageHelper Done at " + DateTime.Now.ToString() + " Stopping!");
        }

        private static void TraverseDirectory(DirectoryInfo directory)
        {
            Console.WriteLine(String.Format("{0} Started working directory {1}", DateTime.Now.ToString(), directory.FullName));

            if (directory == null) return;

            foreach(var subDirectory in directory.GetDirectories())
            {
                TraverseDirectory(subDirectory);
            }

            foreach (var file in directory.GetFiles("*"))
            {
                Upload(file);
            }
        }

        private static void Upload(FileInfo file)
        {
            Console.WriteLine("Creating " + file.Name + " " + file.FullName);
            storageHelper.UploadFileToBlob(file.Name, file.FullName);
        }
    }
}
