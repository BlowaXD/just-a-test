using System.IO;
using System.Threading.Tasks;

namespace Darewise.Feedback.Controllers
{
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