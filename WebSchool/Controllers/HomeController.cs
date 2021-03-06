﻿using Microsoft.AspNetCore.Mvc;

namespace WebSchool.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return Redirect("/Groups/Index");
            }

            return View();
        }
    }
}
