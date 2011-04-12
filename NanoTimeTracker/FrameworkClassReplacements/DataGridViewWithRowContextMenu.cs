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
using System.Drawing;
using System.ComponentModel;

namespace NanoTimeTracker.FrameworkClassReplacements
{
    class DataGridViewWithRowContextMenu : DataGridView
    {
        //code inspired by "Tergiver"'s answer to a question about DGV context menu placement on key context:
        // http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/ef369cf3-58e9-4997-acc3-87a51d83011c
        //
        //also includes double-buffering fix for NVidia nastiness, found on stack overflow:
        // http://stackoverflow.com/questions/118528/horrible-redraw-performance-of-the-datagridview-on-one-of-my-two-screens

        public DataGridViewWithRowContextMenu()
        {
            DoubleBuffered = true;
        }

        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(null)]
        [Description("The shortcut menu to display when the user right-clicks a row.")]
        public ContextMenuStrip RowContextMenuStrip { get; set; }

        //to make sure the user understand what is going on (that the context menu applies to the row),
        //we need to select the entire row when a context menu is being displayed (on mouse event)
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (RowContextMenuStrip != null && e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                this.CurrentRow.Selected = false;
                this.Rows[e.RowIndex].Selected = true;
                this.CurrentCell = this.Rows[e.RowIndex].Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : 0];
            }
            base.OnCellMouseDown(e);
        }

        //this magically makes the context menu appear on row right-click.
        protected override void OnCellContextMenuStripNeeded(DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (RowContextMenuStrip != null && e.RowIndex >= 0)
            {
                e.ContextMenuStrip = RowContextMenuStrip;
            }
            base.OnCellContextMenuStripNeeded(e);
        }

        //here we need to hijack explicit keypresses, and this is really the cause for the existence of this class;
        //it seems ridiculous that there is no other way to make the context menu on a DataGridView keyboard event
        //appear at the current cell!
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (RowContextMenuStrip != null && (e.KeyData == Keys.Apps || e.KeyData == (Keys.F10 | Keys.Control)))
            {
                DataGridViewCell firstSelectedCellDisplayedOnSingleRow = null;
                foreach (DataGridViewCell someCell in this.SelectedCells)
                {
                    if (someCell.Displayed)
                    {
                        if (firstSelectedCellDisplayedOnSingleRow == null)
                        {
                            firstSelectedCellDisplayedOnSingleRow = someCell;
                        }
                        else if (firstSelectedCellDisplayedOnSingleRow.RowIndex != someCell.RowIndex)
                        {
                            firstSelectedCellDisplayedOnSingleRow = null;
                            break;
                        }
                    }
                }

                if (firstSelectedCellDisplayedOnSingleRow != null)
                {
                    this.Rows[firstSelectedCellDisplayedOnSingleRow.RowIndex].Selected = true;

                    ContextMenuStrip strip = firstSelectedCellDisplayedOnSingleRow.ContextMenuStrip;
                    if (strip != null)
                    {
                        Rectangle cellRect = this.GetCellDisplayRectangle(firstSelectedCellDisplayedOnSingleRow.ColumnIndex, firstSelectedCellDisplayedOnSingleRow.RowIndex, true);
                        Point point = new Point(cellRect.Left, cellRect.Bottom);
                        point = this.PointToScreen(point);
                        strip.Show(point);
                    }
                }
            }

            base.OnPreviewKeyDown(e);
        }
    }
}
