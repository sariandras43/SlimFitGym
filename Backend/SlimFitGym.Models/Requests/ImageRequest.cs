using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym.Models.Requests
{
    public class ImageRequest
    {
        public string FileName { get; set; }
        public string ImageInBase64 { get; set; }
    }
}
