using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGG_2
{
    class Segment
    {
        private Vector begin, end;
        public Vector Begin
        {
            get { return begin; }
        }
        public Vector End
        {
            get {return end;}
        }
        public Segment(Vector begin, Vector end)
        {
            this.begin = begin;
            this.end = end;
        }
        public Segment ResizeTo(double length)
        {
            double coefficient = Length() / length;
            return new Segment(begin, new Vector(end.X / coefficient, end.Y / coefficient));
        }
        public double Length()
        {
            return Math.Sqrt(Math.Pow((end.X - begin.X),2) + Math.Pow((end.Y - begin.Y),2));
        }
    }
}
