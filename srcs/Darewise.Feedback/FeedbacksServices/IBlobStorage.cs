using System.IO;
using System.Threading.Tasks;

namespace Darewise.Feedback.Controllers
{
    public interface IBlobStorage
    {
        public Task<string> SaveBlobAsync(string bucketName, string fileName, string contentType, Stream data);
    }
}