using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestTaskWeather.Controllers
{
    /// <summary>
    /// Контроллер Home.
    /// </summary>
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        /// <summary>
        /// Показывает страницу.
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            return View(); 
        }
	}
}