﻿using Microsoft.AspNetCore.Mvc;

namespace PracticaContabilidad.Controller
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}