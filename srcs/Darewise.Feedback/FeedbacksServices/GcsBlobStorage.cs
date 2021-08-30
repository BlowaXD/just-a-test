﻿using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace Darewise.Feedback.Controllers
{
    /// <summary>
    /// They didn't provide interface so I can not mock them
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

    /// <summary>
    /// Just to make it work locally
    /// </summary>
    public class FileSystemBlobStorage : IBlobStorage
    {
        public async Task<string> SaveBlobAsync(string bucketName, string fileName, string contentType, Stream data)
        {
            if (!Directory.Exists(bucketName))
            {
                Directory.CreateDirectory(bucketName);
            }

            string? path = $"{bucketName}/{fileName}";
            await using FileStream? writer = File.OpenWrite(path);
            await data.CopyToAsync(writer);
            return path;
        }
    }
}