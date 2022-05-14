using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public struct VMTime
    {
        public VMGrid parameters { get; set; }
        public double time_vml_HA { get; set; }
        public double time_vml_EP { get; set; }
        public float coeff { get; set; }
        override public string ToString()
        {
            return $"parameters: {parameters}; time HA: {time_vml_HA}; time EP: {time_vml_EP}; coefficient = {coeff}";
        }
    }
}
