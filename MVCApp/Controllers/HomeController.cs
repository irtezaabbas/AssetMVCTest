using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLibrary;
using static DataLibrary.Logic.AssetManager;

namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
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

            var data = LoadAssets();
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
        public ActionResult AssignDevice(InventoryModel model)
        {
            if (ModelState.IsValid)
            {
                int recordsCreated = CreateAsset(model.AssetTag,
                    model.AssetState, 
                    model.UsedBy, 
                    model.UsageType, 
                    model.AssignedOn);
                return RedirectToAction("AssignDevice");
            }

            return View();
        }
    }
}