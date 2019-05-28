using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Helpers
{
    public static class UrlHelpers
    {
        public static string CarImgPath(this UrlHelper helper, string nameCarFolder)
        {
            var CarImgFolder = AppConfig.ImagesFolderPath;
            var path = Path.Combine(CarImgFolder, nameCarFolder);
            var pathFolder = helper.Content(path);

            return pathFolder;
        }
    }
}