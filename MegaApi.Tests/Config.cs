using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MegaApi.Tests
{
    public static class Config
    {
        public static readonly string TestUserName = ConfigurationManager.AppSettings["TestUserName"];
        public static readonly string TestUserPass = ConfigurationManager.AppSettings["TestUserPass"];
        public static readonly string TestUserHash = ConfigurationManager.AppSettings["TestUserHash"];
    }
}
