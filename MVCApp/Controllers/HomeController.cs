using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLibrary;
using static DataLibrary.Logic.AssetManager;
using DataLibrary.Logic;

namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string apiKey = "your_freshservice_api_key"; // Securely store your API key
        private static List<string> validAssetTags;

        public HomeController()
        {
            // Fetch valid asset tags once when the controller is instantiated
            if (validAssetTags == null)
            {
                validAssetTags = FreshserviceClient.GetValidAssetTags(apiKey).Result;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult ViewAssets()
        {
            ViewBag.Message = "Assigned Assets List";

            var data = LoadAssets(); // Load from local database
            List<InventoryModel> assets = new List<InventoryModel>();

            foreach (var row in data)
            {
                assets.Add(new InventoryModel
                {
                    AssetTag = row.AssetTag,
                    AssetState = row.AssetState,
                    UsedBy = row.UsedBy,
                    UsageType = row.UsageType,
                    AssignedOn = row.AssignedOn
                });
            }

            return View(assets);
        }

        public ActionResult AssignDevice()
        {
            ViewBag.Message = "Assign Device";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignDevice(InventoryModel model)
        {
            if (ModelState.IsValid)
            {
                if (validAssetTags.Contains(model.AssetTag))
                {
                    await AssignAsset(model.AssetTag, model.UsedBy, model.AssignedOn, apiKey); // Assign asset and update Freshservice
                    return RedirectToAction("ViewAssets"); // Redirect to view assets list
                }
                else
                {
                    ModelState.AddModelError("AssetTag", "Invalid Asset Tag. Please enter a valid tag from Freshservice.");
                }
            }

            return View(model);
        }
    }
}
