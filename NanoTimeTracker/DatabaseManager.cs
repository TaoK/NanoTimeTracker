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
using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;

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

        private AutoCompletionCache _autoCompletionCache = new AutoCompletionCache();
        internal AutoCompletionCache AutoCompletionCache { get { return _autoCompletionCache; } }

        public void ReadAutoCompletionDataFromDB()
        {
            DataRow[] recentRows = logTable.Select("StartDateTime > #" + Utils.FormatDateFullTimeStamp(DateTime.Now.AddDays(-30)) + "#");
            foreach (DataRow recentRow in recentRows)
                _autoCompletionCache.Feed((string)recentRow["TaskName"].ToString(), (string)recentRow["TaskCategory"], (bool)recentRow["BillableFlag"]);
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
            _autoCompletionCache.Feed(taskDescription, taskCategory, taskTimeBillable);
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
            _autoCompletionCache.Feed(taskDescription, taskCategory, taskTimeBillable);
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

        internal void Export(string destinationFileName, DateTime fromDate, DateTime toDate, bool billableEntriesOnly)
        {
            string filterString = string.Format("StartDateTime >= #{0}# And StartDateTime < #{1}#", Utils.FormatDateFullTimeStamp(fromDate), Utils.FormatDateFullTimeStamp(toDate.AddDays(1)));
            if (billableEntriesOnly) filterString += " And BillableFlag = True";
            Utils.ExportFilteredDataTableToCsv(logTable, destinationFileName, filterString);
        }

        internal void Import(string sourceFileName)
        {
            /*
             * TODO: Add characterset auto-detection
             * AND TEST FOR "Bush hid the facts" BUG IN AUTO-DETECTION ROUTINE!
             * AND ADD REFERENCE TO THIS IN AUTO-DETECTION ROUTINE DESCRIPTION
             */
            using (System.IO.StreamReader textReader = System.IO.File.OpenText(sourceFileName))
            using (CsvReader importReader = new CsvReader(textReader, true, ',', '"', '"', '#', ValueTrimmingOptions.All, 0x1000))
            {
                //other defaults for our accepted format:
                importReader.SupportsMultiline = true;
                importReader.SkipEmptyLines = true;
                importReader.DefaultParseErrorAction = ParseErrorAction.ThrowException;
                importReader.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;

                string[] headers = importReader.GetFieldHeaders();

                Dictionary<string, int> importColumnIDs = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

                bool abort = false;

                for (int i = 0; i < headers.Length && !abort; i++)
                {
                    if (importColumnIDs.ContainsKey(headers[i]))
                    {
                        if (MessageBox.Show(string.Format("Duplicate column detected, name \"{0}\"! Ignore new column?", headers[i]), "Duplicate Column Name", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            abort = true;
                    }
                    else
                    {
                        if (headers[i].Equals("StartDateTime", StringComparison.InvariantCultureIgnoreCase)
                            || headers[i].Equals("EndDateTime", StringComparison.InvariantCultureIgnoreCase)
                            || headers[i].Equals("TaskCategory", StringComparison.InvariantCultureIgnoreCase)
                            || headers[i].Equals("TaskName", StringComparison.InvariantCultureIgnoreCase)
                            || headers[i].Equals("BillableFlag", StringComparison.InvariantCultureIgnoreCase)
                            || headers[i].Equals("TimeTaken", StringComparison.InvariantCultureIgnoreCase)
                            )
                            importColumnIDs.Add(headers[i], i);
                        else if (MessageBox.Show(string.Format("Unsupported column detected, name \"{0}\"! Ignore new column?", headers[i]), "Unknown Column Name", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            abort = true;
                    }
                }

                if (!abort && !importColumnIDs.ContainsKey("StartDateTime"))
                {
                    MessageBox.Show("Required column \"StartDateTime\" not found in import file. Aborting.", "Missing column", MessageBoxButtons.OK);
                    abort = true;
                }

                if (!abort && !importColumnIDs.ContainsKey("TaskName"))
                {
                    MessageBox.Show("Required column \"TaskName\" not found in import file. Aborting.", "Missing column", MessageBoxButtons.OK);
                    abort = true;
                }

                if (!abort
                    && !importColumnIDs.ContainsKey("EndDateTime")
                    && !importColumnIDs.ContainsKey("TimeTaken")
                    )
                {
                    MessageBox.Show("Neither \"EndDateTime\" nor \"TimeTaken\" was found in import file; one or the other must be provided. Aborting.", "Missing column", MessageBoxButtons.OK);
                    abort = true;
                }

                bool alwaysOverwriteOnMismatch = false;

                //main record import loop... 
                while (!abort && importReader.ReadNextRecord())
                {
                    DateTime startTime;
                    DateTime endTime = DateTime.MinValue;
                    string taskCategory = null;
                    string taskName;
                    bool? taskBillable = null;
                    double? providedTimeTaken = null;
                    double? calculatedTimeTaken = null;

                    long currentRowNumber = importReader.CurrentRecordIndex + 1;

                    if (!DateTime.TryParse(importReader[importColumnIDs["StartDateTime"]], out startTime))
                    {
                        MessageBox.Show(string.Format("Invalid \"StartDateTime\" value found, record number {0} in import file. Aborting.", currentRowNumber), "Invalid Value Found", MessageBoxButtons.OK);
                        abort = true;
                    }

                    if (!abort && importColumnIDs.ContainsKey("EndDateTime"))
                    {
                        if (DateTime.TryParse(importReader[importColumnIDs["EndDateTime"]], out endTime))
                        {
                            calculatedTimeTaken = (endTime - startTime).TotalHours;
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Invalid \"EndDateTime\" value found, record number {0} in import file. Aborting.", currentRowNumber), "Invalid Value Found", MessageBoxButtons.OK);
                            abort = true;
                        }
                    }

                    if (importColumnIDs.ContainsKey("TaskCategory"))
                        taskCategory = importReader[importColumnIDs["TaskCategory"]];

                    taskName = importReader[importColumnIDs["TaskName"]];

                    bool parsedBillable;
                    if (bool.TryParse(importReader[importColumnIDs["BillableFlag"]], out parsedBillable))
                    {
                        taskBillable = parsedBillable;
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Invalid \"BillableFlag\" value found, record number {0} in import file. Aborting.", currentRowNumber), "Invalid Value Found", MessageBoxButtons.OK);
                        abort = true;
                    }

                    if (!abort && importColumnIDs.ContainsKey("TimeTaken"))
                    {
                        double parsedTimeTaken = 0;
                        if (double.TryParse(importReader[importColumnIDs["TimeTaken"]], out parsedTimeTaken))
                        {
                            providedTimeTaken = parsedTimeTaken;
                            endTime = startTime.AddHours(providedTimeTaken.Value);
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Invalid \"TimeTaken\" value found, record number {0} in import file. Aborting.", currentRowNumber), "Invalid Value Found", MessageBoxButtons.OK);
                            abort = true;
                        }
                    }

                    if (calculatedTimeTaken != null 
                        && providedTimeTaken != null
                        && Math.Abs(calculatedTimeTaken.Value - providedTimeTaken.Value) > 0.01
                        )
                    {
                        MessageBox.Show(string.Format("Inconsistent \"EndDateTime\" and \"TimeTaken\" values found, record number {0} in import file. Aborting.", currentRowNumber), "Inconsistent Values Found", MessageBoxButtons.OK);
                        abort = true;
                    }

                    if (!abort)
                    {
                        DataRow[] possibleMatch = logTable.Select("StartDateTime = #" + Utils.FormatDateFullTimeStamp(startTime) + "#");
                        if (possibleMatch.Length > 0)
                        {
                            if (possibleMatch[0]["EndDateTime"].ToString() == endTime.ToString()
                                && (taskCategory == null || possibleMatch[0]["TaskCategory"].ToString() == taskCategory)
                                && possibleMatch[0]["TaskName"].ToString() == taskName
                                && (taskBillable == null || possibleMatch[0]["BillableFlag"].ToString() == taskBillable.Value.ToString())
                                )
                            {
                                //Nothing to do, the import data exactly matches the existing data.
                            }
                            else
                            {
                                bool overWriteRecord = false;

                                if (alwaysOverwriteOnMismatch)
                                    overWriteRecord = true;
                                else
                                {
                                    if (Dialogs.DismissableConfirmationWindow.ShowMessage("Overwrite Entry?", string.Format("Existing entry found matching import data for row {0}; click OK to overwrite the existing entry, or Cancel to abort the import.", currentRowNumber), out alwaysOverwriteOnMismatch) == DialogResult.OK)
                                        overWriteRecord = true;
                                    else
                                        abort = true;
                                }

                                if (overWriteRecord)
                                {
                                    possibleMatch[0]["EndDateTime"] = endTime;
                                    possibleMatch[0]["TaskCategory"] = taskCategory;
                                    possibleMatch[0]["TaskName"] = taskName;
                                    if (taskBillable != null)
                                        possibleMatch[0]["BillableFlag"] = taskBillable.Value;
                                    possibleMatch[0]["TimeTaken"] = providedTimeTaken ?? calculatedTimeTaken;
                                    possibleMatch[0].AcceptChanges();
                                }
                            }
                        }
                        else
                        {
                            DataRow newRow = logTable.NewRow();
                            newRow["StartDateTime"] = startTime;
                            newRow["EndDateTime"] = endTime;
                            newRow["TaskCategory"] = taskCategory;
                            newRow["TaskName"] = taskName;
                            if (taskBillable == null)
                                newRow["BillableFlag"] = true;
                            else
                                newRow["BillableFlag"] = taskBillable.Value;
                            newRow["TimeTaken"] = providedTimeTaken ?? calculatedTimeTaken;
                            logTable.Rows.Add(newRow);
                        }

                        //TODO: Add consistency checks here like in UI edit.
                    }
                }

                //everything went OK, save updated DB...?
                //TODO: consider reloading instead of saving, if we ended up aborting.
               SaveTimeTrackingDB(false);

                if (!abort)
                    MessageBox.Show("File imported successfully!", "Success", MessageBoxButtons.OK);
            }
        }
    }
}
