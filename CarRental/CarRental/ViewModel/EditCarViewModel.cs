using CarRental.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.ViewModel
{
    public class EditCarViewModel
    {
        public Car Car { get; set; }
        public IEnumerable<Transmission> Transmissions { get; set; }
        public IEnumerable<CarClass> CarClasses { get; set; }
        public IEnumerable<CarBody> CarBodies { get; set; }
        [DataType(DataType.Upload)]
        public string Image { get; set; }
    }
}