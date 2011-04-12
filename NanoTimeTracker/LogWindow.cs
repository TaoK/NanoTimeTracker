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
            //DataView test1 = new DataView(dataSet1.DataTable1);
            //dataSet1BindingSource.DataSource = dataSet1;
            TaskDialog = new Dialogs.TaskInput();
        }

        //State
        private bool _displayingDialog;
        private DateTime? _gridDisplayDate;
        private bool _taskInProgress;
        private DateTime _taskInProgressStartTime;
        private string _taskInProgressDescription = "";
        private string _taskInProgressCategory = "";
        private bool _taskInProgressTimeBillable = true;
        private double _previousHours;
        private double _previousBillableHours;

        //TEMP
        bool _exiting = false;
        BindingSource dataSet1BindingSource;

        //Resources
        private Icon TaskInProgressIcon;
        private Icon NoTaskActiveIcon;
        private Dialogs.TaskInput TaskDialog;
        private Hotkey TaskHotKey;
        private DatabaseManager _databaseManager;

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

            if (!Properties.Settings.Default.UpgradeCompleted)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeCompleted = true;
                Properties.Settings.Default.Save();
                //TODO: Add data migration too.
                //TODO: ADD Confirmation window asking about deleting previous settings, if there was really an upgrade
            }

            _databaseManager = new DatabaseManager();
            _databaseManager.LoadDatabase();
            dataSet1BindingSource = _databaseManager.GetBindingSource();
            dataGridView_TaskLogList.DataSource = dataSet1BindingSource;

            UpdateControlDisplayConsistency(); //set initial display filter, etc (will be called again in updatestate)
            UpdateStateFromData(true);

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
                if (UIUtils.DismissableWarning("Exiting Application", "You are exiting the Nano TimeTracker application - to later track time later you will need to start it again. If you just want to hide the window, then cancel here and choose \"Close\" instead.", "HideExitWarning"))
                {
                    if (_taskInProgress)
                    {
                        if (!UIUtils.DismissableWarning("Exiting - Task In Progress", "You are exiting Nano TimeTracker, but you still have a task in progress. The next time you start the application, you will be asked to confirm when that task completed.", "HideExitTaskInProgressWarning"))
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
                    _databaseManager.DeleteLogs();
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
                    || dataGridView_TaskLogList.Columns[e.ColumnIndex].Name == "TimeTaken"
                    || dataGridView_TaskLogList.Columns[e.ColumnIndex].Name == "BillableFlag"
                    )
                {
                    //looks like we should consider it, call the function that does the work.
                    MaintainDurationByDataGrid(e.RowIndex);
                }

                if (false)
                {
                    //TODO: throw validation errors if you're trying to do somethng illegal, like delete end date?
                }

                //dirty hack to flush value to dataset (alternative method was to set currentcell to a cell in a 
                //different row, but was horrible hack and failed when only one row existed!). Found this cleaner 
                //suggestion on StackOverflow:
                // http://stackoverflow.com/questions/963601/datagridview-value-does-not-gets-saved-if-selection-is-not-lost-from-a-cell
                dataGridView_TaskLogList.CurrentCell = null;
                dataGridView_TaskLogList.CurrentCell = dataGridView_TaskLogList.Rows[e.RowIndex].Cells[e.ColumnIndex];

                _databaseManager.SaveTimeTrackingDB();
                UpdateStateFromData(false);
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
            //TODO: clean up right double-click handling (right double-click doesn't mean anything normally right?)

            //first select the row whose cell was double-clicked (if single cell)
            if (dataGridView_TaskLogList.SelectedCells.Count == 1 && !dataGridView_TaskLogList.SelectedCells[0].OwningRow.IsNewRow && !dataGridView_TaskLogList.SelectedCells[0].IsInEditMode)
                dataGridView_TaskLogList.Rows[dataGridView_TaskLogList.SelectedCells[0].RowIndex].Selected = true;

            //then display the edit dialog (if a single row is selected)
            if (dataGridView_TaskLogList.SelectedRows.Count == 1 && !dataGridView_TaskLogList.SelectedRows[0].IsNewRow)
                PromptTask((DateTime)dataGridView_TaskLogList.SelectedRows[0].Cells[0].Value);
        }


        private void updateTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView_TaskLogList.SelectedRows.Count == 1)
                PromptTask((DateTime)dataGridView_TaskLogList.SelectedRows[0].Cells[0].Value);
        }

        private void resumeTaskNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView_TaskLogList.SelectedRows.Count == 1)
            {
                DateTime previousStartDate = (DateTime)dataGridView_TaskLogList.SelectedRows[0].Cells[0].Value;
                DateTime? previousEndDate;
                string previousDescription;
                string previousCategory;
                bool previousTimeBillable;
                if (_databaseManager.GetTaskDetailsByTask(previousStartDate, out previousEndDate, out previousDescription, out previousCategory, out previousTimeBillable))
                {
                    _databaseManager.StartLoggingTask(DateTime.Now, previousDescription, previousCategory, previousTimeBillable);
                    UpdateStateFromData(true);
                }
                else
                {
                    MessageBox.Show("Failed to retrieve task details!");
                }
            }
        }

        private void splitTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView_TaskLogList.SelectedRows.Count == 1 && !_displayingDialog)
            {
                _displayingDialog = true;
                UpdateControlDisplayConsistency();

                DateTime previousStartDate = (DateTime)dataGridView_TaskLogList.SelectedRows[0].Cells[0].Value;
                DateTime? previousEndDate;
                string previousDescription;
                string previousCategory;
                bool previousTimeBillable;
                TimeSpan previousDuration;
                if (_databaseManager.GetTaskDetailsByTask(previousStartDate, out previousEndDate, out previousDescription, out previousCategory, out previousTimeBillable))
                {
                    previousDuration = previousStartDate - previousEndDate.Value;
                    TaskDialog.SetPrompt("Please provide details for the new second task:", "Split Task - Provide New Details", (previousStartDate - new TimeSpan(previousDuration.Ticks/2)), previousEndDate, previousDescription, previousCategory, previousTimeBillable, false);
                    if (TaskDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _databaseManager.UpdateLogTask(previousStartDate, previousStartDate, TaskDialog.TaskStartDate.Value, previousDescription, previousCategory, previousTimeBillable);
                        _databaseManager.StartLoggingTask(TaskDialog.TaskStartDate.Value, TaskDialog.TaskDescription, TaskDialog.TaskCategory, TaskDialog.TaskBillable);
                        _databaseManager.EndLoggingOpenTask(TaskDialog.TaskStartDate.Value, TaskDialog.TaskEndDate.Value, TaskDialog.TaskDescription, TaskDialog.TaskCategory, TaskDialog.TaskBillable);
                        UpdateStateFromData(false);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to retrieve task details!");
                }

                _displayingDialog = false;
                UpdateControlDisplayConsistency();
            }
        }

        private void deleteTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView_TaskLogList.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Are you sure you want to delete this task entry?", "Really Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dataGridView_TaskLogList.Rows.Remove(dataGridView_TaskLogList.SelectedRows[0]);
                    //this doesn't fire "UserDeletedRow" event, so call persistence & state updates manually
                    _databaseManager.SaveTimeTrackingDB();
                    UpdateStateFromData(true);
                }
            }
        }

        private void dataGridView_TaskLogList_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this task entry?", "Really Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void dataGridView_TaskLogList_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            _databaseManager.SaveTimeTrackingDB();
            UpdateStateFromData(true);
        }


        #endregion


        #region Local Methods

        private void LogWindowCloseWithWarning()
        {
            if (UIUtils.DismissableWarning("Closing Window", "The Nano TimeTracker log window is closing, but the program will continue to run - you can start and stop tasks, or open the main window, by clicking on the icon in the tray on the bottom right.", "HideCloseWarning"))
                this.Hide();
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

            //systray context strip
            if (_taskInProgress && startTaskToolStripMenuItem.Enabled)
                startTaskToolStripMenuItem.Enabled = false;
            else if (!_taskInProgress && !startTaskToolStripMenuItem.Enabled)
                startTaskToolStripMenuItem.Enabled = true;

            if (!_taskInProgress && stopEditTaskToolStripMenuItem.Enabled)
                stopEditTaskToolStripMenuItem.Enabled = false;
            else if (_taskInProgress && !stopEditTaskToolStripMenuItem.Enabled)
                stopEditTaskToolStripMenuItem.Enabled = true;

            //datagridview context strip
            if (_taskInProgress && resumeTaskNowToolStripMenuItem.Enabled)
                resumeTaskNowToolStripMenuItem.Enabled = false;
            else if (!_taskInProgress && !resumeTaskNowToolStripMenuItem.Enabled)
                resumeTaskNowToolStripMenuItem.Enabled = true;

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

            //timer
            if (_taskInProgress && !timer_StatusUpdate.Enabled)
                timer_StatusUpdate.Start();
            else if (!_taskInProgress && timer_StatusUpdate.Enabled)
                timer_StatusUpdate.Stop();

            //datePicker
            datePicker_FilterDate.MaxDate = DateTime.Today;

            //dataGridView filtering
            DateTime currentFilterDate;
            if (datePicker_FilterDate.Checked && datePicker_FilterDate.Value != null)
                currentFilterDate = datePicker_FilterDate.Value.Date;
            else
                currentFilterDate = DateTime.Today;

            string currentFilterString = string.Format("StartDateTime >= #{0}# And StartDateTime < #{1}#", Utils.FormatDateFullTimeStamp(currentFilterDate), Utils.FormatDateFullTimeStamp(currentFilterDate.AddDays(1)));

            if ((dataSet1BindingSource.Filter == null || !dataSet1BindingSource.Filter.Equals(currentFilterString)) && dataSet1BindingSource.SupportsFiltering)
            {
                dataSet1BindingSource.Filter = currentFilterString;
            }
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
                    _databaseManager.SaveTimeTrackingDB(true);

                bool? doAction = null;
                //only relevant if we're not minimized, but seems to do no harm.
                this.Activate();

                string promptMessage;
                string promptTitle;
                DateTime promptStartDate;
                DateTime? promptEndDate;
                string promptDescription;
                string promptCategory;
                bool promptTimeBillable;

                if (existingTaskTime != null)
                {
                    //we are updating a SPECIFIC task
                    promptMessage = "Please provide updated Task details:";
                    promptTitle = "Update Task";
                    promptStartDate = existingTaskTime.Value;
                    if (!_databaseManager.GetTaskDetailsByTask(existingTaskTime.Value, out promptEndDate, out promptDescription, out promptCategory, out promptTimeBillable))
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
                        promptMessage = "New task details:";
                        promptTitle = "Task Entry - New Task";
                        promptStartDate = DateTime.Now;
                        promptEndDate = null;
                    }
                    else
                    {
                        promptMessage = "Task details:";
                        promptTitle = "Task Entry - Confirm Task Details / End Task";
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
                    TaskDialog.SetPrompt(promptMessage, promptTitle, promptStartDate, promptEndDate, promptDescription, promptCategory, promptTimeBillable, true);
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
                        //update the SPECIFIC entry that was passed in
                        _databaseManager.UpdateLogTask(existingTaskTime.Value, providedStartDate, providedEndDate, providedDescription, providedCategory, providedTimeBillable);
                    }
                    else
                    {
                        //update log if already running but we're not completing
                        if (_taskInProgress && providedEndDate == null)
                            _databaseManager.UpdateLogOpenTask(providedStartDate, providedEndDate, providedDescription, providedCategory, providedTimeBillable);

                        //start log if not already in-progress
                        if (!_taskInProgress)
                        {
                            _databaseManager.StartLoggingTask(providedStartDate, providedDescription, providedCategory, providedTimeBillable);
                        }

                        //end log if end date provided
                        if (providedEndDate != null)
                        {
                            _databaseManager.EndLoggingOpenTask(providedStartDate, providedEndDate.Value, providedDescription, providedCategory, providedTimeBillable);
                        }
                    }

                    UpdateStateFromData(true);
                }

                //release dialog "lock" and update display
                _displayingDialog = false;
                UpdateControlDisplayConsistency();
            }
        }

        private void UpdateStatusDisplay()
        {
            System.TimeSpan timeSinceTaskStart;
            System.TimeSpan totalTimeToday;
            System.TimeSpan totalBillableTimeToday;

            if (_taskInProgress)
            {
                timeSinceTaskStart = System.DateTime.Now.Subtract(_taskInProgressStartTime);
                notifyIcon1.Text = "Time Logger - " + Utils.FormatTimeSpan(timeSinceTaskStart);

                if (!lbl_CurrentTaskValue.Text.Equals(_taskInProgressDescription))
                    lbl_CurrentTaskValue.Text = _taskInProgressDescription;
                if (!lbl_CategoryValue.Text.Equals(_taskInProgressCategory))
                    lbl_CategoryValue.Text = _taskInProgressCategory;
            }
            else
            {
                timeSinceTaskStart = new TimeSpan();
                notifyIcon1.Text = "Time Logger";

                if (!lbl_CurrentTaskValue.Text.Equals(""))
                    lbl_CurrentTaskValue.Text = "";
                if (!lbl_CategoryValue.Text.Equals(""))
                    lbl_CategoryValue.Text = "";
            }
            
            totalTimeToday = timeSinceTaskStart + Utils.DecimalHoursToTimeSpan(_previousHours);
            totalBillableTimeToday = (_taskInProgressTimeBillable ? timeSinceTaskStart : new TimeSpan()) + Utils.DecimalHoursToTimeSpan(_previousBillableHours);

            lbl_WorkingTimeValue.Text = Utils.FormatTimeSpan(timeSinceTaskStart);
            lbl_TimeTodayValue.Text = Utils.FormatTimeSpan(totalTimeToday);
            lbl_BillableTimeTodayValue.Text = Utils.FormatTimeSpan(totalBillableTimeToday);
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

        private void UpdateStateFromData(bool allowReFocus)
        {
            DateTime existingTaskStartTime;
            string existingTaskDescription;
            string existingTaskCategory;
            bool existingTaskBillable;

            if (_databaseManager.GetInProgressTaskDetails(out existingTaskStartTime, out existingTaskDescription, out existingTaskCategory, out existingTaskBillable))
            {
                _taskInProgress = true;
                _taskInProgressStartTime = existingTaskStartTime;
                _taskInProgressDescription = existingTaskDescription;
                _taskInProgressCategory = existingTaskCategory;
                _taskInProgressTimeBillable = existingTaskBillable;
            }
            else
            {
                _taskInProgress = false;
            }

            //retrieve existing totals for display
            _previousHours = _databaseManager.GetPreviousHoursTotalsToday();
            _previousBillableHours = _databaseManager.GetPreviousBillableHoursTotalsToday();

            //UI updates specific to task running change
            UpdateControlDisplayConsistency();
            if (allowReFocus)
            {
                if (_taskInProgress)
                {
                    btn_Stop.Focus();
                    datePicker_FilterDate.Value = DateTime.Today;
                    dataGridView_TaskLogList.CurrentCell = dataGridView_TaskLogList.Rows[dataGridView_TaskLogList.Rows.Count - 1].Cells[0];
                }
                else
                    btn_Start.Focus();
            }
            UpdateStatusDisplay();
        }

        #endregion


        //This is going to become a user control...
        private void datePicker_FilterDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateControlDisplayConsistency();
            if (datePicker_FilterDate.MaxDate.Equals(datePicker_FilterDate.Value) && btn_DateNext.Enabled)
                btn_DateNext.Enabled = false;
            else if (!datePicker_FilterDate.MaxDate.Equals(datePicker_FilterDate.Value) && !btn_DateNext.Enabled)
                btn_DateNext.Enabled = true;
        }

        private void btn_DatePrev_Click(object sender, EventArgs e)
        {
            datePicker_FilterDate.Value = datePicker_FilterDate.Value.AddDays(-1);
        }

        private void btn_DateNext_Click(object sender, EventArgs e)
        {
            datePicker_FilterDate.Value = datePicker_FilterDate.Value.AddDays(1);
        }

    }
}
