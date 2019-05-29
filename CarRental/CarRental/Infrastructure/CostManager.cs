using CarRental.Models;
using CarRental.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Infrastructure
{
    public class CostManager
    {
        private UnitOfWork unitOfWork;

        public CostManager(UnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        public List<SummaryCost> CostSummary(int ID, string place1, string place2, DateTime dateB, DateTime dateE)
        {
            int additionalCostPlace = unitOfWork.PlaceRepository.GetAll(filter: a => a.Name == place1).Select(x => x.AddCost).FirstOrDefault();
            decimal carCost = unitOfWork.CarRepository.GetAll(includeProperties: "CarClass", filter: a => a.ID_Car == ID).Select(x => x.CarClass.Cost).FirstOrDefault();
            double totalDays = TotalDaysCount(dateB, dateE);
            double totalDaysCost = TotalDaysCost(totalDays);
            int totalCostRent = TotalCostRent(totalDaysCost, additionalCostPlace, (int)carCost);
            if (HttpContext.Current.User.Identity.IsAuthenticated) { totalCostRent = UserDiscount(totalCostRent); }
            
            List<SummaryCost> costs = new List<SummaryCost>
            {
                new SummaryCost()
                {
                  ID_car = ID,
                  Place1 = place1,
                  Place2 = place2,
                  DateB = dateB,
                  DateE = dateE,
                  additionalCostPlace = additionalCostPlace,
                  totalDayCost = totalDaysCost,
                  carCost = carCost,
                  totalCost = totalCostRent
                }
            };

            return costs;
        }

        public double TotalDaysCount(DateTime dateB, DateTime dateE)
        {
            return dateE.Date.Subtract(dateB.Date).TotalDays;
        }

        public double TotalDaysCost(double totalDays)
        {
            return totalDays * 85;
        }

        public int TotalCostRent(double totalDaysCost, int additionalCostPlace, int carCost)
        {
            return (int)totalDaysCost + additionalCostPlace + carCost;
        }

        public int UserDiscount(int totalCostRent)
        {
            return totalCostRent - 10;
        }
        
        public bool CostValidation(SummaryCost cost)
        {
            if(cost.totalCost < 0)
            {
                return false;
            }
            return true;
        }

    }
}