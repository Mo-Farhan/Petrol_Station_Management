using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStation
{
    class Attendee
    {
            double totNoOfLtrDispensedPerAttendent;
            public Attendee()
            {
                totNoOfLtrDispensedPerAttendent = 0;
            }
            public void SetTotNoOfLtrDispensedPerAttendent(double noOfLtr)
            {
                totNoOfLtrDispensedPerAttendent += noOfLtr;
            }
        public double GetTotNoOfLtrDispensedPerAttendent()
        {
            return totNoOfLtrDispensedPerAttendent;
        }
    }
}
