using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ClassLibrary1
{
    public class VMBenchmark : INotifyPropertyChanged
    {
        public ObservableCollection<VMTime> time { get; set; }
        public ObservableCollection<VMAccuracy> prec { get; set; }
        public float max_HA_EP_coeff
        {
            get
            {
                if (time.Count < 1)
                    return -1;
                float max_coeff = time[0].coeff;
                foreach (VMTime item in time)
                {
                    if (item.coeff > max_coeff)
                        max_coeff = item.coeff;
                }
                return max_coeff;
            }
        }
        public float min_HA_EP_coeff
        {
            get
            {
                if (time.Count < 1)
                    return -1;
                float min_coeff = time[0].coeff;
                foreach (VMTime item in time)
                {
                    if (item.coeff < min_coeff)
                        min_coeff = item.coeff;
                }
                return min_coeff;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public VMBenchmark()
        {
            time = new();
            prec = new();
            time.CollectionChanged += clct_changed;
        }
        private void clct_changed(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(min_HA_EP_coeff)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(max_HA_EP_coeff)));
        }

        [DllImport("..\\..\\..\\..\\WpfApp1\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double MKL_function(int n, double[] arr, float[] arr_floats, float[] res_time,
            int func, double[] res_HA, double[] res_EP, float[] res_HA_float, float[] res_EP_float);
        public void AddVMTime(VMGrid grid)
        {
            VMTime val = new();
            val.parameters = new(grid);
            double[] vec = new double[grid.len];
            float[] vec_float = new float[grid.len];
            for (int i = 0; i < grid.len; i++)
            {
                vec[i] = grid.limits[0] + (i * grid.step);
                vec_float[i] = grid.limits[0] + (i * grid.step);
            }
            double[] res_HA_double = new double[grid.len];
            double[] res_EP_double = new double[grid.len];
            float[] res_HA_float = new float[grid.len];
            float[] res_EP_float = new float[grid.len];
            float[] res_time = new float[3];
            int res = (int)MKL_function(grid.len, vec, vec_float, res_time, (int)(grid.func), res_HA_double, res_EP_double, res_HA_float, res_EP_float);
            if (res != 0)
                throw new InvalidCastException($"MKL_function failed: {res}");
            val.time_vml_HA = res_time[0];
            val.time_vml_EP = res_time[1];
            val.coeff = res_time[2];
            time.Add(val);
        }
        public void AddVMAccuracy(VMGrid grid)
        {
            VMAccuracy val = new();
            val.parameters = new(grid);
            double[] vec = new double[grid.len];
            float[] vec_float = new float[grid.len];
            for (int i = 0; i < grid.len; i++)
            {
                vec[i] = grid.limits[0] + (i * grid.step);
                vec_float[i] = grid.limits[0] + (i * grid.step);
            }
            double[] res_HA_double = new double[grid.len];
            double[] res_EP_double = new double[grid.len];
            float[] res_HA_float = new float[grid.len];
            float[] res_EP_float = new float[grid.len];
            float[] res_time = new float[3];
            int res = (int)MKL_function(grid.len, vec, vec_float, res_time, (int)(grid.func), res_HA_double, res_EP_double, res_HA_float, res_EP_float);
            if (res != 0)
                throw new InvalidCastException($"MKL_function failed: {res}");
            val.max_diff_abs = 0;
            if ((int)grid.func == 0 || (int)grid.func == 2)
            {
                for (int i = 0; i < grid.len; i++)
                {
                    if (Math.Abs(res_HA_double[i] - res_EP_double[i]) > val.max_diff_abs)
                    {
                        val.max_diff_abs = Math.Abs(res_HA_double[i] - res_EP_double[i]);
                        val.max_diff_arg = vec[i];
                        val.HA_value = res_HA_double[i];
                        val.EP_value = res_EP_double[i];
                    }
                }
            }
            else if ((int)grid.func == 1 || (int)grid.func == 3)
            {
                for (int i = 0; i < grid.len; i++)
                {
                    if (Math.Abs(res_HA_float[i] - res_EP_float[i]) > val.max_diff_abs)
                    {
                        val.max_diff_abs = Math.Abs(res_HA_float[i] - res_EP_float[i]);
                        val.max_diff_arg = vec[i];
                        val.HA_value = res_HA_float[i];
                        val.EP_value = res_EP_float[i];
                    }
                }
            }
            prec.Add(val);
        }
    }
}
