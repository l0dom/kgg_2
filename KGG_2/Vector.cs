using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGG_2
{
    class Vector
    {
        private double x, y;
        public double X
        {
            get { return x; }
        }
        public double Y
        {
            get { return y; }
        }
        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector(Vector a, Vector b)
        {
            this.x = b.x - a.x;
            this.y = b.y - a.y;
        }
        public bool Equals (Object obj)
        {
            if (obj.GetType() != typeof(Vector))
                return false;
            var point = (Vector)obj;
            return x == point.x && y == point.y;
        }
    }
}
