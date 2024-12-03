using Microsoft.AspNetCore.Mvc;
using SoftExpress_Challange.ViewModels;
using System;

namespace SoftExpress_Challange.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
