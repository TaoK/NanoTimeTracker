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

        public bool TaskBillable
        {
            get { return chk_Billable.Checked; }
            set { chk_Billable.Checked = value; }
        }

        public DateTime? TaskStartDate
        {
            get 
            {
                //TODO: think about cultures...
                DateTime startDate;
                if (DateTime.TryParse(txt_StartedAt.Text, out startDate))
                    return startDate;
                else
                    return null;
            }
            set 
            {
                //TODO: think about cultures...
                if (value != null)
                    txt_StartedAt.Text = value.Value.ToString("yyyy-MM-dd HH:mm:ss");
                else
                    txt_StartedAt.Text = "";
            }
        }

        public DateTime? TaskEndDate
        {
            get 
            {
                //TODO: think about cultures...
                DateTime endDate;
                if (DateTime.TryParse(txt_CompletedAt.Text, out endDate))
                    return endDate;
                else
                    return null;
            }
            set 
            {
                //TODO: think about cultures...
                if (value != null)
                    txt_CompletedAt.Text = value.Value.ToString("yyyy-MM-dd HH:mm:ss");
                else
                    txt_CompletedAt.Text = "";
            }
        }

        public void SetPrompt(DateTime startTime, DateTime? endTime, string taskDescription, string taskCategory, bool? taskBillable)
        {
            if (endTime == null)
            {
                this.Text = "Task Entry - New Task";
                lbl_EntryPrompt.Text = "New task details:";
            }
            else
            {
                this.Text = "Task Entry - Confirm Task Details / End Task";
                lbl_EntryPrompt.Text = "Task details:";
            }

            TaskStartDate = startTime;
            TaskEndDate = endTime;

            if (taskDescription != null)
                TaskDescription = taskDescription;
            else
                TaskDescription = "";

            if (taskCategory != null)
                TaskCategory = taskCategory;
            else
                TaskCategory = "";

            if (taskBillable != null)
                TaskBillable = taskBillable.Value;
            else
                TaskBillable = true;


        }

        private void TaskInput_Load(object sender, EventArgs e)
        {
            //grab focus back from the system tray, if necessary
            WindowHacker.SetForegroundWindow(this.Handle);
            //for some reason isVisible is false on second call of ShowDialog on the same form
            this.Show();
            //make sure we focus the right control for entry
            txt_Description.Focus();
        }

        private void txt_StartedAt_TextChanged(object sender, EventArgs e)
        {
            CalculateDuration();
        }

        private void txt_CompletedAt_TextChanged(object sender, EventArgs e)
        {
            chk_TaskCompleted.Checked = !string.IsNullOrEmpty(txt_CompletedAt.Text);
            CalculateDuration();
        }

        private void chk_TaskCompleted_CheckedChanged(object sender, EventArgs e)
        {
            CalculateDuration();
        }

        private void CalculateDuration()
        {
            //the checkbox to textbox relation depends on the PRESENCE of a value
            if (chk_TaskCompleted.Checked && string.IsNullOrEmpty(txt_CompletedAt.Text))
                TaskEndDate = DateTime.Now;
            else if (!chk_TaskCompleted.Checked && !string.IsNullOrEmpty(txt_CompletedAt.Text))
                TaskEndDate = null;

            //the presence of a duration description depends on the VALIDITY of a value
            if (TaskStartDate != null && TaskEndDate != null && TaskStartDate.Value < TaskEndDate.Value && chk_TaskCompleted.Checked)
                chk_TaskCompleted.Text = string.Format("Completed (duration: {0:HH:mm:ss})", new DateTime((TaskEndDate.Value - TaskStartDate.Value).Ticks));
            else
                chk_TaskCompleted.Text = "Completed";
        }

        private void TaskInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(txt_CompletedAt.Text) && TaskEndDate == null)
                {
                    MessageBox.Show("The provided completion time is not a valid date/time.", "Invalid Completion Time", MessageBoxButtons.OK);
                    e.Cancel = true;
                }

                if (TaskStartDate == null)
                {
                    MessageBox.Show("A start time must be provided.", "Missing Start Time", MessageBoxButtons.OK);
                    e.Cancel = true;
                }
            }
        }

    }
}
