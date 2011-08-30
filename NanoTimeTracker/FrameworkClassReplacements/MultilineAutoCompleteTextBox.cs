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
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security;
using System.Linq;

namespace NanoTimeTracker.FrameworkClassReplacements
{
    class MultilineAutoCompleteTextBox : TextBox
    {
        //This is a very basic replacement for TextBox, providing support for Custom AutoComplete ("Suggest" mode)
        // when Multiline is set to True.

        public bool SkipSuggestionOnNextTextChange { get; set; }
        private string _preSuggestionText = null;

        private ListBox suggestionsBox;
        protected override void OnTextChanged(EventArgs e)
        {
            if (this.Multiline 
                && this.AutoCompleteMode == AutoCompleteMode.Suggest 
                && this.AutoCompleteSource == AutoCompleteSource.CustomSource
                && !SkipSuggestionOnNextTextChange
                )
            {
                _preSuggestionText = null;

                // set up suggestions box (NOT THREADSAFE AT ALL!!!)
                if (suggestionsBox == null)
                {
                    ListBox someBox = new ListBox();
                    someBox.Visible = false;
                    this.Parent.Controls.Add(someBox);
                    suggestionsBox = someBox;

                    Point suggestionsLocation = new Point(this.Left, this.Top + this.Height + 2);
                    this.suggestionsBox.Location = suggestionsLocation;

                    suggestionsBox.Width = this.Width - 4;
                    suggestionsBox.BorderStyle = BorderStyle.FixedSingle;

                    suggestionsBox.KeyDown += new KeyEventHandler(suggestionsBox_KeyDown);
                    suggestionsBox.MouseClick += new MouseEventHandler(suggestionsBox_MouseClick);
                    suggestionsBox.LostFocus += new EventHandler(suggestionsBox_LostFocus);
                }

                FillListBox();

                if (suggestionsBox.Items.Count == 0 && suggestionsBox.Visible)
                    suggestionsBox.Hide();
                else if (suggestionsBox.Items.Count > 0)
                {
                    if (suggestionsBox.Items.Count >= 5)
                        suggestionsBox.Height = (int)(suggestionsBox.GetItemHeight(0) * 5 * 1.5);
                    else
                        suggestionsBox.Height = (int)(suggestionsBox.GetItemHeight(0) * suggestionsBox.Items.Count * 1.5);

                    this.suggestionsBox.BringToFront();
                    this.suggestionsBox.Show();
                }
            }

            SkipSuggestionOnNextTextChange = false;
            base.OnTextChanged(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.suggestionsBox != null && this.suggestionsBox.Visible)
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (this.suggestionsBox.SelectedIndex < this.suggestionsBox.Items.Count - 1)
                    {
                        this.suggestionsBox.SelectedIndex++;

                        SkipSuggestionOnNextTextChange = true;
                        if (_preSuggestionText == null) _preSuggestionText = this.Text;
                        this.Text = this.suggestionsBox.SelectedItem.ToString();
                    }
                    else
                    {
                        this.suggestionsBox.SelectedIndex = -1;
                        if (_preSuggestionText != null)
                        {
                            SkipSuggestionOnNextTextChange = true;
                            this.Text = _preSuggestionText;
                            _preSuggestionText = null;
                        }
                    }

                    e.Handled = true;
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (this.suggestionsBox.SelectedIndex == 0)
                    {
                        this.suggestionsBox.SelectedIndex = -1;
                        if (_preSuggestionText != null)
                        {
                            SkipSuggestionOnNextTextChange = true;
                            this.Text = _preSuggestionText;
                            _preSuggestionText = null;
                        }
                    }
                    else
                    {
                        if (this.suggestionsBox.SelectedIndex > 0)
                        {
                            this.suggestionsBox.SelectedIndex--;
                        }
                        else
                        {
                            this.suggestionsBox.SelectedIndex = this.suggestionsBox.Items.Count - 1;
                        }

                        SkipSuggestionOnNextTextChange = true;
                        if (_preSuggestionText == null) _preSuggestionText = this.Text;
                        this.Text = this.suggestionsBox.SelectedItem.ToString();
                    }

                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    //note: this won't normally kick in, esp. on a dialog! (tab gets handled by the form, and in the case of a dialog the enter/return also)
                    if (this.suggestionsBox.SelectedIndex >= 0)
                    {
                        ProcessSelection();
                        e.Handled = true;
                    }
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (suggestionsBox != null
                && !suggestionsBox.Focused 
                && suggestionsBox.Visible
                )
                suggestionsBox.Hide();

            base.OnLostFocus(e);
        }

        void suggestionsBox_KeyDown(object sender, KeyEventArgs e)
        {
            // if keys are somehow pressed in the listview, redirect focus to the textbox.
            this.Focus();
            e.Handled = true;
        }

        void suggestionsBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.suggestionsBox.SelectedIndex >= 0)
            {
                ProcessSelection();
            }
        }

        void suggestionsBox_LostFocus(object sender, EventArgs e)
        {
            if (!this.Focused && suggestionsBox.Visible)
                suggestionsBox.Hide();
        }

        private void FillListBox()
        {
            suggestionsBox.Items.Clear();
            suggestionsBox.Items.AddRange(
                this.AutoCompleteCustomSource.OfType<string>().Where(
                    (entry) => entry.StartsWith(this.Text, StringComparison.InvariantCultureIgnoreCase)
                    ).ToArray()
                );
        }

        private void ProcessSelection()
        {
            this.Text = suggestionsBox.SelectedItem.ToString();
            suggestionsBox.Hide();
            this.Focus();
        }

    }
}
