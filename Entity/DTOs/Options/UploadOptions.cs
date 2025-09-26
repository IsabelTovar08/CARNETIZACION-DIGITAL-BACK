using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Options
{
    public class UploadOptions
    {
        public long MaxBytes { get; set; } = 5 * 1024 * 1024;
        public string[] AllowedContentTypes { get; set; } = new[] { "image/jpeg", "image/png", "image/webp" };
    }
}
