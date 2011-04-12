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
using System.Text;
using System.Data;

namespace NanoTimeTracker
{
    static class Utils
    {
        public static string ExpandPath(string path, DateTime fileDate)
        {
            return path
                .Replace("<DATE>", "<YEAR>-<MONTH>-<DAY>")
                .Replace("<YEAR>", fileDate.ToString("yyyy"))
                .Replace("<MONTH>", fileDate.ToString("MM"))
                .Replace("<WEEK>", fileDate.ToString("ww"))
                .Replace("<DAY>", fileDate.ToString("dd"))
                .Replace("<MYDOCUMENTS>", Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToString()
                );
        }

        public static TimeSpan DecimalHoursToTimeSpan(double hours)
        {
            //accuracy to seconds only...
            return new TimeSpan((int)Math.Floor(hours), (int)Math.Floor((hours * 60) % 60), (int)Math.Floor((hours * 60 * 60) % 60));
        }

        public static string FormatDateFullTimeStamp(DateTime inputDate)
        {
            return inputDate.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        }

        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return String.Format("{0:00}", Math.Floor(timeSpan.TotalHours)) + ":" + String.Format("{0:00}", timeSpan.Minutes) + ":" + String.Format("{0:00}", timeSpan.Seconds);
        }

        public static void SaveDataSetSafe(DataSet targetDataSet, string targetFilePath)
        {
            targetDataSet.WriteXml(targetFilePath + ".tmp");
            System.IO.File.Copy(targetFilePath + ".tmp", targetFilePath, true);
            System.IO.File.Delete(targetFilePath + ".tmp");
        }

    }
}
