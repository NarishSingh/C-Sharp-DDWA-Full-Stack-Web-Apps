using System.Web;

namespace FileUploads.Models
{
    public class FileUploadVM
    {
        public HttpPostedFileBase Upload { get; set; }
    }
}