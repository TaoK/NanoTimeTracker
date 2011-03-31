namespace NanoTimeTracker
{
    partial class CloseConfirmationWindow
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
            this.lbl_WindowClosingConfirmation = new System.Windows.Forms.Label();
            this.chk_DontShowAgain = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.Location = new System.Drawing.Point(278, 80);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // lbl_WindowClosingConfirmation
            // 
            this.lbl_WindowClosingConfirmation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_WindowClosingConfirmation.Location = new System.Drawing.Point(12, 9);
            this.lbl_WindowClosingConfirmation.Name = "lbl_WindowClosingConfirmation";
            this.lbl_WindowClosingConfirmation.Size = new System.Drawing.Size(341, 68);
            this.lbl_WindowClosingConfirmation.TabIndex = 1;
            this.lbl_WindowClosingConfirmation.Text = "The Nano TimeTracker log window is closing, but the program will continue to run " +
                "- you can start and stop tasks, or open the main window, by clicking on the icon" +
                " in the tray on the bottom right.";
            // 
            // chk_DontShowAgain
            // 
            this.chk_DontShowAgain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chk_DontShowAgain.AutoSize = true;
            this.chk_DontShowAgain.Location = new System.Drawing.Point(15, 84);
            this.chk_DontShowAgain.Name = "chk_DontShowAgain";
            this.chk_DontShowAgain.Size = new System.Drawing.Size(127, 17);
            this.chk_DontShowAgain.TabIndex = 3;
            this.chk_DontShowAgain.Text = "Don\'t show this again";
            this.chk_DontShowAgain.UseVisualStyleBackColor = true;
            // 
            // CloseConfirmationWindow
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_OK;
            this.ClientSize = new System.Drawing.Size(365, 115);
            this.Controls.Add(this.chk_DontShowAgain);
            this.Controls.Add(this.lbl_WindowClosingConfirmation);
            this.Controls.Add(this.btn_OK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CloseConfirmationWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Closing Window";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label lbl_WindowClosingConfirmation;
        private System.Windows.Forms.CheckBox chk_DontShowAgain;
    }
}