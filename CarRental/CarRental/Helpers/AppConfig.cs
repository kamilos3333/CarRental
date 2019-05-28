using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CarRental.Helpers
{
    public class AppConfig
    {
        private static string _imagesCarFolderPath = ConfigurationManager.AppSettings["CarsFolder"];

        public static string ImagesFolderPath
        {
            get
            {
                return _imagesCarFolderPath;
            }
        }
    }
}