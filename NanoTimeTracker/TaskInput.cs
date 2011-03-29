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

namespace NanoTimeTracker
{
    public partial class TaskInput : Form
    {
        public TaskInput()
        {
            InitializeComponent();
        }

        public string TaskDescription
        {
            get { return txt_Description.Text; }
            set { txt_Description.Text = value; }
        }

        public string TaskCategory
        {
            get { return txt_Category.Text; }
            set { txt_Category.Text = value; }
        }

        public void SetPrompt(DateTime startTime, DateTime? endTime, string taskDescription, string taskCategory)
        {
            if (endTime == null)
            {
                this.Text = "Task Entry - New Task";
                lbl_EntryPrompt.Text = string.Format("Details for new task starting at {0:yyyy-MM-dd HH:mm:ss}:", startTime);
            }
            else
            {
                this.Text = "Task Entry - Confirm Task Details";
                lbl_EntryPrompt.Text = string.Format("Details for task started at {0:yyyy-MM-dd HH:mm:ss} (duration: {1:HH:mm:ss}):", startTime, new DateTime(endTime.Value.Subtract(startTime).Ticks));
            }

            if (taskDescription != null)
                TaskDescription = taskDescription;
            else
                TaskDescription = "";

            if (taskCategory != null)
                TaskCategory = taskCategory;
            else
                TaskCategory = "";
        }

        private void TaskInput_Load(object sender, EventArgs e)
        {
            //grab focus back from the system tray, if necessary
            WindowHacker.SetForegroundWindow(this.Handle);
            //for some reason isVisible is false on second call of ShowDialog on the same form
            this.Show();
            //make sure we focus the right control
            txt_Description.Focus();
        }

    }
}
