using System;
using VK_API.VkWrapper.Accounts;

namespace VK_API
{
    class GlobalSettings
    {
        private static string slash;
        public static bool IsUnix
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Unix;
            }

        }

        public static string Slash
        {
            get
            {
                if (slash == null)
                {
                    if (IsUnix)
                        slash = "/";
                    else
                        slash = "\\";
                }
                return slash;
            }
        }

        public static bool UseProxy
        {
            get { return false; }
        }

        public static Account Account { get; set; }
    }
}
