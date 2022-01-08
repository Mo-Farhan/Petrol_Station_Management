using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetrolStation
{
    public class Transaction
    {
        public string type;
        public double noOfLitres;
        public int laneId;
        public int pumpId;
        public Transaction(string t, double no, int lId, int pId)
        {
            type = t;
            noOfLitres = no;
            laneId = lId;
            pumpId = pId;
        }
    }
}
