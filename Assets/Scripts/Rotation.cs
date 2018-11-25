using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class Rotation
    {
        public int XAngle { get; set; }
        public int YAngle { get; set; }
        public int ZAngle { get; set; }

        public Rotation(int xangle, int yangle,int zangle)
        {
            XAngle = xangle;
            YAngle = yangle;
            ZAngle = zangle;
        }

    }
}
