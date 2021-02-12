using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bowling.Models;
using Bowling.ViewModels;

namespace Bowling.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            var viewModel = new BowlingViewModel();

            return View(viewModel);
        }
    }
}