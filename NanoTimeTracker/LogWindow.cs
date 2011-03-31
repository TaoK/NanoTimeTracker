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
    public partial class LogWindow : Form
    {
        public LogWindow()
        {
            InitializeComponent();
            dataGridView_TaskLogList.Columns["StartDateTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            dataGridView_TaskLogList.Columns["EndDateTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            TaskDialog = new TaskInput();
        }

        private DateTime _taskInProgressStartTime;
        private string WorkingOnTask = "";
        private bool WorkingBillable = true;

        private string LogFilePath;
        private bool LogTopics;
        private bool logByDate;
        private bool _displayingDialog;
        private bool _taskInProgress;

        private DateTime CurrentFileDate;
        private double previousHours;
        private double previousBillableHours;

        private Icon TaskInProgressIcon;
        private Icon NoTaskActiveIcon;
        private TaskInput TaskDialog;

        #region Event Handlers

        private void LogWindow_Load(object sender, System.EventArgs e)
        {
            TaskInProgressIcon = new Icon(typeof(Icons.IconTypePlaceholder), "view-calendar-tasks-combined.ico");
            NoTaskActiveIcon = new Icon(typeof(Icons.IconTypePlaceholder), "edit-clear-history-2-combined.ico");

            WindowHacker.DisableCloseMenu(this);

            LogFilePath = Properties.Settings.Default.LogFilePath;
            if (LogFilePath.IndexOf("<DATE>") > 0)
                logByDate = true;
            else
                logByDate = false;
            if (Properties.Settings.Default.LogTopics)
                LogTopics = true;

            InitializeData();

            UpdateControlDisplayConsistency();
        }

        private void LogWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (timer_StatusUpdate.Enabled == true)
            {
                PromptTaskEnd();
                //TODO: handle cancellation to avoid exit?
            }
        }

        private void btn_Stop_Click(object sender, System.EventArgs e)
        {
            PromptTaskEnd();
        }

        private void btn_Start_Click(object sender, System.EventArgs e)
        {
            PromptTaskStart();
        }

        private void timer_StatusUpdate_Tick(object sender, System.EventArgs e)
        {
            UpdateStatusDisplay();
        }

        private void LogWindow_Resize(object sender, System.EventArgs e)
        {
            UpdateControlDisplayConsistency();
        }

        private void notifyIcon1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (e.Clicks == 1) && (timer_NotifySingleClick.Enabled == false))
                timer_NotifySingleClick.Start();
        }

        private void notifyIcon1_DoubleClick(object sender, System.EventArgs e)
        {
            ToggleLogWindowDisplay();
        }

        private void timer_NotifySingleClick_Tick(object sender, System.EventArgs e)
        {
            timer_NotifySingleClick.Stop();
            if (timer_StatusUpdate.Enabled == false)
            {
                PromptTaskStart();
            }
            else
            {
                PromptTaskEnd();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            CloseConfirmationWindow closeDialog = new CloseConfirmationWindow();
            closeDialog.ShowDialog();
            closeDialog.Dispose();
             */
            this.WindowState = FormWindowState.Minimized;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string caption = "Confirm Delete";
            DialogResult result;

            result = MessageBox.Show(this, "Are you SURE you want to delete the logfile?", caption, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                result = MessageBox.Show(this, "Really Sure?", caption, MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if (timer_StatusUpdate.Enabled == true)
                    {
                        PromptTaskEnd();

                        //TODO: handle cancellation
                    }

                    DeleteLogs();
                }

            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: IMPLEMENT OPTIONS FORM 

            string messageText;
            messageText = "Current LogFile Path: \u000D\u000A" + LogFilePath;
            messageText = messageText + "\u000D\u000A\u000D\u000A" + "Topic Logging: " + LogTopics.ToString();
            messageText = messageText + "\u000D\u000A\u000D\u000A" + "To change these options, please use regedit.";

            MessageBox.Show(messageText, "LogFile Path", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void dataGridView_TaskLogList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_TaskLogList.Columns[e.ColumnIndex].Name == "StartDateTime"
                || dataGridView_TaskLogList.Columns[e.ColumnIndex].Name == "EndDateTime")
            {
                //looks like we should consider it, call the function that does the work.
                MaintainDurationByDataGrid(e.RowIndex);
            }

            //persist changes
            SaveTimeTrackingDB();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox box = new AboutBox();
            box.ShowDialog();
            box.Dispose();
        }

        private void onlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: implement URL Launch to http://www.architectshack.com/NanoTimeTracker.ashx
        }

        private void openLogWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleLogWindowDisplay();
        }

        private void startTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PromptTaskStart();
        }

        private void stopEditTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PromptTaskEnd();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion


        #region Local Methods

        private void UpdateControlDisplayConsistency()
        {
            //task bar hiding
            if (this.WindowState == FormWindowState.Minimized && this.Visible)
                this.Visible = false;

            //"Open Log Window" contex strip option display
            if (this.WindowState != FormWindowState.Minimized && openLogWindowToolStripMenuItem.Enabled)
                openLogWindowToolStripMenuItem.Enabled = false;

            if (_displayingDialog && openLogWindowToolStripMenuItem.Enabled)
                openLogWindowToolStripMenuItem.Enabled = false;

            if (this.WindowState == FormWindowState.Minimized && !_displayingDialog && !openLogWindowToolStripMenuItem.Enabled)
                openLogWindowToolStripMenuItem.Enabled = true;

            //buttons
            if (_taskInProgress && btn_Start.Enabled)
                btn_Start.Enabled = false;
            else if (!_taskInProgress && !btn_Start.Enabled)
                btn_Start.Enabled = true;

            if (!_taskInProgress && btn_Stop.Enabled)
                btn_Stop.Enabled = false;
            else if (_taskInProgress && !btn_Stop.Enabled)
                btn_Stop.Enabled = true;

            //context strips
            if (_taskInProgress && startTaskToolStripMenuItem.Enabled)
                startTaskToolStripMenuItem.Enabled = false;
            else if (!_taskInProgress && !startTaskToolStripMenuItem.Enabled)
                startTaskToolStripMenuItem.Enabled = true;

            if (!_taskInProgress && stopEditTaskToolStripMenuItem.Enabled)
                stopEditTaskToolStripMenuItem.Enabled = false;
            else if (_taskInProgress && !stopEditTaskToolStripMenuItem.Enabled)
                stopEditTaskToolStripMenuItem.Enabled = true;

            //icons
            if (_taskInProgress && notifyIcon1.Icon != TaskInProgressIcon)
                notifyIcon1.Icon = TaskInProgressIcon;
            else if (!_taskInProgress && notifyIcon1.Icon != NoTaskActiveIcon)
                notifyIcon1.Icon = NoTaskActiveIcon;
            
            if (_taskInProgress && this.Icon != TaskInProgressIcon)
                this.Icon = TaskInProgressIcon;
            else if (!_taskInProgress && this.Icon != NoTaskActiveIcon)
                this.Icon = NoTaskActiveIcon;

            //SysTray Balloon
            if (!_taskInProgress && !notifyIcon1.Text.Equals("Nano TimeLogger - no active task"))
                notifyIcon1.Text = "Nano TimeLogger - no active task";

            //TODO: determine when this actually needs to be set (just initial load??)
            WindowHacker.DisableCloseMenu(this);
        }

        private void ToggleLogWindowDisplay()
        {
            timer_NotifySingleClick.Stop();
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (TaskDialog.Visible)
                {
                    //bring the task dialog to the foreground
                    WindowHacker.SetForegroundWindow(TaskDialog.Handle);
                }
                else
                {
                    //no task dialog around, so bring up the main window
                    this.Show();
                    WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                this.Hide();
                WindowState = FormWindowState.Minimized;
            }
        }

        private void PromptTaskStart()
        {
            //double-check whether we really should be displaying
            if (!_displayingDialog)
            {
                _displayingDialog = true;
                UpdateControlDisplayConsistency();

                bool doAction;
                if (LogTopics)
                {
                    //only relevant if we're not minimized, but seems to do no harm.
                    this.Activate();

                    TaskDialog.SetPrompt(DateTime.Now, null, WorkingOnTask, null);
                    if (TaskDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        WorkingOnTask = TaskDialog.TaskDescription;
                        doAction = true;
                    }
                    else
                        doAction = false;
                }
                else
                    doAction = true;

                //in the absence of an appropriate UI
                WorkingBillable = true;

                if (doAction)
                {
                    //save and switch day if appropriate
                    SaveTimeTrackingDB(true);

                    //retrieve existing for total display
                    CheckHourTotals();

                    //set started state
                    _taskInProgress = true;
                    _taskInProgressStartTime = System.DateTime.Now;

                    //Log task
                    LogText(Utils.ExpandPath(LogFilePath, DateTime.Now, false), String.Format("{0:yyyy-MM-dd HH:mm:ss}", _taskInProgressStartTime) + "\t");

                    //start tracking new task in DB
                    DataRow newRow = dataSet1.DataTable1.NewRow();
                    newRow["StartDateTime"] = _taskInProgressStartTime;
                    if (WorkingOnTask != "") newRow["TaskName"] = WorkingOnTask;
                    newRow["BillableFlag"] = WorkingBillable;
                    dataSet1.DataTable1.Rows.Add(newRow);
                    SaveTimeTrackingDB();

                    //LogBox
                    txt_LogBox.Text = txt_LogBox.Text + "Starting... " + _taskInProgressStartTime.ToString() + ";";
                    if (WorkingOnTask != "") txt_LogBox.Text = txt_LogBox.Text + " " + WorkingOnTask;
                    txt_LogBox.Text = txt_LogBox.Text + " \u000D\u000A";

                    //display timer...
                    lbl_WorkingTimeValue.Text = "Starting...";
                    timer_StatusUpdate.Start();
                }

                //release dialog "lock" and update display
                _displayingDialog = false;
                UpdateControlDisplayConsistency();
            }
        }

        private void PromptTaskEnd()
        {
            //double-check whether we really should be displaying
            if (!_displayingDialog)
            {
                _displayingDialog = true;
                UpdateControlDisplayConsistency();

                bool doAction;
                DateTime taskEndTime = DateTime.Now;

                if (LogTopics)
                {
                    //only relevant if we're not minimized, but seems to do no harm.
                    this.Activate();

                    TaskDialog.SetPrompt(_taskInProgressStartTime, taskEndTime, WorkingOnTask, null);
                    if (TaskDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        WorkingOnTask = TaskDialog.TaskDescription;
                        doAction = true;
                    }
                    else
                        doAction = false;
                }
                else
                    doAction = true;

                if (doAction)
                {
                    //Log action...
                    LogText(Utils.ExpandPath(LogFilePath, _taskInProgressStartTime, false), String.Format("{0:yyyy-MM-dd HH:mm:ss}", taskEndTime) + "\t" + String.Format("{0:0.00}", taskEndTime.Subtract(_taskInProgressStartTime).TotalHours) + "\t" + WorkingOnTask + "\u000D\u000A");

                    DataRow[] rowToClose = dataSet1.DataTable1.Select("StartDateTime = #" + _taskInProgressStartTime.ToString() + "#");
                    if (rowToClose.Length == 0)
                        rowToClose = dataSet1.DataTable1.Select("StartDateTime Is Not Null And EndDateTime Is Null", "StartDateTime Desc");
                    if (rowToClose.Length > 0)
                    {
                        DateTime realStartedTime = (DateTime)rowToClose[0]["StartDateTime"];
                        if (rowToClose.Length > 1)
                            MessageBox.Show("More than one unfinished task found. Using more recent one: " + String.Format("{0:yyyy-MM-dd HH:mm:ss}", taskEndTime), "Multiple log entries", MessageBoxButtons.OK);
                        rowToClose[0]["EndDateTime"] = taskEndTime;
                        rowToClose[0]["TaskName"] = WorkingOnTask;
                        rowToClose[0]["BillableFlag"] = WorkingBillable;
                        rowToClose[0]["TimeTaken"] = taskEndTime.Subtract(realStartedTime).TotalHours;

                        //save and switch day if appropriate
                        SaveTimeTrackingDB(true);
                    }
                    else
                    {
                        MessageBox.Show("Could not find log entry to complete! Task Lost.", "Missing log entry", MessageBoxButtons.OK);
                    }

                    //Textbox display...
                    txt_LogBox.Text = txt_LogBox.Text + "Stopping... " + System.DateTime.Now.ToString() + ";";
                    if (WorkingOnTask != "") txt_LogBox.Text = txt_LogBox.Text + " " + WorkingOnTask;
                    txt_LogBox.Text = txt_LogBox.Text + " \u000D\u000A";

                    //display timer...
                    timer_StatusUpdate.Stop();
                    lbl_WorkingTimeValue.Text = "00:00:00";

                    //status flag
                    _taskInProgress = false;
                }

                //release dialog "lock" and update display
                _displayingDialog = false;
                UpdateControlDisplayConsistency();
            }
        }

        private void UpdateStatusDisplay()
        {
            System.TimeSpan TimeSinceTaskStart;
            System.TimeSpan TotalTimeToday;
            System.TimeSpan TotalBillableTimeToday;
            String FriendlyTimeSinceTaskStart;
            String FriendlyTimeToday;
            String FriendlyBillableTimeToday;
            TimeSinceTaskStart = System.DateTime.Now.Subtract(_taskInProgressStartTime);
            TotalTimeToday = TimeSinceTaskStart + new TimeSpan((int)Math.Floor(previousHours), (int)Math.Floor((previousHours * 60) % 60), (int)Math.Floor((previousHours * 60 * 60) % 60));
            TotalBillableTimeToday = TimeSinceTaskStart + new TimeSpan((int)Math.Floor(previousBillableHours), (int)Math.Floor((previousBillableHours * 60) % 60), (int)Math.Floor((previousBillableHours * 60 * 60) % 60));
            FriendlyTimeSinceTaskStart = String.Format("{0:00}", TimeSinceTaskStart.Hours) + ":" + String.Format("{0:00}", TimeSinceTaskStart.Minutes) + ":" + String.Format("{0:00}", TimeSinceTaskStart.Seconds);
            FriendlyTimeToday = String.Format("{0:00}", TotalTimeToday.Hours) + ":" + String.Format("{0:00}", TotalTimeToday.Minutes) + ":" + String.Format("{0:00}", TotalTimeToday.Seconds);
            FriendlyBillableTimeToday = String.Format("{0:00}", TotalBillableTimeToday.Hours) + ":" + String.Format("{0:00}", TotalBillableTimeToday.Minutes) + ":" + String.Format("{0:00}", TotalBillableTimeToday.Seconds);

            lbl_WorkingTimeValue.Text = FriendlyTimeSinceTaskStart;
            lbl_TimeTodayValue.Text = FriendlyTimeToday;
            lbl_BillableTimeTodayValue.Text = FriendlyBillableTimeToday;
            notifyIcon1.Text = "Time Logger - " + FriendlyTimeSinceTaskStart;
        }

        #endregion


        #region Data-Management Methods - TODO: send to dedicated class.

        private void InitializeData()
        {
            CurrentFileDate = DateTime.Now;

            //Recover any existing info for today...
            string effectivePath = Utils.ExpandPath(LogFilePath, DateTime.Now, true);
            if (System.IO.File.Exists(effectivePath))
                dataSet1.ReadXml(effectivePath);

            //Recover running state if required...
            DataRow[] recentrows = dataSet1.DataTable1.Select("StartDateTime Is Not Null", "StartDateTime Desc");
            if (recentrows.Length > 0)
            {
                DataRow lastRow = recentrows[0];
                if (lastRow["EndDateTime"] == DBNull.Value)
                {
                    //set running state in app
                    _taskInProgressStartTime = (DateTime)lastRow["StartDateTime"];
                    _taskInProgress = true;
                    if (lastRow["TaskName"] != DBNull.Value)
                        WorkingOnTask = (string)lastRow["TaskName"];
                    WorkingBillable = (bool)lastRow["BillableFlag"];

                    //display timer...
                    lbl_WorkingTimeValue.Text = "Recovering to Running...";
                    timer_StatusUpdate.Start();
                }
            }

            CheckHourTotals();
        }

        private void LogText(String Path, String Text)
        {
            CheckLogHeader(Path);
            System.IO.TextWriter TextLogFile;
            TextLogFile = new System.IO.StreamWriter(Path, true);
            TextLogFile.Write(Text);
            TextLogFile.Close();
            TextLogFile.Dispose();
        }

        private void CheckLogHeader(String Path)
        {
            if (!System.IO.File.Exists(Path))
            {
                System.IO.TextWriter TextLogFile;
                TextLogFile = new System.IO.StreamWriter(Path, true);
                TextLogFile.WriteLine("StartTime\tEndTime\tDuration\tTask");
                TextLogFile.Close();
            }
        }

        private void MaintainDurationByDataGrid(int RowIndex)
        {
            DateTime from;
            DateTime to;
            if (dataGridView_TaskLogList.Rows[RowIndex].Cells["StartDateTime"].Value.ToString() != "")
                from = (System.DateTime)(System.DateTime)dataGridView_TaskLogList.Rows[RowIndex].Cells["StartDateTime"].Value;
            else
                from = System.DateTime.MinValue;
            if (dataGridView_TaskLogList.Rows[RowIndex].Cells["EndDateTime"].Value.ToString() != "")
                to = (System.DateTime)dataGridView_TaskLogList.Rows[RowIndex].Cells["EndDateTime"].Value;
            else
                to = System.DateTime.MinValue;
            if (from != System.DateTime.MinValue && to != System.DateTime.MinValue)
            {
                dataGridView_TaskLogList.Rows[RowIndex].Cells["TimeTaken"].Value = to.Subtract(from).TotalHours;
            }
            else
            {
                dataGridView_TaskLogList.Rows[RowIndex].Cells["TimeTaken"].Value = DBNull.Value;
            }
        }

        private void SaveTimeTrackingDB()
        {
            SaveTimeTrackingDB(false);
        }

        private void SaveTimeTrackingDB(bool AllowDateSwitch)
        {
            DateTime CurrentDate = DateTime.Now;

            //If we have a valid date and path set, log. Otherwise we must be starting up...
            if (CurrentFileDate != null && LogFilePath != null)
            {
                //Write the dataset
                string effectivePath = Utils.ExpandPath(LogFilePath, CurrentFileDate, true);
                dataSet1.WriteXml(effectivePath);

                //if the day has ended, clear/start anew
                if (CurrentFileDate.Date != CurrentDate.Date && AllowDateSwitch)
                {
                    dataSet1.DataTable1.Rows.Clear();
                    CurrentFileDate = CurrentDate;
                    effectivePath = Utils.ExpandPath(LogFilePath, CurrentFileDate, true);
                    dataSet1.WriteXml(effectivePath);
                }
            }
        }

        private void CheckHourTotals()
        {
            //retrieve existing for total display
            object previousHoursResult = dataSet1.DataTable1.Compute("Sum(TimeTaken)", null);
            if (previousHoursResult != DBNull.Value)
                previousHours = (double)previousHoursResult;
            else
                previousHours = 0;

            object previousHoursBillableResult = dataSet1.DataTable1.Compute("Sum(TimeTaken)", "BillableFlag = True");
            if (previousHoursBillableResult != DBNull.Value)
                previousBillableHours = (double)previousHoursBillableResult;
            else
                previousBillableHours = 0;
        }

        private void DeleteLogs()
        {
            //Delete the file
            string effectivePath = Utils.ExpandPath(LogFilePath, DateTime.Now, false);
            if (System.IO.File.Exists(effectivePath))
            {
                System.IO.File.Delete(effectivePath);
            }
            else
            {
                MessageBox.Show(this, "No text file to delete.", "Log Deletion", MessageBoxButtons.OK);
            }

            //Delete the records (and save)
            if (dataSet1.DataTable1.Rows.Count > 0)
            {
                dataSet1.DataTable1.Rows.Clear();
                SaveTimeTrackingDB();
            }
            else
            {
                MessageBox.Show(this, "No datagrid entries to delete.", "Log Deletion", MessageBoxButtons.OK);
            }
        }

        #endregion

    }
}
