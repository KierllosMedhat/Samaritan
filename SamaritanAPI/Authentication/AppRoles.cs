using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SamaritanAPI.Authentication
{
    public static class AppRoles
    {
        public const string Administrator = "Administrator";
        public const string SubLeader = "SubLeader";
        public const string ServantDialler = "ServantDialler";
    
    }
}