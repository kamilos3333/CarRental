using CarRental.Helpers;
using CarRental.Infrastructure.Injection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CarRental.Infrastructure
{
    public class ImageManager : IImageManager
    {
        public string InsertImage(HttpPostedFileBase upload)
        {
            var filename = Guid.NewGuid() + Path.GetExtension(upload.FileName);
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(AppConfig.ImagesFolderPath), filename);
            upload.SaveAs(path);
            return filename;
        }
    }
}