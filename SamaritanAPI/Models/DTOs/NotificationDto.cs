using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamaritanAPI.Models.DTOs
{
    public class NotificationDto
    {
        public string title { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;
    }
}