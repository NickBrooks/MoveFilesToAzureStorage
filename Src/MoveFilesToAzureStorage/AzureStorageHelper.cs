using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MoveFilesToAzureStorage
{
    /*
     * Thanks to Rakki Muthukumar 
     * http://thenextdoorgeek.com/post/Windows-Azure-Websites-WebJob-to-upload-FREB-logs-to-Azure-Blob-Storage
     */
    public class AzureStorageHelper
    {
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer container;
        private CloudBlockBlob blockBlob;

        private bool deleteAfterUpload = false;

        public AzureStorageHelper()
        {
            Console.WriteLine("Using Storage Container " + Environment.GetEnvironmentVariable("APPSETTING_StorageContainer"));
            storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("CUSTOMCONNSTR_StorageConnectionString"));
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(Environment.GetEnvironmentVariable("APPSETTING_StorageContainer"));
            container.CreateIfNotExists();
        }

        public void UploadFileToBlob(string name, string path)
        {
            try
            {
                Console.WriteLine("Starting uploading " + name);
                using (var fileStream = System.IO.File.OpenRead(path))
                {
                    blockBlob = container.GetBlockBlobReference(name);
                    blockBlob.Properties.ContentType = MimeHelper.GetMimeBasedOnExtensionFor(path);
                    blockBlob.UploadFromStream(fileStream);
                    Console.WriteLine(name + " successfully uploaded!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}