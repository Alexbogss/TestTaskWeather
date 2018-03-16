using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;

namespace TestTaskWeather.Models
{
    /// <summary>
    /// Сущность погодной записи. 
    /// Единственное что не сделал: не смог разобраться с форматом записи в БД в виде time(0), чтобы не было мс.
    /// Учтено, что некоторые целочисленные параметры могут иметь значение NULL.
    /// </summary>
    public class WeatherRecord 
    {
        public int WeatherRecordId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan Time { get; set; }
        public double Temperature { get; set; }
        public double AirHumidity { get; set; }
        public double DewPoint { get; set; }
        public int AirPressure { get; set; }
        public string WindDirection { get; set; }
        public int? WindSpeed { get; set; }
        public int? Cloudiness { get; set; }
        public int? CloudBoundary { get; set; }
        public int? VisibilityRange { get; set; }
        public string WeatherConditions { get; set; }
        
        public WeatherRecord()
        {
            WindSpeed = null;
            Cloudiness = null;
            VisibilityRange = null;
        }
         
    }
}