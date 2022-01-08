using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStation
{
    public class Vehicle
    {
        string type;
        public Fuel fuel;
        public Vehicle()
        {
            fuel = new Fuel();
        }
        public Vehicle(Vehicle v)
        {
            this.type = v.type;
            this.fuel = v.fuel;
        }
        public Vehicle(string CarType)
        {
            type = CarType;
            fuel = new Fuel();
        }
        public string GetCarType()
        {
            return type;
        }

    }
}
