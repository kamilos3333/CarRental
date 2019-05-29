using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.ViewModel
{
    public class ContactDetailsViewModels
    {
        public int ID_Cont { get; set; }

        public int ID_Reserv { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [StringLength(9)]
        public string Telephone { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return Surname + ", " + Name;
            }
        }
        
    }
}