using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Image_Management.Models
{
    public class FileViewModel
    {
        
        public IFormFile file { get; set; }

    }
}
