using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace PetrolStation
{
    public class WaitingQueue
    {
        int inTime;
        int limitTime;
        public PictureBox queue_pb_Car;
        Vehicle myVehicle;
   
        public WaitingQueue(int y, Form myForm)
        {
            myVehicle = new Vehicle();
            queue_pb_Car = new PictureBox();
            queue_pb_Car.Location = new Point(30, y);
            queue_pb_Car.Size = new Size(100, 50);
            queue_pb_Car.Image = Properties.Resources.car;
            queue_pb_Car.SizeMode = PictureBoxSizeMode.CenterImage;
            queue_pb_Car.Visible = true;
            myForm.Controls.Add(queue_pb_Car);
        }
        public void SetInTime(int t)
        {
            inTime = t;
        }
        public int GetInTime()
        {
            return inTime;
        }
        public void SetLimitTime(int t)
        {
            limitTime = t;
        }
        public int GetLimitTime()
        {
            return limitTime;
        }
        public void AddVehicle(Vehicle v)
        {
            myVehicle = v;

        }
        public Vehicle GetVehicle()
        {
            return myVehicle;
        }
    }
}
