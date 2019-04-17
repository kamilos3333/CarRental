﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public int Cost { get; set; }
        public string place1 { get; set; }
        public string place2 { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }

        public virtual Car car { get; set; }
        public virtual ApplicationUser user { get; set; }
    }

    public class TempReservation
    {
        public string Place1 { get; set; }
        public string Place2 { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateB { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateE { get; set; }
    }

    public class SummaryCost
    {
        public int ID_car { get; set; }
        public string Place1 { get; set; }
        public string Place2 { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateB { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateE { get; set; }
        public int additionalCostPlace { get; set; }
        public double totalDayCost { get; set; }
        public decimal carCost { get; set; }
        public decimal totalCost { get; set; }
    }
}