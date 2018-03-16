using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace TestTaskWeather.Controllers
{
    /// <summary>
    /// Контроллер страницы загрузки файлов.
    /// </summary>
    public class UploadController : Controller
    {

        private Models.RecordContext db = new Models.RecordContext();
        //
        // GET: /Upload/
        /// <summary>
        /// Действие страницы Index. Выводит сообщения о результатах обработки и загрузки.
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            ViewBag.Exception = TempData["Message"];
            return View();
        }

        /// <summary>
        /// Обработчик загрузки файла в БД.
        /// Позволяет загружать n-ое кол-во документов единовременно.
        /// При появлении ошибки при обработке отменяет загрузку файла целиком. Отказоустойчив.
        /// </summary>
        /// <param name="uploads"></param>
        /// <returns></returns>
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> uploads)
        {
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<Models.RecordContext>());

            foreach (var file in uploads)
            {
                if (file != null)
                {
                    try
                    {
                        string fileName = System.IO.Path.GetFileName(file.FileName);

                        string[] check = fileName.Split('.');
                        if (check[1] == "xlsx")
                        {
                            XSSFWorkbook xssfwb = new XSSFWorkbook(file.InputStream);

                            for (int sheetNum = 0; sheetNum < xssfwb.NumberOfSheets; ++sheetNum)
                            {
                                ISheet sheet = xssfwb.GetSheetAt(sheetNum);

                                for (int row = 4; row <= sheet.LastRowNum; ++row)
                                {
                                    var currentRow = sheet.GetRow(row);
                                    if (currentRow != null)
                                    {
                                        Models.WeatherRecord record = new Models.WeatherRecord();

                                        for (int currentCell = 0; currentCell < 12; ++currentCell)
                                        {
                                            var cellVal = currentRow.GetCell(currentCell);
                                            if (cellVal != null)
                                            {
                                                switch (currentCell)
                                                {
                                                    case 0:
                                                        {
                                                            DateTime a = DateTime.Parse(cellVal.ToString());
                                                            record.Date = a.Date;
                                                            break;
                                                        }
                                                    case 1:
                                                        {
                                                            string[] tempTime = cellVal.ToString().Split(':');
                                                            DateTime tempDate = new DateTime(record.Date.Year, record.Date.Month, record.Date.Day,
                                                                                Int32.Parse(tempTime[0]), Int32.Parse(tempTime[1]), 0);
                                                            record.Time = tempDate.TimeOfDay;
                                                            break;
                                                        }
                                                    case 2:
                                                        {
                                                            record.Temperature = cellVal.NumericCellValue;
                                                            break;
                                                        }
                                                    case 3:
                                                        {
                                                            record.AirHumidity = cellVal.NumericCellValue;
                                                            break;
                                                        }
                                                    case 4:
                                                        {
                                                            record.DewPoint = cellVal.NumericCellValue;
                                                            break;
                                                        }
                                                    case 5:
                                                        {
                                                            int temp;
                                                            if (int.TryParse(cellVal.ToString(), out temp))
                                                                record.AirPressure = temp;
                                                            break;
                                                        }
                                                    case 6:
                                                        {
                                                            record.WindDirection = cellVal.ToString();
                                                            break;
                                                        }
                                                    case 7:
                                                        {
                                                            int temp;
                                                            if (int.TryParse(cellVal.ToString(), out temp))
                                                                record.WindSpeed = temp;
                                                            break;
                                                        }
                                                    case 8:
                                                        {
                                                            int temp;
                                                            if (int.TryParse(cellVal.ToString(), out temp))
                                                                record.Cloudiness = temp;
                                                            break;
                                                        }
                                                    case 9:
                                                        {
                                                            int temp;
                                                            if (int.TryParse(cellVal.ToString(), out temp))
                                                                record.CloudBoundary = temp;
                                                            break;
                                                        }
                                                    case 10:
                                                        {
                                                            int temp;
                                                            if (int.TryParse(cellVal.ToString(), out temp))
                                                                record.VisibilityRange = temp;
                                                            break;
                                                        }
                                                    case 11:
                                                        {
                                                            record.WeatherConditions = cellVal.ToString();
                                                            break;
                                                        }
                                                }
                                            }
                                        }

                                        db.Records.Add(record);

                                    }
                                }
                            }
                        }
                        else
                        {
                            TempData["Message"] = "Формат файлов несоответствует";
                            return RedirectToAction("Index");
                        }
                    }
                    catch(Exception ex)
                    {
                        TempData["Message"] = ex.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
            db.SaveChanges();
            TempData["Message"] = "Загрузка успешно завершена";
            return RedirectToAction("Index");
        }
    }
}