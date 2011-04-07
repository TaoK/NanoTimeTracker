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
using ManagedWinapi;

namespace NanoTimeTracker
{
    public partial class LogWindow : Form
    {
        public LogWindow()
        {
            InitializeComponent();
            dataGridView_TaskLogList.Columns["StartDateTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            dataGridView_TaskLogList.Columns["EndDateTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            TaskDialog = new Dialogs.TaskInput();
        }

        //State
        private bool _displayingDialog;
        private bool _taskInProgress;
        private DateTime _taskInProgressStartTime;
        private string _taskInProgressDescription = "";
        private string _taskInProgressCategory = "";
        private bool _taskInProgressTimeBillable = true;
        private double _previousHours;
        private double _previousBillableHours;

        //TEMP
        bool _exiting = false;

        //Resources
        private Icon TaskInProgressIcon;
        private Icon NoTaskActiveIcon;
        private Dialogs.TaskInput TaskDialog;
        private Hotkey TaskHotKey;

        #region Event Handlers

        private void LogWindow_Load(object sender, System.EventArgs e)
        {
            TaskInProgressIcon = new Icon(typeof(Icons.IconTypePlaceholder), "view-calendar-tasks-combined.ico");
            NoTaskActiveIcon = new Icon(typeof(Icons.IconTypePlaceholder), "edit-clear-history-2-combined.ico");

            TaskHotKey = new Hotkey();
            TaskHotKey.WindowsKey = true;
            TaskHotKey.KeyCode = System.Windows.Forms.Keys.T;
            TaskHotKey.HotkeyPressed += new EventHandler(TaskHotKey_HotkeyPressed);
            try
            {
                TaskHotKey.Enabled = true;
            }
            catch (ManagedWinapi.HotkeyAlreadyInUseException)
            {
                //TODO: Make this an option, and simply disable if fail on start/load.
                // -> any error to be displayed should be displayed in real-time on options screen, in a special popup.
                // -> use ManagedWinapi.ShortcutBox
            }

            if (!Properties.Settings.Default.upgraded)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.upgraded = true;
                Properties.Settings.Default.Save();
                //TODO: Add data migration too.
                //TODO: ADD Confirmation window asking about deleting previous settings, if there was really an upgrade
            }

            InitializeData();
            UpdateControlDisplayConsistency();
        }

        private void LogWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_exiting)
            {
                //behave like Windows Messenger and other systray-based programs, hijack exit for close and explain
                e.Cancel = true;
                LogWindowCloseWithWarning();
            }
            else
            {
                if (DismissableWarning("Exiting Application", "You are exiting the Nano TimeTracker application - to later track time later you will need to start it again. If you just want to hide the window, then cancel here and choose \"Close\" instead.", "HideExitWarning"))
                {
                    if (_taskInProgress)
                    {
                        if (!DismissableWarning("Exiting - Task In Progress", "You are exiting Nano TimeTracker, but you still have a task in progress. The next time you start the application, you will be asked to confirm when that task completed.", "HideExitTaskInProgressWarning"))
                        {
                            //user cancelled on the exit task in progress warning
                            e.Cancel = true;
                        }
                    }
                }
                else
                {
                    //user cancelled on the exit warning
                    e.Cancel = true;
                }
            }

            //assuming we didn't actually exit, reset exiting flag for next time
            if (e.Cancel)
                _exiting = false;
            else
                TaskHotKey.Dispose();
        }

        private void btn_Stop_Click(object sender, System.EventArgs e)
        {
            PromptTask();
        }

        private void btn_Start_Click(object sender, System.EventArgs e)
        {
            PromptTask();
        }

        void TaskHotKey_HotkeyPressed(object sender, EventArgs e)
        {
            PromptTask();
        }
        
        private void timer_StatusUpdate_Tick(object sender, System.EventArgs e)
        {
            UpdateStatusDisplay();
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
            PromptTask();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogWindowCloseWithWarning();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _exiting = true;
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
                    if (_taskInProgress)
                    {
                        PromptTask();
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
            messageText = "Current LogFile Path: \u000D\u000A" + Properties.Settings.Default.LogFilePath;
            messageText = messageText + "\u000D\u000A\u000D\u000A" + "To change these options, please use regedit (or something like that).";

            MessageBox.Show(messageText, "LogFile Path", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void dataGridView_TaskLogList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) //to avoid strange calls during initialize
            {
                if (dataGridView_TaskLogList.Columns[e.ColumnIndex].Name == "StartDateTime"
                    || dataGridView_TaskLogList.Columns[e.ColumnIndex].Name == "EndDateTime"
                    || dataGridView_TaskLogList.Columns[e.ColumnIndex].Name == "TimeTaken")
                {
                    //looks like we should consider it, call the function that does the work.
                    MaintainDurationByDataGrid(e.RowIndex);
                    CheckHourTotals();
                    UpdateStatusDisplay();
                }

                if (false)
                {
                    //TODO: Add automatic updating of "current" state if the row being edited was the current row.
                }

                if (false)
                {
                    //TODO: throw validation errors if you're trying to do somethng illegal, like delete end date?
                }

                //persist changes
                SaveTimeTrackingDB();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialogs.AboutBox box = new Dialogs.AboutBox();
            box.ShowDialog();
            box.Dispose();
        }

        private void onlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.architectshack.com/NanoTimeTracker.ashx");
        }

        private void openLogWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleLogWindowDisplay();
        }

        private void startTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PromptTask();
        }

        private void stopEditTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PromptTask();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _exiting = true;
            this.Close();
        }

        private void dataGridView_TaskLogList_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView_TaskLogList.SelectedRows.Count == 1 && !dataGridView_TaskLogList.SelectedRows[0].IsNewRow)
                PromptTask((DateTime)dataGridView_TaskLogList.SelectedRows[0].Cells[0].Value);
            else if (dataGridView_TaskLogList.SelectedCells.Count == 1 && !dataGridView_TaskLogList.SelectedCells[0].OwningRow.IsNewRow && !dataGridView_TaskLogList.SelectedCells[0].IsInEditMode)
                PromptTask((DateTime)dataGridView_TaskLogList.SelectedCells[0].OwningRow.Cells[0].Value);
        }

        #endregion


        #region Local Methods

        private void LogWindowCloseWithWarning()
        {
            if (DismissableWarning("Closing Window", "The Nano TimeTracker log window is closing, but the program will continue to run - you can start and stop tasks, or open the main window, by clicking on the icon in the tray on the bottom right.", "HideCloseWarning"))
                this.Hide();
        }

        private bool DismissableWarning(string warningTitle, string warningMessage, string settingKey)
        {
            bool hideMessage = (bool)Properties.Settings.Default.PropertyValues[settingKey].PropertyValue;
            if (!hideMessage)
            {
                bool permanentlyDismissed;
                if (Dialogs.DismissableConfirmationWindow.ShowMessage(warningTitle, warningMessage, out permanentlyDismissed) == DialogResult.OK)
                {
                    if (permanentlyDismissed)
                    {
                        Properties.Settings.Default.PropertyValues[settingKey].PropertyValue = true;
                        Properties.Settings.Default.Save();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //if the user has decided to hide the message, we assume consent.
                return true;
            }
        }

        private void UpdateControlDisplayConsistency()
        {
            //"Open Log Window" context strip option display
            if (this.Visible && openLogWindowToolStripMenuItem.Enabled)
                openLogWindowToolStripMenuItem.Enabled = false;

            if (!this.Visible && !_displayingDialog && !openLogWindowToolStripMenuItem.Enabled)
                openLogWindowToolStripMenuItem.Enabled = true;

            if (_displayingDialog && openLogWindowToolStripMenuItem.Enabled)
                openLogWindowToolStripMenuItem.Enabled = false;

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
            //WindowHacker.DisableCloseMenu(this);
        }

        private void ToggleLogWindowDisplay()
        {
            timer_NotifySingleClick.Stop();
            if (!this.Visible)
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
                    if (this.WindowState == FormWindowState.Minimized)
                        this.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                this.Hide();
            }
        }

        private void PromptTask()
        {
            PromptTask(null);
        }

        private void PromptTask(DateTime? existingTaskTime)
        {
            //double-check whether we really should be displaying a dialog at all
            if (!_displayingDialog)
            {
                _displayingDialog = true;
                UpdateControlDisplayConsistency();

                //save and switch day if appropriate, before starting any new logging
                if (!_taskInProgress && existingTaskTime == null)
                    SaveTimeTrackingDB(true);

                bool? doAction = null;
                //only relevant if we're not minimized, but seems to do no harm.
                this.Activate();

                DateTime promptStartDate;
                DateTime? promptEndDate;
                string promptDescription;
                string promptCategory;
                bool promptTimeBillable;

                if (existingTaskTime != null)
                {
                    //we are updating a SPECIFIC task
                    promptStartDate = existingTaskTime.Value;
                    if (!GetTaskDetailsByTask(existingTaskTime.Value, out promptEndDate, out promptDescription, out promptCategory, out promptTimeBillable))
                    {
                        MessageBox.Show("Failed to retrieve task details!");
                        doAction = false;
                    }
                }
                else
                {
                    //we are updating the CURRENT task (or starting a new one)
                    promptDescription = _taskInProgressDescription;
                    promptCategory = _taskInProgressCategory;
                    promptTimeBillable = _taskInProgressTimeBillable;

                    if (!_taskInProgress)
                    {
                        promptStartDate = DateTime.Now;
                        promptEndDate = null;
                    }
                    else
                    {
                        promptStartDate = _taskInProgressStartTime;
                        promptEndDate = DateTime.Now;
                    }
                }

                DateTime providedStartDate = DateTime.Now;
                DateTime? providedEndDate = null;
                string providedDescription = "";
                string providedCategory = "";
                bool providedTimeBillable = false;

                if (doAction == null)
                {
                    TaskDialog.SetPrompt(promptStartDate, promptEndDate, promptDescription, promptCategory, promptTimeBillable);
                    if (TaskDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        providedStartDate = TaskDialog.TaskStartDate.Value; //TODO: add validation to ensure this never fails
                        providedEndDate = TaskDialog.TaskEndDate;
                        providedDescription = TaskDialog.TaskDescription;
                        providedCategory = TaskDialog.TaskCategory;
                        providedTimeBillable = TaskDialog.TaskBillable;
                        doAction = true;
                    }
                    else
                        doAction = false;
                }

                if (doAction.Value)
                {
                    if (existingTaskTime != null)
                    {
                        if (_taskInProgressStartTime.Equals(promptStartDate))
                        {
                            //we're updating the in-progress task;
                            _taskInProgressStartTime = providedStartDate;
                            _taskInProgressDescription = providedDescription;
                            _taskInProgressCategory = providedCategory;
                            _taskInProgressTimeBillable = providedTimeBillable;

                            if (_taskInProgress && providedEndDate != null)
                                _taskInProgress = false;
                        }

                        UpdateLogTask(existingTaskTime.Value, providedStartDate, providedEndDate, providedDescription, providedCategory, providedTimeBillable);
                    }
                    else
                    {
                        _taskInProgressStartTime = providedStartDate;
                        _taskInProgressDescription = providedDescription;
                        _taskInProgressCategory = providedCategory;
                        _taskInProgressTimeBillable = providedTimeBillable;

                        //update log if already running but we're not completing
                        if (_taskInProgress && providedEndDate == null)
                            UpdateLogOpenTask(_taskInProgressStartTime, providedEndDate, _taskInProgressDescription, _taskInProgressCategory, _taskInProgressTimeBillable);

                        //start log if not already in-progress
                        if (!_taskInProgress)
                        {
                            _taskInProgress = true;
                            StartLoggingTask(_taskInProgressStartTime, _taskInProgressDescription, _taskInProgressCategory, _taskInProgressTimeBillable);
                        }

                        //end log if end date provided
                        if (providedEndDate != null)
                        {
                            _taskInProgress = false;
                            EndLoggingOpenTask(_taskInProgressStartTime, providedEndDate.Value, _taskInProgressDescription, _taskInProgressCategory, _taskInProgressTimeBillable);
                        }
                    }

                    //retrieve existing totals for display
                    CheckHourTotals();

                    //UI updates specific to task running change
                    UpdateControlDisplayConsistency();
                    if (_taskInProgress)
                    {
                        timer_StatusUpdate.Start();
                        btn_Stop.Focus();
                    }
                    else
                    {
                        timer_StatusUpdate.Stop();
						UpdateStatusDisplay();
                        btn_Start.Focus();
                    }
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

            if (_taskInProgress)
                TimeSinceTaskStart = System.DateTime.Now.Subtract(_taskInProgressStartTime);
            else
                TimeSinceTaskStart = new TimeSpan();
            TotalTimeToday = TimeSinceTaskStart + new TimeSpan((int)Math.Floor(_previousHours), (int)Math.Floor((_previousHours * 60) % 60), (int)Math.Floor((_previousHours * 60 * 60) % 60));
            TotalBillableTimeToday = TimeSinceTaskStart + new TimeSpan((int)Math.Floor(_previousBillableHours), (int)Math.Floor((_previousBillableHours * 60) % 60), (int)Math.Floor((_previousBillableHours * 60 * 60) % 60));

            lbl_WorkingTimeValue.Text = Utils.FormatTimeSpan(TimeSinceTaskStart);
            lbl_TimeTodayValue.Text = Utils.FormatTimeSpan(TotalTimeToday);
            lbl_BillableTimeTodayValue.Text = Utils.FormatTimeSpan(TotalBillableTimeToday);

            if (_taskInProgress)
                notifyIcon1.Text = "Time Logger - " + Utils.FormatTimeSpan(TimeSinceTaskStart);
            else
                notifyIcon1.Text = "Time Logger";
        }

        #endregion

        #region Data-Management Methods - TODO: send to dedicated class.

        private void InitializeData()
        {
            //TODO: add fileDate Switch... or should it be filename switch simply?
            _currentLogFileName = DeriveLogFileName();
            if (System.IO.File.Exists(_currentLogFileName + ".xml"))
                dataSet1.ReadXml(_currentLogFileName + ".xml");

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
                        _taskInProgressDescription = (string)lastRow["TaskName"];
                    _taskInProgressTimeBillable = (bool)lastRow["BillableFlag"];

                    //display timer...
                    lbl_WorkingTimeValue.Text = "Recovering to Running...";
                    timer_StatusUpdate.Start();
                }
            }

            CheckHourTotals();
        }

        private void StartLoggingTask(DateTime taskStartTime, string taskDescription, string taskCategory, bool taskTimeBillable)
        {
            //CSV File
            LogText(
                _currentLogFileName,
                String.Format("{0:yyyy-MM-dd HH:mm:ss}\t",
                    taskStartTime
                    )
                );

            //DB
            DataRow newRow = dataSet1.DataTable1.NewRow();
            newRow["StartDateTime"] = taskStartTime;
            newRow["TaskCategory"] = taskCategory;
            newRow["TaskName"] = taskDescription;
            newRow["BillableFlag"] = taskTimeBillable;
            dataSet1.DataTable1.Rows.Add(newRow);
            SaveTimeTrackingDB();

        }

        private void EndLoggingOpenTask(DateTime taskStartTime, DateTime taskEndDate, string taskDescription, string taskCategory, bool taskTimeBillable)
        {
            //CSV File
            LogText(
                _currentLogFileName,
                String.Format("{0:yyyy-MM-dd HH:mm:ss}\t{1:0.00}\t{2}\u000D\u000A",
                    taskEndDate,
                    taskEndDate.Subtract(_taskInProgressStartTime).TotalHours,
                    taskDescription
                    )
                );

            //DB
            UpdateLogOpenTask(taskStartTime, taskEndDate, taskDescription, taskCategory, taskTimeBillable);

        }

        private void UpdateLogOpenTask(DateTime taskStartTime, DateTime? taskEndDate, string taskDescription, string taskCategory, bool taskTimeBillable)
        {
            DataRow rowToUpdate = FindExistingInProgressTask(taskStartTime);
            if (rowToUpdate != null)
                UpdateTask(rowToUpdate, taskStartTime, taskEndDate, taskDescription, taskCategory, taskTimeBillable);
            else
                MessageBox.Show("Could not find open log entry to update! Task Lost.", "Missing log entry", MessageBoxButtons.OK);
        }

        private void UpdateLogTask(DateTime taskStartTime, DateTime taskNewStartTime, DateTime? taskNewEndDate, string taskNewDescription, string taskNewCategory, bool taskNewTimeBillable)
        {
            DataRow rowToUpdate = GetTaskRow(taskStartTime);
            if (rowToUpdate != null)
                UpdateTask(rowToUpdate, taskNewStartTime, taskNewEndDate, taskNewDescription, taskNewCategory, taskNewTimeBillable);
            else
                MessageBox.Show("Could not find requested log entry! Changes Lost.", "Missing log entry", MessageBoxButtons.OK);
        }

        private DateTime UpdateTask(DataRow rowToUpdate, DateTime taskStartTime, DateTime? taskEndDate, string taskDescription, string taskCategory, bool taskTimeBillable)
        {
            rowToUpdate["StartDateTime"] = taskStartTime;
            rowToUpdate["TaskCategory"] = taskCategory;
            rowToUpdate["TaskName"] = taskDescription;
            rowToUpdate["BillableFlag"] = taskTimeBillable;
            if (taskEndDate != null)
            {
                rowToUpdate["EndDateTime"] = taskEndDate.Value;
                rowToUpdate["TimeTaken"] = taskEndDate.Value.Subtract(taskStartTime).TotalHours;
                //save and switch day if appropriate
                SaveTimeTrackingDB(true);
            }
            else
            {
                rowToUpdate["EndDateTime"] = DBNull.Value;
                rowToUpdate["TimeTaken"] = DBNull.Value;
                //save WITHOUT switching day
                SaveTimeTrackingDB();
            }
            return taskStartTime;
        }

        private DataRow FindExistingInProgressTask(DateTime taskStartTime)
        {
            DataRow targetRow = null;
            DataRow[] candidateRows = dataSet1.DataTable1.Select("StartDateTime = #" + taskStartTime.ToString() + "# And EndDateTime Is Null");
            if (candidateRows.Length == 0)
                candidateRows = dataSet1.DataTable1.Select("StartDateTime Is Not Null And EndDateTime Is Null", "StartDateTime Desc");
            if (candidateRows.Length > 0)
            {
                DateTime matchStartTime = (DateTime)candidateRows[0]["StartDateTime"];
                if (candidateRows.Length > 1)
                    MessageBox.Show(string.Format("More than one unfinished task found. Using more recent one, with original start date {0:yyyy-MM-dd HH:mm:ss}.", matchStartTime), "Multiple log entries", MessageBoxButtons.OK);
                targetRow = candidateRows[0];
            }
            return targetRow;
        }

        private DataRow GetTaskRow(DateTime existingTaskTime)
        {
            DataRow[] candidateRows = dataSet1.DataTable1.Select("StartDateTime = #" + existingTaskTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff") + "#");
            if (candidateRows.Length == 1)
                return candidateRows[0];
            else
                return null;
        }

        private bool GetTaskDetailsByTask(DateTime existingTaskTime, out DateTime? taskEndDate, out string taskDescription, out string taskCategory, out bool taskTimeBillable)
        {
            DataRow[] candidateRows = dataSet1.DataTable1.Select("StartDateTime = #" + existingTaskTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff") + "#");
            if (candidateRows.Length == 1)
            {
                if (DBNull.Value.Equals(candidateRows[0]["EndDateTime"]))
                    taskEndDate = null;
                else
                    taskEndDate = (DateTime?)candidateRows[0]["EndDateTime"];

                taskCategory = (string)candidateRows[0]["TaskCategory"];
                taskDescription = (string)candidateRows[0]["TaskName"];
                taskTimeBillable = (bool)candidateRows[0]["BillableFlag"];
                return true;
            }
            else
            {
                taskEndDate = null;
                taskCategory = "";
                taskDescription = "";
                taskTimeBillable = false;
                return false;
            }
        }

        private string _currentLogFileName;
        private string DeriveLogFileName()
        {
            return Utils.ExpandPath(Properties.Settings.Default.LogFilePath, DateTime.Now);
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
            //If we have a valid date set, log. Otherwise we must be starting up...
            if (!string.IsNullOrEmpty(_currentLogFileName))
            {
                //Write the dataset
                dataSet1.WriteXml(_currentLogFileName + ".xml");

                //if the day has ended, clear/start anew
                if (!_currentLogFileName.Equals(DeriveLogFileName()) && AllowDateSwitch)
                {
                    dataSet1.DataTable1.Rows.Clear();
                    _currentLogFileName = DeriveLogFileName();
                    dataSet1.WriteXml(_currentLogFileName + ".xml");
                }
            }
        }

        private void CheckHourTotals()
        {
            //retrieve existing for total display
            object previousHoursResult = dataSet1.DataTable1.Compute("Sum(TimeTaken)", null);
            if (previousHoursResult != DBNull.Value)
                _previousHours = (double)previousHoursResult;
            else
                _previousHours = 0;

            object previousHoursBillableResult = dataSet1.DataTable1.Compute("Sum(TimeTaken)", "BillableFlag = True");
            if (previousHoursBillableResult != DBNull.Value)
                _previousBillableHours = (double)previousHoursBillableResult;
            else
                _previousBillableHours = 0;
        }

        private void DeleteLogs()
        {
            //Delete the file
            if (System.IO.File.Exists(_currentLogFileName))
            {
                System.IO.File.Delete(_currentLogFileName);
            }
            else
            {
                MessageBox.Show(this, "No text file to delete.", "Log Deletion", MessageBoxButtons.OK);
            }

            //Delete the records (and save)
            if (dataSet1.DataTable1.Rows.Count > 0)
            {
                dataSet1.DataTable1.Rows.Clear();
                SaveTimeTrackingDB(true);
            }
            else
            {
                MessageBox.Show(this, "No datagrid entries to delete.", "Log Deletion", MessageBoxButtons.OK);
            }
        }

        #endregion

    }
}
