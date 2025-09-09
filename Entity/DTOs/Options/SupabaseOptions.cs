using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Options
{
    public class SupabaseOptions
    {
        public string Url { get; set; } = default!;
        public string ServiceRoleKey { get; set; } = default!;
        public string Bucket { get; set; } = "img-people";
        public string PublicBaseUrl { get; set; } = default!;
    }
}
