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
        public TaskInput(DateTime startTime, DateTime? endTime, string taskDescription, string taskCategory)
        {
            InitializeComponent();

            if (endTime == null)
            {
                this.Text = "Task Entry - New Task";
                lbl_EntryPrompt.Text = string.Format("Details for new task starting at {0:yyyy-MM-dd HH:mm:ss}:", startTime);
            }
            else
            {
                this.Text = "Task Entry - Confirm Task Details";
                lbl_EntryPrompt.Text = string.Format("Details for task started at {0:yyyy-MM-dd HH:mm:ss} ({0:0.00} hours):", startTime, endTime.Value.Subtract(startTime).TotalHours);
            }

            if (taskDescription != null)
                TaskDescription = taskDescription;

            if (taskCategory != null)
                TaskCategory = taskCategory;
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

    }
}
