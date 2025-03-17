using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApplicationLayer.DTOs.Supabase
{
    public class SupabaseSettings
    {
        public string Url { get; set; }
        public string Bucket { get; set; }
        public string ServiceRoleKey { get; set; }
    }

    public class UploadFileRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }

}
