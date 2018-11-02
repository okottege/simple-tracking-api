using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using TrackerService.Data.Contracts;

namespace TrackerService.Data.Repositories
{
    public class BlobStorageRepository : IDocumentStorageRepository
    {
        private readonly StorageConnectionInfo connInfo;

        public BlobStorageRepository(StorageConnectionInfo connInfo)
        {
            this.connInfo = connInfo;
        }

        public async Task SaveDocument(string fileName, Stream stream)
        {
            var storage = CloudStorageAccount.Parse(connInfo.ConnectionString);
            var blobClient = storage.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(connInfo.ContainerName);
            var blob = container.GetBlockBlobReference(fileName);

            stream.Position = 0;
            await blob.UploadFromStreamAsync(stream);
        }
    }
}
