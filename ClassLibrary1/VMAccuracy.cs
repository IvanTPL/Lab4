using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public struct VMAccuracy
    {
        public VMGrid parameters { get; set; }
        public double max_diff_abs { get; set; }
        public double max_diff_arg { get; set; }
        public double HA_value { get; set; }
        public double EP_value { get; set; }

        public override string ToString()
        {
            return $"parameters: {parameters}; max value of absolute difference: {max_diff_abs}; argument: {max_diff_arg}; value HA: {HA_value}; value EP: {EP_value}";
        }
    }
}
