using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class CarBody
    {
        [Key]
        public int ID_CarBody { get; set; }
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}