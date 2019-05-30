using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CarRental.Infrastructure.Interface
{
    public interface IImageManager
    {
        string InsertImage(HttpPostedFileBase upload);
    }
}
