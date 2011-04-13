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
using System.Data;
using System.Windows.Forms;

namespace NanoTimeTracker
{
    class DatabaseManager
    {
        private DataSet dataSet;
        private DataTable logTable;

        public DatabaseManager()
        {
            dataSet = new DataSet("DataSet1");
            logTable = dataSet.Tables.Add("DataTable1");
            logTable.Columns.Add("StartDateTime");
            logTable.Columns["StartDateTime"].DataType = typeof(DateTime);
            logTable.Columns["StartDateTime"].AllowDBNull = false;

            logTable.Columns.Add("EndDateTime");
            logTable.Columns["EndDateTime"].DataType = typeof(DateTime);
            logTable.Columns["EndDateTime"].AllowDBNull = true;

            logTable.Columns.Add("TaskCategory");
            logTable.Columns["TaskCategory"].DataType = typeof(string);
            logTable.Columns["TaskCategory"].AllowDBNull = false;

            logTable.Columns.Add("TaskName");
            logTable.Columns["TaskName"].DataType = typeof(string);
            logTable.Columns["TaskName"].AllowDBNull = false;

            logTable.Columns.Add("BillableFlag");
            logTable.Columns["BillableFlag"].DataType = typeof(bool);
            logTable.Columns["BillableFlag"].AllowDBNull = false;

            logTable.Columns.Add("TimeTaken");
            logTable.Columns["TimeTaken"].DataType = typeof(double);
            logTable.Columns["TimeTaken"].AllowDBNull = true;

            logTable.Constraints.Add("PK_StartDateTime", logTable.Columns["StartDateTime"], true);
        }

        public BindingSource GetBindingSource()
        {
            BindingSource newBindingSource = new BindingSource();
            newBindingSource.DataSource = dataSet;
            newBindingSource.DataMember = "DataTable1";
            newBindingSource.Sort = "StartDateTime ASC";
            return newBindingSource;
        }

        public void LoadDatabase()
        {
            if (System.IO.File.Exists(DeriveLogFileName()))
                dataSet.ReadXml(DeriveLogFileName());
        }



        public bool GetInProgressTaskDetails(out DateTime taskStartTime, out string taskDescription, out string taskCategory, out bool taskBillable)
        {
            DataRow[] recentOpenTasks = logTable.Select("StartDateTime Is Not Null And EndDateTime Is Null", "StartDateTime Desc");
            if (recentOpenTasks.Length > 0)
            {
                DataRow mostRecentOpenTask = recentOpenTasks[0];
                taskStartTime = (DateTime)mostRecentOpenTask["StartDateTime"];
                taskDescription = (string)mostRecentOpenTask["TaskName"];
                taskCategory = (string)mostRecentOpenTask["TaskCategory"];
                taskBillable = (bool)mostRecentOpenTask["BillableFlag"];
                return true;
            }
            else
            {
                taskStartTime = DateTime.Now;
                taskDescription = "";
                taskCategory = "";
                taskBillable = false;
                return false;
            }
        }

        public void StartLoggingTask(DateTime taskStartTime, string taskDescription, string taskCategory, bool taskTimeBillable)
        {
            DataRow newRow = logTable.NewRow();
            newRow["StartDateTime"] = taskStartTime;
            newRow["TaskCategory"] = taskCategory;
            newRow["TaskName"] = taskDescription;
            newRow["BillableFlag"] = taskTimeBillable;
            logTable.Rows.Add(newRow);
            SaveTimeTrackingDB();

        }

        public void UpdateLogOpenTask(DateTime taskStartTime, DateTime? taskEndDate, string taskDescription, string taskCategory, bool taskTimeBillable)
        {
            DataRow rowToUpdate = FindExistingInProgressTask(taskStartTime);
            if (rowToUpdate != null)
                UpdateTask(rowToUpdate, taskStartTime, taskEndDate, taskDescription, taskCategory, taskTimeBillable);
            else
                MessageBox.Show("Could not find open log entry to update! Task Lost.", "Missing log entry", MessageBoxButtons.OK);
        }

        public void UpdateLogTask(DateTime taskStartTime, DateTime taskNewStartTime, DateTime? taskNewEndDate, string taskNewDescription, string taskNewCategory, bool taskNewTimeBillable)
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
            DataRow[] candidateRows = logTable.Select("StartDateTime = #" + Utils.FormatDateFullTimeStamp(taskStartTime) + "# And EndDateTime Is Null");
            if (candidateRows.Length == 0)
                candidateRows = logTable.Select("StartDateTime Is Not Null And EndDateTime Is Null", "StartDateTime Desc");
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
            DataRow[] candidateRows = logTable.Select("StartDateTime = #" + Utils.FormatDateFullTimeStamp(existingTaskTime) + "#");
            if (candidateRows.Length == 1)
                return candidateRows[0];
            else
                return null;
        }

        public bool GetTaskDetailsByTask(DateTime existingTaskTime, out DateTime? taskEndDate, out string taskDescription, out string taskCategory, out bool taskTimeBillable)
        {
            DataRow[] candidateRows = logTable.Select("StartDateTime = #" + Utils.FormatDateFullTimeStamp(existingTaskTime) + "#");
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

        private string DeriveLogFileName()
        {
            return FrameworkClassReplacements.LocalFileSettingsProviderConfigurable.GetDataFilePath("Database.xml");
        }

        public void SaveTimeTrackingDB()
        {
            SaveTimeTrackingDB(false);
        }

        public void SaveTimeTrackingDB(bool AllowDateSwitch)
        {
            Utils.SaveDataSetSafe(dataSet, DeriveLogFileName());
            //TODO: Add auto-exporting here.
        }

        public double GetHoursTotals(DateTime fromDate, DateTime toDate, bool billableOnly)
        {
            string filterString = string.Format("StartDateTime >= #{0}# And StartDateTime < #{1}#", Utils.FormatDateFullTimeStamp(fromDate.Date), Utils.FormatDateFullTimeStamp(toDate.Date.AddDays(1)));
            if (billableOnly) filterString += " And BillableFlag = True";
            return GetComputedDouble("Sum(TimeTaken)", filterString);
        }

        private double GetComputedDouble(string expression, string filter)
        {
            object computedResult = logTable.Compute(expression, filter);
            if (computedResult != DBNull.Value)
                return (double)computedResult;
            else
                return 0;
        }

        public void DeleteLogs()
        {
            //Delete the records (and save)
            if (logTable.Rows.Count > 0)
            {
                logTable.Rows.Clear();
                SaveTimeTrackingDB(true);
            }
            else
            {
                MessageBox.Show("No data to delete.", "Log Deletion", MessageBoxButtons.OK);
            }
        }

    }
}
