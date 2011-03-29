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
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NanoTimeTracker
{
    public static class WindowHacker
    {
        private const int SC_CLOSE = 0xF060;
        private const int MF_BYCOMMAND = 0x0;
        private const int MF_GRAYED = 0x1;
        private const int MF_ENABLED = 0x0;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetSystemMenu(IntPtr hWnd, int revert);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int EnableMenuItem(int menu, int ideEnableItem, int enable);

        public static void DisableCloseMenu(Form form)
        {
            IntPtr hWnd = form.Handle;
            int SystemMenu = GetSystemMenu(hWnd, 0);
            int PreviousState = EnableMenuItem(SystemMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
            if (PreviousState == -1)
                throw new Exception("The close menu does not exist");
        }

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}