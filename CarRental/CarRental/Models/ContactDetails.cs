using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class ContactDetails
    {
        [Key]
        public int ID_Cont { get; set; }

        public int ID_Reserv { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Telephone { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return Name + ", " + Surname;
            }
        }

        public virtual ReservForm ReservForm { get; set; }
    }
}