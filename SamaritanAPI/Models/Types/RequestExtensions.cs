using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamaritanAPI.Models.Types
{
    public static class RequestExtensions
    {
        public static void IncreaseLevel(this RequestLevel level)
        {
            if(level < RequestLevel.Closed)
            {
                level++;
            }
            else
            {
                throw new InvalidOperationException("Cannot increase level beyond Closed");
            }
        }

        public static void DecreaseLevel(this RequestLevel level)
        {
            if(level > RequestLevel.Admin)
            {
                level--;
            }
            else
            {
                throw new InvalidOperationException("Cannot decrease level beyond Open");
            }
        }

        public static void UpdateTimeline(this Request request, string update)
        {
            request.RequestTimeline += $"{DateTime.Now}: {update}\n";
        }
    }
}