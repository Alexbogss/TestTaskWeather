using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TestTaskWeather.Models
{
    /// <summary>
    /// Класс контекста для погодных записей.
    /// Подробности соединения указанны в Web.config
    /// </summary>
    public class RecordContext : DbContext
    {
        public RecordContext()
            : base("DBConnection")
        { }
        public DbSet<WeatherRecord> Records { get; set; }
    }
}