using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PetrolStation
{

    public partial class Form1 : Form
    {

        //double totAmountOfMoney = 0;
        //int commission = 1;

        Transaction[] TRANS = new Transaction[500];
        int cntOfTrans = 0;
        public int totLifeTimeInSecs = 0;
        Lane[] myLanes = new Lane[Globals.noOfLanes];
        WaitingQueue[] queue = new WaitingQueue[5];
        Fuel myFuel = new Fuel();
        int noOfVehiclesWaiting = -1;
        int y_coordinate_queue = 450;
        public static Random rad;

        public Form1()
        {
            InitializeComponent();
            int x = 340, y = 240;
            //placing the hidden car image at pumps 
            for (int i = 0; i < Globals.noOfLanes; i++)
            {
                myLanes[i] = new Lane(x, y, this);
                for (int j = 0; j < Globals.noOfPumpsInLane; j++)
                    this.Controls.Add(myLanes[i].pb_Car[j]);

                y += 140;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            timer1.Start();
            timer1.Interval = 1000;

            timer2.Start();
            timer2.Interval = 1000;
            // Random r = new Random();
            // label3.Text = (r.Next(1, 100)).ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i, j;
            int min = 0, sec = 0;

            //Running timer on screen
            totLifeTimeInSecs++;
            if (totLifeTimeInSecs >= 60)
            {
                min = totLifeTimeInSecs / 60;
                sec = totLifeTimeInSecs % 60;
            }
            else
            {
                sec = totLifeTimeInSecs;
            }
            lbl_timer.Text = min.ToString() + " : " + sec.ToString();

            //When application will start, set all the pumps availability to Yes(Available)
            if (totLifeTimeInSecs <= 1)
            {
                for (i = 0; i < Globals.noOfLanes; i++)
                {
                    for (j = 0; j < Globals.noOfPumpsInLane; j++)
                    {
                        myLanes[i].myPumps[j].SetAvailability('Y');
                        myLanes[i].myPumps[j].SetAssignedTimeInSecs(totLifeTimeInSecs);
                    }
                }
            }

            if (totLifeTimeInSecs >= 18)
            {
                for (i = 0; i < Globals.noOfLanes; i++)
                {
                    for (j = Globals.noOfPumpsInLane - 1; j >=0 ; j--)
                    {
                        if(myLanes[i].myPumps[j].myVehicle != null)
                        {
                            int timeInSecs = myLanes[i].myPumps[j].GetAssignedTimeInSecs();

                            //check if any pump finished fuelling the vehicle then make it available
                            if ( (totLifeTimeInSecs - ( timeInSecs + myLanes[i].myPumps[j].myVehicle.fuel.GetFuellingProcessTimeInSecs() ) ) > 0)
                            {
                                bool flag = false;
                                //make that particular pump available
                                //update the no of liters dispensed for attendent as well as lifetime of application
                                for(int k = Globals.noOfPumpsInLane - 1; k >= j+1; k--)
                                {
                                    if( myLanes[i].myPumps[k].GetAvailability() == 'N')
                                    {
                                        flag = true;
                                    }
                                }
                                if(flag == false)
                                { 
                                    double fuelDispensed = myLanes[i].myPumps[j].myVehicle.fuel.GetAmtOfFuelToBeFilled(); 
                                    myLanes[i].myPumps[j].SetAvailability('Y');
                                    myLanes[i].myPumps[j].SetAssignedTimeInSecs(totLifeTimeInSecs);
                                    myLanes[i].pb_Car[j].Visible = false;
                                    myLanes[i].myAttendents[j].SetTotNoOfLtrDispensedPerAttendent(fuelDispensed);
                                    Globals.amtOfFuelDispensed += fuelDispensed;
                                    string FuelType = myLanes[i].myPumps[j].myVehicle.fuel.GetFuelType();
                                    switch(FuelType)
                                    {
                                        case "Unleaded":
                                            Globals.amtOfFuel_Unleaded_Dispensed += fuelDispensed;
                                            break;
                                        case "Diesel":
                                            Globals.amtOfFuel_Diesel_Dispensed += fuelDispensed;
                                            break;
                                        case "LPG":
                                            Globals.amtOfFuel_LPG_Dispensed += fuelDispensed;
                                            break;
                                    }
                                    TRANS[cntOfTrans] = new Transaction(myLanes[i].myPumps[j].myVehicle.GetCarType(), 
                                                                        myLanes[i].myPumps[j].myVehicle.fuel.GetAmtOfFuelToBeFilled(), 
                                                                        i, j);
                                    myLanes[i].myPumps[j].myVehicle = null;
                                    cntOfTrans++;
                                    Globals.noOfVehicles++;

                                    //Running Total Fuel Dispensed on screen
                                    lbl_TotFuelDispensed.Text = "Total Fuel Dispensed : " + Globals.amtOfFuelDispensed.ToString();
                                }
                            }
                            else
                            {
                                break;
                                //if Pump 3 is allocated, then there is no point in checking whether pump 1 & 2 is free or not.
                            }
                        }
                    }
                }
            }

            //check availability and allocate a vehicle from queue if its not empty
            if(noOfVehiclesWaiting >= 0)
                   AllocateFromQueue();

        }

        public void AllocateFromQueue()
        {
            int i = 0, j = 0, pumpId = 0, k = 0 ;
            for(i = 0; i < noOfVehiclesWaiting; i++)
            {
                if( totLifeTimeInSecs > ( queue[i].GetInTime() + queue[i].GetLimitTime() ) )
                {
                    //remove the vehicle from waiting queue, because we are unable to service it within its service time
                    Globals.noOfVehiclesLeftWithoutFuelling++;
                    queue[i].queue_pb_Car.Visible = false;
                    queue[i] = null;
                    //shifting vehicles below to top of queue by 1
                    for (k = i; k < noOfVehiclesWaiting - 1; k++)
                    {
                        queue[k] = queue[k + 1];
                        queue[k].queue_pb_Car.Location = new Point(70, y_coordinate_queue - 60);
                    }
                }
                else
                {
                    //find out an available pump 
                    for (j = 0; j < Globals.noOfLanes; j++)
                    {
                        pumpId = myLanes[j].CheckAvailability();
                        if (pumpId != 0)
                        {
                            break;
                        }
                    }
                    if (pumpId > 0)
                    {
                        //assign vehicle to pumpId of lane j
                        //and make that pumps availabilty to busy
                        myLanes[j].myPumps[pumpId - 1].AllocateVehicle(queue[i].GetVehicle()); 
                        myLanes[j].myPumps[pumpId - 1].SetAvailability('N');
                        myLanes[j].myPumps[pumpId - 1].SetAssignedTimeInSecs(totLifeTimeInSecs);
                        myLanes[j].pb_Car[pumpId - 1].Visible = true;

                        //free that car from waiting queue
                        queue[i].queue_pb_Car.Visible = false;
                        for(k = i; k < noOfVehiclesWaiting - 1; k++)
                        {
                            queue[k] = queue[k + 1];
                            queue[k].queue_pb_Car.Location = new Point(70, y_coordinate_queue - 60);
                        }

                        noOfVehiclesWaiting--;
                        y_coordinate_queue -= 60;
                    }
                    //break;
                }
            }
        }

        public void MoveVehiclesInQueue()
        {
            int y = 450;
            for(int i = 0; i < noOfVehiclesWaiting; i++)
            {
                queue[i].queue_pb_Car.Location = new Point(70, y);
                y += 60;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //write all the transaction details to a text file
            string fileName = "transaction.txt";
            if (File.Exists(fileName))
            {
                
                string line = "\n";
                for(int i = 0; i < cntOfTrans; i++)
                {
                    line += TRANS[i].type + "," + TRANS[i].noOfLitres.ToString() + "," + "Lane " + TRANS[i].laneId.ToString() + ", Pump " + TRANS[i].pumpId.ToString() + Environment.NewLine;
                   

                }
                File.WriteAllText(fileName, line);
            }
            Result r = new Result();
            r.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int pumpId = -1, i;

            //reset queue (move vehicles in queue towards front)
            if(noOfVehiclesWaiting >= 0)
                  MoveVehiclesInQueue();

            //create a vehicle at random time interval between 1500 to 2200 miliseconds

            string[] carType = { "Car", "Van", "HGV" };
            rad = new Random();
            int index = rad.Next(carType.Length);
            Vehicle myCar = new Vehicle(carType[index]);
            string[] fuelType = { "Diesel", "LPG", "Unleaded" };

            //set fuel type and capacity of fuel tank for the newly created vehicle
            switch (index)
            {
                case 0:
                           myCar.fuel.SetFuelCapacity(40);
                           int fType = rad.Next(fuelType.Length);                         
                           myCar.fuel.SetFuelType(fuelType[fType]);
                           //label4.Text = "Car" + myCar.fuel.GetFuelType();
                             break;
                case 1:
                          myCar.fuel.SetFuelCapacity(80);
                          fType = rad.Next(fuelType.Length - 1);
                          myCar.fuel.SetFuelType(fuelType[fType]);
                         //label4.Text = "Van" + myCar.fuel.GetFuelType();
                         break;
                case 2:
                          myCar.fuel.SetFuelCapacity(150);
                          myCar.fuel.SetFuelType("Diesel");
                         //label4.Text = "HGV" + myCar.fuel.GetFuelType();
                         break;
            }

            //select amount of fuel of newly created vehicle randomly
            int amtOfFuelInTank = rad.Next(1, (myCar.fuel.GetFuelCapacity() / 4));
            myCar.fuel.SetAmtOfFuelToBeFilled( myCar.fuel.GetFuelCapacity() - amtOfFuelInTank );
            //label4.Text = myCar.GetCarType() + " " + myCar.fuel.GetFuelCapacity() + " " + amtOfFuelInTank + " " + myCar.fuel.GetAmtOfFuelToBeFilled();
            myCar.fuel.SetFuellingProcessTimeInSecs(myCar.fuel.GetAmtOfFuelToBeFilled() / 1.5); //pump fills 1.5ltr per sec, so calculating required time taken to fill  

            if (noOfVehiclesWaiting == -1)
            { 
                for (i = 0; i < Globals.noOfLanes; i++)
                {
                    pumpId = myLanes[i].CheckAvailability();
                    
                    if (pumpId != 0)
                    {
                        break;
                    }
                }
               
                if (pumpId > 0)
                {
                    //assign vehicle to pumpId of lane (i+1)
                    //and make that pumps availabilty to busy
                    myLanes[i].myPumps[pumpId - 1].AllocateVehicle(myCar);
                    myLanes[i].myPumps[pumpId - 1].SetAvailability('N');
                    myLanes[i].myPumps[pumpId - 1].SetAssignedTimeInSecs(totLifeTimeInSecs);
                    myLanes[i].pb_Car[pumpId - 1].Visible = true;

                }
                else
                {
                    //if no pump is available, then, start queue
                    noOfVehiclesWaiting = 0;
                }
            }
            else
            {
                //assign it to waiting queue
                if(noOfVehiclesWaiting < 5)
                { 
                    queue[noOfVehiclesWaiting] = new WaitingQueue(y_coordinate_queue, this);
                    queue[noOfVehiclesWaiting].AddVehicle(myCar);
                    queue[noOfVehiclesWaiting].SetInTime(totLifeTimeInSecs);
                    Random ran = new Random();
                    int time = ran.Next(1000, 2000);
                    queue[noOfVehiclesWaiting].SetLimitTime(time);

                    noOfVehiclesWaiting++;
                    y_coordinate_queue += 60;
                    //label4.Text = noOfVehiclesWaiting.ToString();
                }
                else
                {
                    Globals.noOfVehiclesLeftWithoutFuelling++;
                }
            }

            //find next time interval for creating a vehicle
            Random rd = new Random();
            int interval_sec = rd.Next(1500, 2200);
            timer2.Interval = interval_sec;
        
        }
    }
}
