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
            this.lbl_WorkingTimeValue = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip_SysTrayContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openLogWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopEditTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_NotifySingleClick = new System.Windows.Forms.Timer(this.components);
            this.dataGridView_TaskLogList = new System.Windows.Forms.DataGridView();
            this.dataSet1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new NanoTimeTracker.DataSet1();
            this.lbl_TimeTodayValue = new System.Windows.Forms.Label();
            this.lbl_BillableTimeTodayValue = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbl_WorkingTimeLabel = new System.Windows.Forms.Label();
            this.lbl_BillableTodayLabel = new System.Windows.Forms.Label();
            this.lbl_TimeTodayLabel = new System.Windows.Forms.Label();
            this.grp_Status = new System.Windows.Forms.GroupBox();
            this.StartDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Billable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TimeTaken = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip_SysTrayContext.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TaskLogList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.grp_Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Start
            // 
            this.btn_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Start.Location = new System.Drawing.Point(12, 337);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(106, 37);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "Start";
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Stop.Location = new System.Drawing.Point(12, 379);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(106, 37);
            this.btn_Stop.TabIndex = 1;
            this.btn_Stop.Text = "Stop / Edit";
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // timer_StatusUpdate
            // 
            this.timer_StatusUpdate.Interval = 1000;
            this.timer_StatusUpdate.Tick += new System.EventHandler(this.timer_StatusUpdate_Tick);
            // 
            // lbl_WorkingTimeValue
            // 
            this.lbl_WorkingTimeValue.AutoSize = true;
            this.lbl_WorkingTimeValue.Location = new System.Drawing.Point(112, 23);
            this.lbl_WorkingTimeValue.Name = "lbl_WorkingTimeValue";
            this.lbl_WorkingTimeValue.Size = new System.Drawing.Size(49, 13);
            this.lbl_WorkingTimeValue.TabIndex = 3;
            this.lbl_WorkingTimeValue.Text = "00:00:00";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip_SysTrayContext;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Time Logger";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDown);
            // 
            // contextMenuStrip_SysTrayContext
            // 
            this.contextMenuStrip_SysTrayContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openLogWindowToolStripMenuItem,
            this.startTaskToolStripMenuItem,
            this.stopEditTaskToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.contextMenuStrip_SysTrayContext.Name = "contextMenuStrip_SysTrayContext";
            this.contextMenuStrip_SysTrayContext.Size = new System.Drawing.Size(183, 92);
            // 
            // openLogWindowToolStripMenuItem
            // 
            this.openLogWindowToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openLogWindowToolStripMenuItem.Name = "openLogWindowToolStripMenuItem";
            this.openLogWindowToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.openLogWindowToolStripMenuItem.Text = "Open Log Window...";
            this.openLogWindowToolStripMenuItem.Click += new System.EventHandler(this.openLogWindowToolStripMenuItem_Click);
            // 
            // startTaskToolStripMenuItem
            // 
            this.startTaskToolStripMenuItem.Name = "startTaskToolStripMenuItem";
            this.startTaskToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.startTaskToolStripMenuItem.Text = "Start Task...";
            this.startTaskToolStripMenuItem.Click += new System.EventHandler(this.startTaskToolStripMenuItem_Click);
            // 
            // stopEditTaskToolStripMenuItem
            // 
            this.stopEditTaskToolStripMenuItem.Name = "stopEditTaskToolStripMenuItem";
            this.stopEditTaskToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.stopEditTaskToolStripMenuItem.Text = "Stop/Edit Task...";
            this.stopEditTaskToolStripMenuItem.Click += new System.EventHandler(this.stopEditTaskToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(182, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // timer_NotifySingleClick
            // 
            this.timer_NotifySingleClick.Interval = 500;
            this.timer_NotifySingleClick.Tick += new System.EventHandler(this.timer_NotifySingleClick_Tick);
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
            this.TaskCategory,
            this.TaskName,
            this.Billable,
            this.TimeTaken});
            this.dataGridView_TaskLogList.DataMember = "DataTable1";
            this.dataGridView_TaskLogList.DataSource = this.dataSet1BindingSource;
            this.dataGridView_TaskLogList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridView_TaskLogList.Location = new System.Drawing.Point(8, 27);
            this.dataGridView_TaskLogList.Name = "dataGridView_TaskLogList";
            this.dataGridView_TaskLogList.Size = new System.Drawing.Size(801, 293);
            this.dataGridView_TaskLogList.TabIndex = 6;
            this.dataGridView_TaskLogList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_TaskLogList_CellValueChanged);
            this.dataGridView_TaskLogList.DoubleClick += new System.EventHandler(this.dataGridView_TaskLogList_DoubleClick);
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
            // lbl_TimeTodayValue
            // 
            this.lbl_TimeTodayValue.AutoSize = true;
            this.lbl_TimeTodayValue.Location = new System.Drawing.Point(112, 46);
            this.lbl_TimeTodayValue.Name = "lbl_TimeTodayValue";
            this.lbl_TimeTodayValue.Size = new System.Drawing.Size(49, 13);
            this.lbl_TimeTodayValue.TabIndex = 7;
            this.lbl_TimeTodayValue.Text = "00:00:00";
            // 
            // lbl_BillableTimeTodayValue
            // 
            this.lbl_BillableTimeTodayValue.AutoSize = true;
            this.lbl_BillableTimeTodayValue.Location = new System.Drawing.Point(112, 69);
            this.lbl_BillableTimeTodayValue.Name = "lbl_BillableTimeTodayValue";
            this.lbl_BillableTimeTodayValue.Size = new System.Drawing.Size(49, 13);
            this.lbl_BillableTimeTodayValue.TabIndex = 8;
            this.lbl_BillableTimeTodayValue.Text = "00:00:00";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(817, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteLogToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // deleteLogToolStripMenuItem
            // 
            this.deleteLogToolStripMenuItem.Name = "deleteLogToolStripMenuItem";
            this.deleteLogToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteLogToolStripMenuItem.Text = "Delete Log...";
            this.deleteLogToolStripMenuItem.Click += new System.EventHandler(this.deleteLogToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlineHelpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // onlineHelpToolStripMenuItem
            // 
            this.onlineHelpToolStripMenuItem.Name = "onlineHelpToolStripMenuItem";
            this.onlineHelpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.onlineHelpToolStripMenuItem.Text = "Online Help";
            this.onlineHelpToolStripMenuItem.Click += new System.EventHandler(this.onlineHelpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // lbl_WorkingTimeLabel
            // 
            this.lbl_WorkingTimeLabel.AutoSize = true;
            this.lbl_WorkingTimeLabel.Location = new System.Drawing.Point(6, 23);
            this.lbl_WorkingTimeLabel.Name = "lbl_WorkingTimeLabel";
            this.lbl_WorkingTimeLabel.Size = new System.Drawing.Size(60, 13);
            this.lbl_WorkingTimeLabel.TabIndex = 10;
            this.lbl_WorkingTimeLabel.Text = "Task Time:";
            // 
            // lbl_BillableTodayLabel
            // 
            this.lbl_BillableTodayLabel.AutoSize = true;
            this.lbl_BillableTodayLabel.Location = new System.Drawing.Point(6, 69);
            this.lbl_BillableTodayLabel.Name = "lbl_BillableTodayLabel";
            this.lbl_BillableTodayLabel.Size = new System.Drawing.Size(102, 13);
            this.lbl_BillableTodayLabel.TabIndex = 12;
            this.lbl_BillableTodayLabel.Text = "Billable Time Today:";
            // 
            // lbl_TimeTodayLabel
            // 
            this.lbl_TimeTodayLabel.AutoSize = true;
            this.lbl_TimeTodayLabel.Location = new System.Drawing.Point(6, 46);
            this.lbl_TimeTodayLabel.Name = "lbl_TimeTodayLabel";
            this.lbl_TimeTodayLabel.Size = new System.Drawing.Size(66, 13);
            this.lbl_TimeTodayLabel.TabIndex = 11;
            this.lbl_TimeTodayLabel.Text = "Time Today:";
            // 
            // grp_Status
            // 
            this.grp_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grp_Status.Controls.Add(this.lbl_BillableTodayLabel);
            this.grp_Status.Controls.Add(this.lbl_WorkingTimeLabel);
            this.grp_Status.Controls.Add(this.lbl_BillableTimeTodayValue);
            this.grp_Status.Controls.Add(this.lbl_WorkingTimeValue);
            this.grp_Status.Controls.Add(this.lbl_TimeTodayLabel);
            this.grp_Status.Controls.Add(this.lbl_TimeTodayValue);
            this.grp_Status.Location = new System.Drawing.Point(133, 326);
            this.grp_Status.Name = "grp_Status";
            this.grp_Status.Size = new System.Drawing.Size(258, 100);
            this.grp_Status.TabIndex = 12;
            this.grp_Status.TabStop = false;
            this.grp_Status.Text = "Status";
            // 
            // StartDateTime
            // 
            this.StartDateTime.DataPropertyName = "StartDateTime";
            this.StartDateTime.HeaderText = "StartDateTime";
            this.StartDateTime.Name = "StartDateTime";
            this.StartDateTime.Width = 130;
            // 
            // EndDateTime
            // 
            this.EndDateTime.DataPropertyName = "EndDateTime";
            this.EndDateTime.HeaderText = "EndDateTime";
            this.EndDateTime.Name = "EndDateTime";
            this.EndDateTime.Width = 130;
            // 
            // TaskCategory
            // 
            this.TaskCategory.DataPropertyName = "TaskCategory";
            this.TaskCategory.HeaderText = "TaskCategory";
            this.TaskCategory.Name = "TaskCategory";
            this.TaskCategory.Width = 150;
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
            // LogWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(817, 438);
            this.Controls.Add(this.dataGridView_TaskLogList);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.grp_Status);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(436, 306);
            this.Name = "LogWindow";
            this.Text = "Nano TimeTracker - Log Window";
            this.Load += new System.EventHandler(this.LogWindow_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogWindow_FormClosing);
            this.contextMenuStrip_SysTrayContext.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TaskLogList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grp_Status.ResumeLayout(false);
            this.grp_Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Button btn_Start;
        private Button btn_Stop;
        private Label lbl_WorkingTimeValue;
        private Label lbl_TimeTodayValue;
        private Label lbl_BillableTimeTodayValue;
        private NotifyIcon notifyIcon1;
        private Timer timer_StatusUpdate;
        private Timer timer_NotifySingleClick;
        private DataGridView dataGridView_TaskLogList;
        private BindingSource dataSet1BindingSource;
        private DataSet1 dataSet1;
        private MenuStrip menuStrip1;
        private Label lbl_WorkingTimeLabel;
        private Label lbl_TimeTodayLabel;
        private Label lbl_BillableTodayLabel;
        private GroupBox grp_Status;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem deleteLogToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem onlineHelpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip_SysTrayContext;
        private ToolStripMenuItem openLogWindowToolStripMenuItem;
        private ToolStripMenuItem startTaskToolStripMenuItem;
        private ToolStripMenuItem stopEditTaskToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private DataGridViewTextBoxColumn StartDateTime;
        private DataGridViewTextBoxColumn EndDateTime;
        private DataGridViewTextBoxColumn TaskCategory;
        private DataGridViewTextBoxColumn TaskName;
        private DataGridViewCheckBoxColumn Billable;
        private DataGridViewTextBoxColumn TimeTaken;

    }
}

