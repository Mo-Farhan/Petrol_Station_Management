using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStation
{
    public class Pump
    {
        char availability;
        int assignedTimeInSecs;
        public Vehicle myVehicle;
        public Pump()
        {
            availability = 'Y';
            assignedTimeInSecs = 0;
            myVehicle = new Vehicle();
        }

        public char GetAvailability()
        {
            return availability;
        }
        public void SetAvailability(char ch)
        {
            availability = ch;
        }
        public int GetAssignedTimeInSecs()
        {
            return assignedTimeInSecs;
        }
        public void SetAssignedTimeInSecs(int sec)
        {
            assignedTimeInSecs = sec;
        }
        public void AllocateVehicle(Vehicle v)
        {
            myVehicle = v;
        }
        public Vehicle GetAllocatedVehicle()
        {
            return myVehicle;
        }

    }
}
