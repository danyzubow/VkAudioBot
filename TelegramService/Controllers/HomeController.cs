using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TelegramService.Library.Models;
using VK_API.VkWrapper;
using VkAudioBot.VkWrapper.Models;

namespace TelegramService.Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AudioProvider audioProvider;
      
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<AudioInfoModel> GetPlayList()
        {
            if (audioProvider == null)
            {
                audioProvider = new AudioProvider();
            }
            return audioProvider.GetPlayList("id556153348");
        }
    }
}
