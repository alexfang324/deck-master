using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayPalDemo.Models;

namespace PayPalDemo.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            List<Product> items;
            string projectPath = System.IO.Directory.GetCurrentDirectory();
            string jsonFilePath = "/wwwroot/file/products.json";
            using (StreamReader r = new StreamReader(projectPath + jsonFilePath))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Product>>(json);
            }


            return View(items);
        }
    }
}
