using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class CarClass
    {
        [Key]
        public int ID_CarClass { get; set; }
        [StringLength(15, MinimumLength = 1, ErrorMessage = "Invalid")]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:n}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}