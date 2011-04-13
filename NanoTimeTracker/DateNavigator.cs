/*
Nano TimeTracker - a small free windows time-tracking utility
Copyright (C) 2011 Tao Klerks

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace NanoTimeTracker
{
    public partial class DateNavigator : UserControl
    {
        public DateNavigator()
        {
            InitializeComponent();
            datePicker_FilterDate.Value = DateTime.Today;
        }

        public delegate void DateValueChangeHandler();
        public event DateValueChangeHandler DateValueChanged;
        protected void OnDateValueChanged()
        {
            if (DateValueChanged != null)
                DateValueChanged();
        }

        [Browsable(true)]
        [Category("Behavior")]
        [DefaultMinDate()]
        [Description("The minimum date that can be selected through this date navigator.")]
        public DateTime MinDate
        {
            get
            {
                return datePicker_FilterDate.MinDate;
            }
            set
            {
                datePicker_FilterDate.MinDate = value;
                DoDisplayConsistency();
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        [DefaultMaxDate()]
        [Description("The maximum date that can be selected through this date navigator.")]
        public DateTime MaxDate
        {
            get
            {
                return datePicker_FilterDate.MaxDate;
            }
            set
            {
                datePicker_FilterDate.MaxDate = value;
                DoDisplayConsistency();
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        [DefaultMaxDate()]
        [Description("The maximum date that can be selected through this date navigator.")]
        public DateTime DateValue
        {
            get
            {
                return datePicker_FilterDate.Value;
            }
            set
            {
                datePicker_FilterDate.Value = value;
            }
        }

        private void datePicker_FilterDate_ValueChanged(object sender, EventArgs e)
        {
            OnDateValueChanged();
            DoDisplayConsistency();
        }

        private void btn_DatePrev_Click(object sender, EventArgs e)
        {
            datePicker_FilterDate.Value = datePicker_FilterDate.Value.AddDays(-1);
        }

        private void btn_DateNext_Click(object sender, EventArgs e)
        {
            datePicker_FilterDate.Value = datePicker_FilterDate.Value.AddDays(1);
        }

        private void btn_DateLast_Click(object sender, EventArgs e)
        {
            datePicker_FilterDate.Value = datePicker_FilterDate.MaxDate;
        }

        private void DoDisplayConsistency()
        {
            //prev button
            if (datePicker_FilterDate.MinDate.Equals(datePicker_FilterDate.Value) && btn_DatePrev.Enabled)
                btn_DatePrev.Enabled = false;
            else if (!datePicker_FilterDate.MinDate.Equals(datePicker_FilterDate.Value) && !btn_DatePrev.Enabled)
                btn_DatePrev.Enabled = true;

            //next button
            if (datePicker_FilterDate.MaxDate.Equals(datePicker_FilterDate.Value) && btn_DateNext.Enabled)
                btn_DateNext.Enabled = false;
            else if (!datePicker_FilterDate.MaxDate.Equals(datePicker_FilterDate.Value) && !btn_DateNext.Enabled)
                btn_DateNext.Enabled = true;

            //last button
            if ((datePicker_FilterDate.MaxDate.Equals(datePicker_FilterDate.Value) || datePicker_FilterDate.MaxDate.Equals(DateTime.MinValue)) && btn_DateLast.Enabled)
                btn_DateLast.Enabled = false;
            else if (!datePicker_FilterDate.MaxDate.Equals(datePicker_FilterDate.Value) && !btn_DateLast.Enabled)
                btn_DateLast.Enabled = true;
        }

        //simplistic workaround from stack overflow - there must be a better way to do this, but this was fast.
        // http://stackoverflow.com/questions/691035/setting-the-default-value-of-a-datetime-property-to-datetime-now-inside-the-syste
        public class DefaultMinDateAttribute : DefaultValueAttribute
        {
            public DefaultMinDateAttribute()
                : base(DateTime.MinValue)
            {
            }
        }

        public class DefaultMaxDateAttribute : DefaultValueAttribute
        {
            public DefaultMaxDateAttribute()
                : base(DateTime.MaxValue)
            {
            }
        }
    }
}
