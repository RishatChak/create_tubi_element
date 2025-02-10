using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aveva.Core.PMLNet;

namespace Aveva.C.Sharp.Training
{
        [PMLNetCallable()]
    public partial class DatePickerControl : UserControl
    {
        
        [PMLNetCallable()]
        public DatePickerControl()
        {
            InitializeComponent();
        }
        [PMLNetCallable()]
        public void Assign(DatePickerControl that)
        {
            this.dateTimePicker1.Value = that.dateTimePicker1.Value;
        }
        [PMLNetCallable()]
        public void SetDate(string DateString)
        {
            try
            {
                this.dateTimePicker1.Value = DateTime.Parse(DateString);
            }
            catch
            {
                throw new PMLNetException(1000, 1, "String cannot be converted into a date");
            }
        }
        public event PMLNetDelegate.PMLNetEventHandler OnDatePicked;
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (OnDatePicked != null)
            {
                ArrayList args = new ArrayList();
                args.Add(this.dateTimePicker1.Value.ToLongTimeString());
                args.Add(this.dateTimePicker1.Value.ToLongDateString()); OnDatePicked(args);
            }
        }
    }
}
