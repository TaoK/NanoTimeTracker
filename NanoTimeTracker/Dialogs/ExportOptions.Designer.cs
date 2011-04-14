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
    partial class ExportOptions
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
            this.dateTimePicker_FromDate = new System.Windows.Forms.DateTimePicker();
            this.lbl_DateFrom = new System.Windows.Forms.Label();
            this.lbl_ToDate = new System.Windows.Forms.Label();
            this.dateTimePicker_ToDate = new System.Windows.Forms.DateTimePicker();
            this.chk_BillableOnly = new System.Windows.Forms.CheckBox();
            this.btn_Export = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateTimePicker_FromDate
            // 
            this.dateTimePicker_FromDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_FromDate.Location = new System.Drawing.Point(87, 12);
            this.dateTimePicker_FromDate.Name = "dateTimePicker_FromDate";
            this.dateTimePicker_FromDate.Size = new System.Drawing.Size(241, 20);
            this.dateTimePicker_FromDate.TabIndex = 0;
            this.dateTimePicker_FromDate.ValueChanged += new System.EventHandler(this.dateTimePicker_FromDate_ValueChanged);
            // 
            // lbl_DateFrom
            // 
            this.lbl_DateFrom.AutoSize = true;
            this.lbl_DateFrom.Location = new System.Drawing.Point(12, 16);
            this.lbl_DateFrom.Name = "lbl_DateFrom";
            this.lbl_DateFrom.Size = new System.Drawing.Size(59, 13);
            this.lbl_DateFrom.TabIndex = 1;
            this.lbl_DateFrom.Text = "From Date:";
            // 
            // lbl_ToDate
            // 
            this.lbl_ToDate.AutoSize = true;
            this.lbl_ToDate.Location = new System.Drawing.Point(12, 42);
            this.lbl_ToDate.Name = "lbl_ToDate";
            this.lbl_ToDate.Size = new System.Drawing.Size(49, 13);
            this.lbl_ToDate.TabIndex = 1;
            this.lbl_ToDate.Text = "To Date:";
            // 
            // dateTimePicker_ToDate
            // 
            this.dateTimePicker_ToDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker_ToDate.Location = new System.Drawing.Point(87, 38);
            this.dateTimePicker_ToDate.Name = "dateTimePicker_ToDate";
            this.dateTimePicker_ToDate.Size = new System.Drawing.Size(241, 20);
            this.dateTimePicker_ToDate.TabIndex = 2;
            this.dateTimePicker_ToDate.ValueChanged += new System.EventHandler(this.dateTimePicker_ToDate_ValueChanged);
            // 
            // chk_BillableOnly
            // 
            this.chk_BillableOnly.AutoSize = true;
            this.chk_BillableOnly.Checked = true;
            this.chk_BillableOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_BillableOnly.Location = new System.Drawing.Point(87, 64);
            this.chk_BillableOnly.Name = "chk_BillableOnly";
            this.chk_BillableOnly.Size = new System.Drawing.Size(118, 17);
            this.chk_BillableOnly.TabIndex = 3;
            this.chk_BillableOnly.Text = "Billable Entries Only";
            this.chk_BillableOnly.UseVisualStyleBackColor = true;
            // 
            // btn_Export
            // 
            this.btn_Export.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_Export.Location = new System.Drawing.Point(172, 97);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(75, 23);
            this.btn_Export.TabIndex = 4;
            this.btn_Export.Text = "Export";
            this.btn_Export.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(253, 97);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // ExportOptions
            // 
            this.AcceptButton = this.btn_Export;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(340, 132);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.chk_BillableOnly);
            this.Controls.Add(this.dateTimePicker_ToDate);
            this.Controls.Add(this.lbl_ToDate);
            this.Controls.Add(this.lbl_DateFrom);
            this.Controls.Add(this.dateTimePicker_FromDate);
            this.Name = "ExportOptions";
            this.Text = "Export Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker_FromDate;
        private System.Windows.Forms.Label lbl_DateFrom;
        private System.Windows.Forms.Label lbl_ToDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_ToDate;
        private System.Windows.Forms.CheckBox chk_BillableOnly;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.Button btn_Cancel;
    }
}