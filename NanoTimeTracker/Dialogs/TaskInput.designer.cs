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

namespace NanoTimeTracker.Dialogs
{
    partial class TaskInput
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
            this.txt_Description = new FrameworkClassReplacements.MultilineAutoCompleteTextBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.lbl_Description = new System.Windows.Forms.Label();
            this.lbl_Category = new System.Windows.Forms.Label();
            this.txt_Category = new System.Windows.Forms.TextBox();
            this.lbl_EntryPrompt = new System.Windows.Forms.Label();
            this.chk_Billable = new System.Windows.Forms.CheckBox();
            this.lbl_StartDate = new System.Windows.Forms.Label();
            this.txt_StartedAt = new System.Windows.Forms.TextBox();
            this.lbl_CompletedAt = new System.Windows.Forms.Label();
            this.txt_CompletedAt = new System.Windows.Forms.TextBox();
            this.chk_TaskCompleted = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txt_Description
            // 
            this.txt_Description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Description.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_Description.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_Description.Location = new System.Drawing.Point(102, 29);
            this.txt_Description.Multiline = true;
            this.txt_Description.Name = "txt_Description";
            this.txt_Description.Size = new System.Drawing.Size(426, 55);
            this.txt_Description.TabIndex = 0;
            this.txt_Description.TextChanged += new System.EventHandler(this.txt_Description_TextChanged);
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(372, 158);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 8;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(453, 158);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 9;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // lbl_Description
            // 
            this.lbl_Description.Location = new System.Drawing.Point(12, 32);
            this.lbl_Description.Name = "lbl_Description";
            this.lbl_Description.Size = new System.Drawing.Size(84, 16);
            this.lbl_Description.TabIndex = 3;
            this.lbl_Description.Text = "Description:";
            this.lbl_Description.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_Category
            // 
            this.lbl_Category.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_Category.Location = new System.Drawing.Point(12, 93);
            this.lbl_Category.Name = "lbl_Category";
            this.lbl_Category.Size = new System.Drawing.Size(84, 17);
            this.lbl_Category.TabIndex = 4;
            this.lbl_Category.Text = "Category:";
            this.lbl_Category.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_Category
            // 
            this.txt_Category.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Category.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_Category.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_Category.Location = new System.Drawing.Point(102, 90);
            this.txt_Category.Name = "txt_Category";
            this.txt_Category.Size = new System.Drawing.Size(361, 20);
            this.txt_Category.TabIndex = 1;
            this.txt_Category.TextChanged += new System.EventHandler(this.txt_Category_TextChanged);
            // 
            // lbl_EntryPrompt
            // 
            this.lbl_EntryPrompt.Location = new System.Drawing.Point(68, 9);
            this.lbl_EntryPrompt.Name = "lbl_EntryPrompt";
            this.lbl_EntryPrompt.Size = new System.Drawing.Size(346, 17);
            this.lbl_EntryPrompt.TabIndex = 7;
            this.lbl_EntryPrompt.Text = "Task details:";
            // 
            // chk_Billable
            // 
            this.chk_Billable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_Billable.AutoSize = true;
            this.chk_Billable.Location = new System.Drawing.Point(469, 92);
            this.chk_Billable.Name = "chk_Billable";
            this.chk_Billable.Size = new System.Drawing.Size(59, 17);
            this.chk_Billable.TabIndex = 2;
            this.chk_Billable.Text = "Billable";
            this.chk_Billable.UseVisualStyleBackColor = true;
            // 
            // lbl_StartDate
            // 
            this.lbl_StartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_StartDate.Location = new System.Drawing.Point(15, 119);
            this.lbl_StartDate.Name = "lbl_StartDate";
            this.lbl_StartDate.Size = new System.Drawing.Size(81, 17);
            this.lbl_StartDate.TabIndex = 9;
            this.lbl_StartDate.Text = "Started At:";
            this.lbl_StartDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_StartedAt
            // 
            this.txt_StartedAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_StartedAt.Location = new System.Drawing.Point(102, 116);
            this.txt_StartedAt.Name = "txt_StartedAt";
            this.txt_StartedAt.Size = new System.Drawing.Size(114, 20);
            this.txt_StartedAt.TabIndex = 3;
            this.txt_StartedAt.TextChanged += new System.EventHandler(this.txt_StartedAt_TextChanged);
            // 
            // lbl_CompletedAt
            // 
            this.lbl_CompletedAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_CompletedAt.Location = new System.Drawing.Point(12, 145);
            this.lbl_CompletedAt.Name = "lbl_CompletedAt";
            this.lbl_CompletedAt.Size = new System.Drawing.Size(84, 17);
            this.lbl_CompletedAt.TabIndex = 11;
            this.lbl_CompletedAt.Text = "Completed At:";
            this.lbl_CompletedAt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_CompletedAt
            // 
            this.txt_CompletedAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_CompletedAt.Location = new System.Drawing.Point(102, 142);
            this.txt_CompletedAt.Name = "txt_CompletedAt";
            this.txt_CompletedAt.Size = new System.Drawing.Size(114, 20);
            this.txt_CompletedAt.TabIndex = 4;
            this.txt_CompletedAt.TextChanged += new System.EventHandler(this.txt_CompletedAt_TextChanged);
            // 
            // chk_TaskCompleted
            // 
            this.chk_TaskCompleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chk_TaskCompleted.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chk_TaskCompleted.Location = new System.Drawing.Point(222, 144);
            this.chk_TaskCompleted.Name = "chk_TaskCompleted";
            this.chk_TaskCompleted.Size = new System.Drawing.Size(121, 37);
            this.chk_TaskCompleted.TabIndex = 5;
            this.chk_TaskCompleted.Text = "Completed";
            this.chk_TaskCompleted.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chk_TaskCompleted.UseVisualStyleBackColor = true;
            this.chk_TaskCompleted.CheckedChanged += new System.EventHandler(this.chk_TaskCompleted_CheckedChanged);
            // 
            // TaskInput
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(540, 193);
            this.Controls.Add(this.chk_TaskCompleted);
            this.Controls.Add(this.txt_CompletedAt);
            this.Controls.Add(this.lbl_CompletedAt);
            this.Controls.Add(this.txt_StartedAt);
            this.Controls.Add(this.lbl_StartDate);
            this.Controls.Add(this.chk_Billable);
            this.Controls.Add(this.lbl_EntryPrompt);
            this.Controls.Add(this.txt_Category);
            this.Controls.Add(this.lbl_Category);
            this.Controls.Add(this.lbl_Description);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.txt_Description);
            this.MinimumSize = new System.Drawing.Size(528, 188);
            this.Name = "TaskInput";
            this.Text = "Task Entry";
            this.Load += new System.EventHandler(this.TaskInput_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskInput_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FrameworkClassReplacements.MultilineAutoCompleteTextBox txt_Description;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label lbl_Description;
        private System.Windows.Forms.Label lbl_Category;
        private System.Windows.Forms.TextBox txt_Category;
        private System.Windows.Forms.Label lbl_EntryPrompt;
        private System.Windows.Forms.CheckBox chk_Billable;
        private System.Windows.Forms.Label lbl_StartDate;
        private System.Windows.Forms.TextBox txt_StartedAt;
        private System.Windows.Forms.Label lbl_CompletedAt;
        private System.Windows.Forms.TextBox txt_CompletedAt;
        private System.Windows.Forms.CheckBox chk_TaskCompleted;
    }
}