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
    partial class DismissableConfirmationWindow
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
            this.btn_OK = new System.Windows.Forms.Button();
            this.lbl_ConfirmationText = new System.Windows.Forms.Label();
            this.chk_DontShowAgain = new System.Windows.Forms.CheckBox();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(225, 94);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // lbl_ConfirmationText
            // 
            this.lbl_ConfirmationText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_ConfirmationText.Location = new System.Drawing.Point(12, 9);
            this.lbl_ConfirmationText.Name = "lbl_ConfirmationText";
            this.lbl_ConfirmationText.Size = new System.Drawing.Size(369, 82);
            this.lbl_ConfirmationText.TabIndex = 1;
            this.lbl_ConfirmationText.Text = "(confirmation message)";
            // 
            // chk_DontShowAgain
            // 
            this.chk_DontShowAgain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chk_DontShowAgain.AutoSize = true;
            this.chk_DontShowAgain.Location = new System.Drawing.Point(15, 98);
            this.chk_DontShowAgain.Name = "chk_DontShowAgain";
            this.chk_DontShowAgain.Size = new System.Drawing.Size(127, 17);
            this.chk_DontShowAgain.TabIndex = 0;
            this.chk_DontShowAgain.Text = "Don\'t show this again";
            this.chk_DontShowAgain.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(306, 94);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // DismissableConfirmationWindow
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(393, 129);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.chk_DontShowAgain);
            this.Controls.Add(this.lbl_ConfirmationText);
            this.Controls.Add(this.btn_OK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DismissableConfirmationWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "(confirmation title)";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label lbl_ConfirmationText;
        private System.Windows.Forms.CheckBox chk_DontShowAgain;
        private System.Windows.Forms.Button btn_Cancel;
    }
}