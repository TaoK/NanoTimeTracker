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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NanoTimeTracker.Dialogs
{
    public partial class ExportOptions : Form
    {
        public ExportOptions()
        {
            InitializeComponent();
            dateTimePicker_FromDate.MaxDate = DateTime.Today;
            dateTimePicker_ToDate.MaxDate = DateTime.Today;
        }

        public DateTime FromDate
        {
            get
            {
                return dateTimePicker_FromDate.Value;
            }
            set
            {
                dateTimePicker_FromDate.Value = value;
            }
        }

        public DateTime ToDate
        {
            get
            {
                return dateTimePicker_ToDate.Value;
            }
            set
            {
                dateTimePicker_ToDate.Value = value;
            }
        }

        public bool BillableOnly
        {
            get
            {
                return chk_BillableOnly.Checked;
            }
            set
            {
                chk_BillableOnly.Checked = value;
            }
        }

        private void dateTimePicker_FromDate_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker_FromDate.Value > dateTimePicker_ToDate.Value)
                dateTimePicker_ToDate.Value = dateTimePicker_FromDate.Value;
        }

        private void dateTimePicker_ToDate_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker_ToDate.Value < dateTimePicker_FromDate.Value)
                dateTimePicker_FromDate.Value = dateTimePicker_ToDate.Value;
        }
    }
}
