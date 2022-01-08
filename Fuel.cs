using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStation
{
    public class 
        Fuel
    {
        string type;
        int fuelCapacity;
        int amtOfFuelToBeFilled;
        double fuellingProcessTimeInSecs;

        public Fuel()
        {

        }
        public void SetFuelType(string FuelType)
        {
            type = FuelType;
        }
        public string GetFuelType()
        {
            return type;
        }
        public void SetFuelCapacity(int capacity)
        {
            fuelCapacity = capacity;
        }
        public int GetFuelCapacity()
        {
            return fuelCapacity;
        }
        public void SetAmtOfFuelToBeFilled(int AmtFuel)
        {
            amtOfFuelToBeFilled = AmtFuel;
        }
        public int GetAmtOfFuelToBeFilled()
        {
            return amtOfFuelToBeFilled;
        }
        public void SetFuellingProcessTimeInSecs(double fuelTime)
        {
            fuellingProcessTimeInSecs = fuelTime;
        }
        public double GetFuellingProcessTimeInSecs()
        {
            return fuellingProcessTimeInSecs;
        }
    }
}
