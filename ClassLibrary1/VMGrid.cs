using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class VMGrid
    {
        public int len { get; set; }
        public float[] limits { get; set; }
        public float step
        {
            get
            {
                return Math.Abs(limits[1] - limits[0]) / len;
            }
        }
        public VMf func { get; set; }

        public VMGrid(int len, float left, float right, VMf func)
        {
            this.len = len;
            this.limits = new float[2];
            limits[0] = left;
            limits[1] = right;  
            this.func = func;
        }

        public VMGrid(VMGrid other)
        {
            len = other.len;
            limits = other.limits;
            func = other.func;
        }
    }
}
