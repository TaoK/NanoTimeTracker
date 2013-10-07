
Nano TimeTracker
----------------

This is a simple .Net 2.0, WinForms C# app that sits in your system tray tracking 
the start and end of tasks you wish to log. There are already numerous time tracking
applications out there that do this, but in my (admittedly cursory) search I 
couldn't find any that are free and customizable, windows-native, and reduce the 
overhead to one click and a task name/description typed in.

Features:
 - System tray integration so you can see and update status without cluttering
 - Hotkey support for instant and mouse-less task entry (Ctrl-Win-T)
 - Grid-view of time entries, for easy review and editing
 - At-a-glance stats, including billable and non-billable time for recent periods
 - Simple Xml-based storage format
 - Single-file install-free program, with option to keep data with executable or 
     in standard user data folders
 - Export to CSV
 - Import from CSV
 - Autocomplete in task name and category tracking


This app is not really finished, many enhancements are possible and planned, most 
notably:
 - Overlapping time detection (on manual edit and import)
 - Hotkey selection
 - Tracking billability more easily
 - A more elegant and configurable data persistence mechanism
   - DB file change detection (eg on network share)
   - Optional external/web-based storage (eg google docs, dropbox, 
      etc - support a standard if possible), with sync/merge.
 - Options screen
 - Auto-Export
 - Auto-Backup
 - Simple stats screen, export screen, etc
 - Computer idle time detection
 - Translations
 - Better error-handling (PK violations, files in use, etc)
 
This application is released under GPLv3: http://www.gnu.org/licenses/gpl.html

Some icons used in the application are licensed under LGPL, see the 
"NanoTimeTracker/Icons" folder for details.

This application uses the ManagedWinAPI library, licensed under LGPL; see the 
"NanoTimeTracker/References/ManagedWinAPI" folder for details.

This application uses the LinqBridge library, for convenience, supporting extension 
methods and Linq-to-Objects despite this being a .Net 2.0 application. LinqBridge 
is licensed under the BSD 3-clause license (details in 
"NanoTimeTracker/References/LinqBridge"), and its homepage is here: 
http://code.google.com/p/linqbridge/

This application uses the LumenWorks.Framework.IO library, licensed under the MIT
license; see the "NanoTimeTracker/References/LumenWorks.Framework.IO" folder for 
details.

Please contact me with any questions, concerns, or issues: my email address starts
with tao, and is hosted at klerks dot biz.

Tao Klerks
