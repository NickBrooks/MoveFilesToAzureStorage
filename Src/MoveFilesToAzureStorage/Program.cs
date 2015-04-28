using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace MoveFilesToAzureStorage
{
    public class Program
    {
        private static AzureStorageHelper storageHelper;
        private static string path;

        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing AzureStorageHelper!");

            storageHelper = new AzureStorageHelper();

            path = ENV.Get("APPSETTING_FilesLocation");

            Console.WriteLine("Loading files from " + path);

            TraverseDirectory(new DirectoryInfo(path));

            Console.WriteLine("AzureStorageHelper Done at " + DateTime.Now.ToString() + " Stopping!");
        }

        private static void TraverseDirectory(DirectoryInfo directory)
        {
            Console.WriteLine(String.Format("{0} Started working directory {1}", DateTime.Now.ToString(), directory.FullName));

            if (directory == null) return;

            foreach (var subDirectory in directory.GetDirectories())
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
            var name = CreateNameFromPath(file.FullName);

            Console.WriteLine("Creating " + name + " " + file.FullName);
            storageHelper.UploadFileToBlob(name, file.FullName);
        }

        private static string CreateNameFromPath(string filename)
        {
            return filename.Replace(path, "")
                .Replace(@"\", "/")
                .TrimStart('/');
        }
    }
}
