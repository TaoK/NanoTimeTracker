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

namespace NanoTimeTracker.Dialogs
{
    public partial class DismissableConfirmationWindow : Form
    {
        private DismissableConfirmationWindow()
        {
            InitializeComponent();
        }

        public static DialogResult ShowMessage(string confirmationTitle, string confirmationMessage, out bool permanentlyDismissed)
        {
            DismissableConfirmationWindow thisWindow = new DismissableConfirmationWindow();
            thisWindow.Text = confirmationTitle;
            thisWindow.lbl_ConfirmationText.Text = confirmationMessage;
            DialogResult result = thisWindow.ShowDialog();
            permanentlyDismissed = thisWindow.chk_DontShowAgain.Checked;
            thisWindow.Dispose();
            return result;
        }
    }
}
