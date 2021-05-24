using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Image_Management.Models
{
    public class FileModel
    {
        
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string  FilePath { get; set; }
        [Required]
        public string  ThumbPath { get; set; }

        [Required]
        public DateTime UploadTime { get; set; }
        
        
    }
}
