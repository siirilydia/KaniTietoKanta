using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KaniTietoKantaMVC.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using KaniTietoKantaMVC.Extensions;

namespace KaniTietoKantaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //List<Kani> kaikki = DataHelper.HaeKaikkiKanit();

                //string json = "";
                //using (var client = new HttpClient())
                //{
                //    client.DefaultRequestHeaders.Accept.Add(new
                //   MediaTypeWithQualityHeaderValue("application/json"));
                //    var response = client.GetAsync($"https://localhost:44323/api/KaniDB/Valikko").Result;
                //    json = response.Content.ReadAsStringAsync().Result;
                //}

                //List<Kani> kaikki = JsonConvert.DeserializeObject<List<Kani>>(json);

                //ViewBag.Kaikkikanit = kaikki;

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
    }
}
