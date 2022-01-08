using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace PetrolStation
{
    class Lane
    {
        
        public Pump[] myPumps = new Pump[Globals.noOfPumpsInLane];
        public Attendee[] myAttendents = new Attendee[Globals.noOfPumpsInLane];   //1 attendent for every pump
        public PictureBox[] pb_Car = new PictureBox[Globals.noOfPumpsInLane];

        public Lane(int x, int y, Form myForm)
        {

            for (int i = 0; i < Globals.noOfPumpsInLane; i++)
            {
                myPumps[i] = new Pump();
                myAttendents[i] = new Attendee();
            }

            //runtime creation of car images


            for (int i = 0; i < Globals.noOfPumpsInLane; i++)
            {
               
                pb_Car[i] = new PictureBox();
               
                pb_Car[i].Location = new Point(x, y);
                pb_Car[i].Size = new Size(100, 50);

                pb_Car[i].Image = Properties.Resources.car;
                pb_Car[i].SizeMode = PictureBoxSizeMode.CenterImage;
                pb_Car[i].Visible = false;
                x += 340;
            }
        }
        public int CheckAvailability()
        {
            //started checking the availability of pump in that lane in decreasing order
            //because if pump 1, 2 and 3 all are free then will not assign the vehicle to pump1
            //which will block the way to pump 2 and 3.Instead will assign the vehicle to pump3.
            int i, k;
            for (i = Globals.noOfPumpsInLane - 1; i >= 0; i--)
            {
                
                if (myPumps[i].GetAvailability() == 'Y')
                {
                    //check if pump1 and 2 are not busy so that waiting car can drive to pump3
                    
                    for (k = i - 1; k >= 0; k--)
                    {
                        if (myPumps[k].GetAvailability() != 'Y')
                            break;
                    }
                    if (k == -1)
                        return i + 1;
                    else
                        return 0;
                }
            }
            //if none of pump is available then will return 0;
            return 0;
        }

    }
}
