using System;
using System.IO;

namespace Darewise.Feedback.Controllers
{
    public class FeedbackBlob
    {
        /// <summary>
        /// File name
        /// Some files are named to include some context
        /// I decided to keep the original file name to facilitate further treatment
        /// Example: crash-log-2021-08.log
        /// Example: ui-bug-inventory.png
        /// </summary>
        public string Filename { get; set; }
        
        /// <summary>
        /// Stream to read the data
        /// </summary>
        public Stream Data { get; set; }

        public string ContentType { get; set; }
    }
}