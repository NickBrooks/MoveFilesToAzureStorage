using System;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Collections.Generic;

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

        public AzureStorageHelper()
        {
            Console.WriteLine("Using Storage Container " + ENV.Get("APPSETTING_StorageContainer"));
            storageAccount = CloudStorageAccount.Parse(ENV.Get("CUSTOMCONNSTR_StorageConnectionString"));
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(ENV.Get("APPSETTING_StorageContainer"));
            container.CreateIfNotExists();
            var perm = new BlobContainerPermissions();
            perm.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(perm); 

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