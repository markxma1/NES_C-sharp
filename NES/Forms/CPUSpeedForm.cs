using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NES
{
    public partial class CPUSpeedForm : Form
    {
        public CPUSpeedForm()
        {
            InitializeComponent();
            chart1.Series.Add("CPU");
            chart1.Series["CPU"].ChartType = SeriesChartType.FastLine;

            chart1.Series["CPU"].ChartArea = "ChartArea1";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double speed = NES_Console.getCPUSpeed();
            CPUSpeed.Text = speed.ToString();
            chart1.Series["CPU"].Points.AddY(speed);
        }
    }
}
