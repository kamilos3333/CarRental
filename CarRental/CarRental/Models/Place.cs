using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class Place
    {
        [Key]
        public int Id_Place { get; set; }
        public string Name { get; set; }
        public int AddCost { get; set; }

        [InverseProperty("place1")]
        public ICollection<ReservForm> frmReservations1 { get; set; }
        [InverseProperty("place2")]
        public ICollection<ReservForm> frmReservations2 { get; set; }
    }
    
}