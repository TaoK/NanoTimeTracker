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

namespace NanoTimeTracker
{
    partial class DateNavigator
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_DateNext = new System.Windows.Forms.Button();
            this.btn_DatePrev = new System.Windows.Forms.Button();
            this.datePicker_FilterDate = new System.Windows.Forms.DateTimePicker();
            this.btn_DateLast = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_DateNext
            // 
            this.btn_DateNext.Location = new System.Drawing.Point(112, 3);
            this.btn_DateNext.Name = "btn_DateNext";
            this.btn_DateNext.Size = new System.Drawing.Size(18, 20);
            this.btn_DateNext.TabIndex = 3;
            this.btn_DateNext.Text = ">";
            this.btn_DateNext.UseVisualStyleBackColor = true;
            this.btn_DateNext.Click += new System.EventHandler(this.btn_DateNext_Click);
            // 
            // btn_DatePrev
            // 
            this.btn_DatePrev.Location = new System.Drawing.Point(3, 3);
            this.btn_DatePrev.Name = "btn_DatePrev";
            this.btn_DatePrev.Size = new System.Drawing.Size(18, 20);
            this.btn_DatePrev.TabIndex = 1;
            this.btn_DatePrev.Text = "<";
            this.btn_DatePrev.UseVisualStyleBackColor = true;
            this.btn_DatePrev.Click += new System.EventHandler(this.btn_DatePrev_Click);
            // 
            // datePicker_FilterDate
            // 
            this.datePicker_FilterDate.CustomFormat = "yyyy-MM-dd";
            this.datePicker_FilterDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datePicker_FilterDate.Location = new System.Drawing.Point(27, 3);
            this.datePicker_FilterDate.Name = "datePicker_FilterDate";
            this.datePicker_FilterDate.Size = new System.Drawing.Size(79, 20);
            this.datePicker_FilterDate.TabIndex = 2;
            this.datePicker_FilterDate.ValueChanged += new System.EventHandler(this.datePicker_FilterDate_ValueChanged);
            // 
            // btn_DateLast
            // 
            this.btn_DateLast.Location = new System.Drawing.Point(136, 3);
            this.btn_DateLast.Name = "btn_DateLast";
            this.btn_DateLast.Size = new System.Drawing.Size(30, 20);
            this.btn_DateLast.TabIndex = 4;
            this.btn_DateLast.Text = ">>";
            this.btn_DateLast.UseVisualStyleBackColor = true;
            this.btn_DateLast.Click += new System.EventHandler(this.btn_DateLast_Click);
            // 
            // DateNavigator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_DateLast);
            this.Controls.Add(this.btn_DateNext);
            this.Controls.Add(this.btn_DatePrev);
            this.Controls.Add(this.datePicker_FilterDate);
            this.Name = "DateNavigator";
            this.Size = new System.Drawing.Size(169, 27);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_DateNext;
        private System.Windows.Forms.Button btn_DatePrev;
        private System.Windows.Forms.DateTimePicker datePicker_FilterDate;
        private System.Windows.Forms.Button btn_DateLast;
    }
}
