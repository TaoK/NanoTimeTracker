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
using System.Drawing;
using System.Windows.Forms;


namespace NanoTimeTracker
{
    partial class LogWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogWindow));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_Start = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.timer_StatusUpdate = new System.Windows.Forms.Timer(this.components);
            this.txt_LogBox = new System.Windows.Forms.TextBox();
            this.lbl_WorkingTime = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu_SysTrayContext = new System.Windows.Forms.ContextMenu();
            this.menuItem_StartCounting = new System.Windows.Forms.MenuItem();
            this.menuItem_StopCounting = new System.Windows.Forms.MenuItem();
            this.menuItem_Quit = new System.Windows.Forms.MenuItem();
            this.timer_NotifySingleClick = new System.Windows.Forms.Timer(this.components);
            this.btn_DeleteLog = new System.Windows.Forms.Button();
            this.btn_Options = new System.Windows.Forms.Button();
            this.dataGridView_TaskLogList = new System.Windows.Forms.DataGridView();
            this.StartDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Billable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TimeTaken = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSet1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new NanoTimeTracker.DataSet1();
            this.lbl_TotalWorkingTime = new System.Windows.Forms.Label();
            this.lbl_BillableWorkingTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TaskLogList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(8, 8);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "Start";
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Location = new System.Drawing.Point(96, 8);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(75, 23);
            this.btn_Stop.TabIndex = 1;
            this.btn_Stop.Text = "Stop";
            this.btn_Stop.Visible = false;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // timer_StatusUpdate
            // 
            this.timer_StatusUpdate.Interval = 1000;
            this.timer_StatusUpdate.Tick += new System.EventHandler(this.timer_StatusUpdate_Tick);
            // 
            // txt_LogBox
            // 
            this.txt_LogBox.AcceptsReturn = true;
            this.txt_LogBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_LogBox.Enabled = false;
            this.txt_LogBox.Location = new System.Drawing.Point(8, 63);
            this.txt_LogBox.Multiline = true;
            this.txt_LogBox.Name = "txt_LogBox";
            this.txt_LogBox.Size = new System.Drawing.Size(638, 118);
            this.txt_LogBox.TabIndex = 2;
            // 
            // lbl_WorkingTime
            // 
            this.lbl_WorkingTime.Location = new System.Drawing.Point(213, 5);
            this.lbl_WorkingTime.Name = "lbl_WorkingTime";
            this.lbl_WorkingTime.Size = new System.Drawing.Size(100, 15);
            this.lbl_WorkingTime.TabIndex = 3;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenu = this.contextMenu_SysTrayContext;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Time Logger";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDown);
            // 
            // contextMenu_SysTrayContext
            // 
            this.contextMenu_SysTrayContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_StartCounting,
            this.menuItem_StopCounting,
            this.menuItem_Quit});
            // 
            // menuItem_StartCounting
            // 
            this.menuItem_StartCounting.Index = 0;
            this.menuItem_StartCounting.Text = "Start Counting";
            this.menuItem_StartCounting.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // menuItem_StopCounting
            // 
            this.menuItem_StopCounting.Enabled = false;
            this.menuItem_StopCounting.Index = 1;
            this.menuItem_StopCounting.Text = "Stop Counting";
            this.menuItem_StopCounting.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // menuItem_Quit
            // 
            this.menuItem_Quit.Index = 2;
            this.menuItem_Quit.Text = "Quit";
            this.menuItem_Quit.Click += new System.EventHandler(this.menuItem_Quit_Click);
            // 
            // timer_NotifySingleClick
            // 
            this.timer_NotifySingleClick.Interval = 500;
            this.timer_NotifySingleClick.Tick += new System.EventHandler(this.timer_NotifySingleClick_Tick);
            // 
            // btn_DeleteLog
            // 
            this.btn_DeleteLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_DeleteLog.Location = new System.Drawing.Point(8, 441);
            this.btn_DeleteLog.Name = "btn_DeleteLog";
            this.btn_DeleteLog.Size = new System.Drawing.Size(144, 23);
            this.btn_DeleteLog.TabIndex = 4;
            this.btn_DeleteLog.Text = "Delete Log";
            this.btn_DeleteLog.Click += new System.EventHandler(this.btn_DeleteLog_Click);
            // 
            // btn_Options
            // 
            this.btn_Options.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Options.Location = new System.Drawing.Point(502, 441);
            this.btn_Options.Name = "btn_Options";
            this.btn_Options.Size = new System.Drawing.Size(144, 24);
            this.btn_Options.TabIndex = 5;
            this.btn_Options.Text = "Options";
            this.btn_Options.Click += new System.EventHandler(this.btn_Options_Click);
            // 
            // dataGridView_TaskLogList
            // 
            this.dataGridView_TaskLogList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_TaskLogList.AutoGenerateColumns = false;
            this.dataGridView_TaskLogList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_TaskLogList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StartDateTime,
            this.EndDateTime,
            this.TaskName,
            this.Billable,
            this.TimeTaken});
            this.dataGridView_TaskLogList.DataMember = "DataTable1";
            this.dataGridView_TaskLogList.DataSource = this.dataSet1BindingSource;
            this.dataGridView_TaskLogList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridView_TaskLogList.Location = new System.Drawing.Point(8, 188);
            this.dataGridView_TaskLogList.Name = "dataGridView_TaskLogList";
            this.dataGridView_TaskLogList.Size = new System.Drawing.Size(638, 241);
            this.dataGridView_TaskLogList.TabIndex = 6;
            this.dataGridView_TaskLogList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_TaskLogList_CellValueChanged);
            // 
            // StartDateTime
            // 
            this.StartDateTime.DataPropertyName = "StartDateTime";
            this.StartDateTime.HeaderText = "StartDateTime";
            this.StartDateTime.Name = "StartDateTime";
            this.StartDateTime.Width = 120;
            // 
            // EndDateTime
            // 
            this.EndDateTime.DataPropertyName = "EndDateTime";
            this.EndDateTime.HeaderText = "EndDateTime";
            this.EndDateTime.Name = "EndDateTime";
            this.EndDateTime.Width = 120;
            // 
            // TaskName
            // 
            this.TaskName.DataPropertyName = "TaskName";
            this.TaskName.HeaderText = "TaskName";
            this.TaskName.Name = "TaskName";
            this.TaskName.Width = 200;
            // 
            // Billable
            // 
            this.Billable.DataPropertyName = "BillableFlag";
            this.Billable.HeaderText = "Billable";
            this.Billable.Name = "Billable";
            this.Billable.Width = 50;
            // 
            // TimeTaken
            // 
            this.TimeTaken.DataPropertyName = "TimeTaken";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.TimeTaken.DefaultCellStyle = dataGridViewCellStyle2;
            this.TimeTaken.HeaderText = "TimeTaken";
            this.TimeTaken.Name = "TimeTaken";
            this.TimeTaken.Width = 80;
            // 
            // dataSet1BindingSource
            // 
            this.dataSet1BindingSource.DataSource = this.dataSet1;
            this.dataSet1BindingSource.Position = 0;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lbl_TotalWorkingTime
            // 
            this.lbl_TotalWorkingTime.Location = new System.Drawing.Point(213, 20);
            this.lbl_TotalWorkingTime.Name = "lbl_TotalWorkingTime";
            this.lbl_TotalWorkingTime.Size = new System.Drawing.Size(100, 15);
            this.lbl_TotalWorkingTime.TabIndex = 7;
            // 
            // lbl_BillableWorkingTime
            // 
            this.lbl_BillableWorkingTime.Location = new System.Drawing.Point(213, 35);
            this.lbl_BillableWorkingTime.Name = "lbl_BillableWorkingTime";
            this.lbl_BillableWorkingTime.Size = new System.Drawing.Size(100, 15);
            this.lbl_BillableWorkingTime.TabIndex = 8;
            // 
            // LogWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(654, 476);
            this.Controls.Add(this.lbl_BillableWorkingTime);
            this.Controls.Add(this.lbl_TotalWorkingTime);
            this.Controls.Add(this.dataGridView_TaskLogList);
            this.Controls.Add(this.btn_Options);
            this.Controls.Add(this.btn_DeleteLog);
            this.Controls.Add(this.lbl_WorkingTime);
            this.Controls.Add(this.txt_LogBox);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.btn_Start);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogWindow";
            this.Text = "LogWindow";
            this.Load += new System.EventHandler(this.LogWindow_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.LogWindow_Closing);
            this.Resize += new System.EventHandler(this.LogWindow_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TaskLogList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Button btn_Start;
        private Button btn_Stop;
        private Button btn_DeleteLog;
        private Button btn_Options;
        private TextBox txt_LogBox;
        private Label lbl_WorkingTime;
        private Label lbl_TotalWorkingTime;
        private Label lbl_BillableWorkingTime;
        private NotifyIcon notifyIcon1;
        private ContextMenu contextMenu_SysTrayContext;
        private MenuItem menuItem_StartCounting;
        private MenuItem menuItem_StopCounting;
        private MenuItem menuItem_Quit;
        private Timer timer_StatusUpdate;
        private Timer timer_NotifySingleClick;
        private DataGridView dataGridView_TaskLogList;
        private BindingSource dataSet1BindingSource;
        private DataSet1 dataSet1;
        private DataGridViewTextBoxColumn StartDateTime;
        private DataGridViewTextBoxColumn EndDateTime;
        private DataGridViewTextBoxColumn TaskName;
        private DataGridViewCheckBoxColumn Billable;
        private DataGridViewTextBoxColumn TimeTaken;
    }
}

