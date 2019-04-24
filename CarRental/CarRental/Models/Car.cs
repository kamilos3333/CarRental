using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class Car
    {
        [Key]
        public int ID_Car { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Brand { get; set; }
        [Display(Name = "Transmission")]
        public int ID_Tran { get; set; }
        [Display(Name = "Class")]
        public int ID_CarClass { get; set; }
        [Display(Name = "Body")]
        public int ID_CarBody { get; set; }
        public string Photo { get; set; }
        public bool Active { get; set; }

        public virtual CarClass CarClass { get; set; }
        public virtual CarBody CarBody { get; set; }
        public virtual Transmission Transmission { get; set; }
        public ICollection<ReservForm> reservForms { get; set; }
    }
}