using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.ComponentModel;
using ClassLibrary1;

namespace WpfApp1
{
    public class ViewData : INotifyPropertyChanged
    {
        public VMBenchmark benchmark { get; set; }
        private bool _Changed;
        public bool Changed
        {
            get { return _Changed; }
            set
            {
                if (value != _Changed)
                {
                    _Changed = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Changed)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void clct_changed(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Changed = true;
        }
        public ViewData()
        {
            benchmark = new();
            Changed = false;
            benchmark.time.CollectionChanged += clct_changed;
            benchmark.prec.CollectionChanged += clct_changed;
        }

        public void AddVMTime(VMGrid grid)
        {
            try
            {
                benchmark.AddVMTime(grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddVMAccuracy(VMGrid grid)
        {
            try
            {
                benchmark.AddVMAccuracy(grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool Save(string filename)
        {
            try
            {
                StreamWriter writer = new(filename, false);
                try
                {
                    writer.WriteLine(benchmark.time.Count());
                    foreach (VMTime item in benchmark.time)
                    {
                        writer.WriteLine(item.parameters.len);
                        writer.WriteLine($"{item.parameters.limits[0]:0.00000000}");
                        writer.WriteLine($"{item.parameters.limits[1]:0.00000000}");
                        writer.WriteLine($"{item.parameters.step:0.00000000}");
                        writer.WriteLine((int)item.parameters.func);
                        writer.WriteLine($"{item.time_vml_HA:0.00000000}");
                        writer.WriteLine($"{item.time_vml_EP:0.00000000}");
                        writer.WriteLine($"{item.coeff:0.00000000}");
                    }
                    writer.WriteLine(benchmark.prec.Count());
                    foreach (VMAccuracy item in benchmark.prec)
                    {
                        writer.WriteLine(item.parameters.len);
                        writer.WriteLine($"{item.parameters.limits[0]:0.00000000}");
                        writer.WriteLine($"{item.parameters.limits[1]:0.00000000}");
                        writer.WriteLine($"{item.parameters.step:0.00000000}");
                        writer.WriteLine((int)item.parameters.func);
                        writer.WriteLine($"{item.max_diff_abs:0.00000000}");
                        writer.WriteLine($"{item.max_diff_arg:0.00000000}");
                        writer.WriteLine($"{item.HA_value:0.00000000}");
                        writer.WriteLine($"{item.EP_value:0.00000000}");
                    }
                }
                catch (Exception e)
                {
                    benchmark.time.Clear();
                    benchmark.prec.Clear();
                    MessageBox.Show($"Unable to save: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    writer.Close();
                    return false;
                }
                finally
                {
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                benchmark.time.Clear();
                benchmark.prec.Clear();
                MessageBox.Show($"Unable to save: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool Load(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(filename);
                try
                {
                    benchmark.time.Clear();
                    benchmark.prec.Clear();
                    int cnt = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < cnt; i++)
                    {
                        VMTime item = new();
                        int grid_len = Int32.Parse(reader.ReadLine());
                        float grid_left = float.Parse(reader.ReadLine());
                        float grid_right = float.Parse(reader.ReadLine());
                        float grid_step = float.Parse(reader.ReadLine());
                        VMf grid_func = (VMf)int.Parse(reader.ReadLine());
                        VMGrid grid = new(grid_len, grid_left, grid_right, grid_func);
                        item.parameters = grid;
                        item.time_vml_HA = double.Parse(reader.ReadLine());
                        item.time_vml_EP = double.Parse(reader.ReadLine());
                        item.coeff = float.Parse(reader.ReadLine());
                        benchmark.time.Add(item);
                    }
                    int count2 = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count2; i++)
                    {
                        VMAccuracy item = new();
                        int grid_len = Int32.Parse(reader.ReadLine());
                        float grid_left = float.Parse(reader.ReadLine());
                        float grid_right = float.Parse(reader.ReadLine());
                        float grid_step = float.Parse(reader.ReadLine());
                        VMf grid_CurFunction = (VMf)int.Parse(reader.ReadLine());
                        VMGrid grid = new(grid_len, grid_left, grid_right, grid_CurFunction);
                        item.parameters = grid;
                        item.max_diff_abs = double.Parse(reader.ReadLine());
                        item.max_diff_arg = double.Parse(reader.ReadLine());
                        item.HA_value = double.Parse(reader.ReadLine());
                        item.EP_value = double.Parse(reader.ReadLine());
                        benchmark.prec.Add(item);
                    }
                }
                catch (Exception e)
                {
                    benchmark.time.Clear();
                    benchmark.prec.Clear();
                    MessageBox.Show($"Unable to load: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    reader.Close();
                    return false;
                }
                finally
                {
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                benchmark.time.Clear();
                benchmark.prec.Clear();
                MessageBox.Show($"Unable to load: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
