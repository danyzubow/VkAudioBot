using System;

namespace VK_API.VkWrapper.Accounts
{
    [Serializable]
    public class Account
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Account(string login, string password)
        {
            Login = login;
            Password = password;
        }
        public Account()
        {
            
        }
    }
}
