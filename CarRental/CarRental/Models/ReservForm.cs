using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class ReservForm
    {
        [Key]
        public int ID_Reserv { get; set; }
        public int ID_Car { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateBegin { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public int Coast { get; set; }
        [ForeignKey("place1")]
        public int? Id_Place1 { get; set; }
        public Place place1 { get; set; }
        [ForeignKey("place2")]
        public int? Id_Place2 { get; set; }
        public Place place2 { get; set; }

        public virtual Car car { get; set; }
        public virtual ApplicationUser user { get; set; }
    }

    public class TempReservation
    {
        [Required]
        public string Place1 { get; set; }
        [Required]
        public string Place2 { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string DateB { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string DateE { get; set; }


    }

}