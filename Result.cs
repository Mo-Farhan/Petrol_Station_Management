using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetrolStation
{
    public partial class Result : Form
    {
        public Result()
        {
            InitializeComponent();
        }

        private void Result_Load(object sender, EventArgs e)
        {
            txtAmtOfFuel.Text = (Globals.amtOfFuelDispensed).ToString();
            txtAmtOfMoney.Text = (Globals.amtOfFuelDispensed * 2).ToString() + " GBP";
            txtNoOfVehiclesServiced.Text = (Globals.noOfVehicles).ToString();
            txtNoOfVehiclesLeftWithoutFuelling.Text = (Globals.noOfVehiclesLeftWithoutFuelling).ToString();
            txtAmtOfUnleaded.Text = (Globals.amtOfFuel_Unleaded_Dispensed).ToString();
            txtAmtOfDiesel.Text = (Globals.amtOfFuel_Diesel_Dispensed).ToString();
            txtAmtOfLPG.Text = (Globals.amtOfFuel_LPG_Dispensed).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
