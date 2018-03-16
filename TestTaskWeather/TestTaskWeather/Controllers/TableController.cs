using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TestTaskWeather.Models;

namespace TestTaskWeather.Controllers
{
    /// <summary>
    /// Контроллер Table
    /// обрабатывает вывод таблицы погоды по фильтру с БД.
    /// </summary>
    public class TableController : Controller
    {
        private RecordContext db = new RecordContext();

        
        // GET: /Table/
        /// <summary>
        /// Действие страницы Index для GET запросов
        /// Выводит в выпадающие списки доступные для фильтрации года и месяцы.
        /// </summary>
        /// <returns>Пустую таблицу</returns>
        [HttpGet]
        public ViewResult Index()
        {
            List<WeatherRecord> all = db.Records.ToList();
            List<object> yearsList = new List<object>();
            yearsList.Add("All");
            List<object> monthsList = new List<object>();
            monthsList.Add("All");
            foreach (WeatherRecord rec in all)
            {
                if (!yearsList.Contains(rec.Date.Year))
                    yearsList.Add(rec.Date.Year);
            }
            SelectList years = new SelectList(yearsList);
            ViewBag.Years = years;

            foreach(WeatherRecord rec in all)
            {
                if (!monthsList.Contains(rec.Date.Month))
                    monthsList.Add(rec.Date.Month);
            }
            SelectList months = new SelectList(monthsList);
            ViewBag.Months = months;
            List<WeatherRecord> np = new List<WeatherRecord>();
                return View(np);
        }

        /// <summary>
        /// Действие страницы Index для POST запросов.
        /// По фильтру в POST запросе, указанному параметрами в выпадающем списке выводит записи о погоде.
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns>Список с отфильтрованными записями о погоде</returns>
        [HttpPost]
        public ViewResult Index(string Year, string Month)
        {
            try
            {
                List<WeatherRecord> all = db.Records.ToList();
                List<object> yearsList = new List<object>();
                yearsList.Add("All");
                List<object> monthsList = new List<object>();
                monthsList.Add("All");
                foreach (WeatherRecord rec in all)
                {
                    if (!yearsList.Contains(rec.Date.Year))
                        yearsList.Add(rec.Date.Year);
                }
                SelectList years = new SelectList(yearsList);
                ViewBag.Years = years;

                foreach (WeatherRecord rec in all)
                {
                    if (!monthsList.Contains(rec.Date.Month))
                        monthsList.Add(rec.Date.Month);
                }
                SelectList months = new SelectList(monthsList);
                ViewBag.Months = months;

                if (Year != "All" && Month != "All")
                {
                    var result =
                        from record in all
                        where record.Date.Year == int.Parse(Year) && record.Date.Month == int.Parse(Month)
                        select record;
                    return View(result);
                }
                else if (Month == "All" && Year != "All")
                {
                    var result =
                        from record in all
                        where record.Date.Year == int.Parse(Year)
                        select record;
                    return View(result);
                }
                else if (Year == "All" && Month != "All")
                {
                    var result =
                        from record in all
                        where record.Date.Month == int.Parse(Month)
                        select record;
                    return View(result);
                }
                return View(all);
            }
            catch(Exception ex)
            {
                return View();
            }
        }

    }
}
