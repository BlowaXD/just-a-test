using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace Darewise.Feedback.Controllers
{
    /// <summary>
    /// They didn't provide interface so I can not mock them
    /// Just showing it isn't that hard to implement but this class is not used
    /// </summary>
    public class GcsBlobStorage : IBlobStorage
    {
        private readonly StorageClient _client;

        public GcsBlobStorage(StorageClient client) => _client = client;

        public async Task<string> SaveBlobAsync(string bucketName, string fileName, string contentType, Stream data)
        {
            Object? obj = await _client.UploadObjectAsync(bucketName, fileName, contentType, data);
            return obj.SelfLink;
        }
    }
}