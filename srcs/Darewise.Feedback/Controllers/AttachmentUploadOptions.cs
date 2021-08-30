using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Darewise.Feedback.Controllers
{
    public class AttachmentUploadOptions
    {
        public int LimitPerFile { get; set; }
        public int LimitTotal { get; set; }
        public int LimitFilesCount { get; set; }
    }
}