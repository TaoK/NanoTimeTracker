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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ManagedWinapi;

namespace NanoTimeTracker
{
    public class AutoCompletionCache
    {

        private AutoCompleteStringCollection _names;
        private Dictionary<string, AutoCompleteStringCollection> _nameCategories;
        private Dictionary<string, bool?> _nameBillableDefaults;
        private AutoCompleteStringCollection _categories;
        private Dictionary<string, bool?> _categoryBillableDefaults;

        public AutoCompletionCache()
        {
            _names = new AutoCompleteStringCollection();
            _nameCategories = new Dictionary<string, AutoCompleteStringCollection>(StringComparer.InvariantCultureIgnoreCase);
            _nameBillableDefaults = new Dictionary<string, bool?>(StringComparer.InvariantCultureIgnoreCase);
            _categories = new AutoCompleteStringCollection();
            _categoryBillableDefaults = new Dictionary<string, bool?>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void Feed(string taskName, string taskCategory, bool taskBillable)
        {
            if (!string.IsNullOrEmpty(taskName))
            {

                if (!_names.Contains(taskName))
                    _names.Add(taskName);

                if (!_nameBillableDefaults.ContainsKey(taskName))
                    _nameBillableDefaults.Add(taskName, taskBillable);
                else if (_nameBillableDefaults[taskName] != null
                    && _nameBillableDefaults[taskName] != taskBillable
                    )
                    _nameBillableDefaults[taskName] = null;

                if (!_nameCategories.ContainsKey(taskName))
                    _nameCategories.Add(taskName, new AutoCompleteStringCollection());

                if (!string.IsNullOrEmpty(taskCategory)
                    && !_nameCategories[taskName].Contains(taskCategory)
                    )
                    _nameCategories[taskName].Add(taskCategory);
            }

            if (!string.IsNullOrEmpty(taskCategory))
            {
                if (!_categories.Contains(taskCategory))
                    _categories.Add(taskCategory);

                if (!_categoryBillableDefaults.ContainsKey(taskCategory))
                    _categoryBillableDefaults.Add(taskCategory, taskBillable);
                else if (_categoryBillableDefaults[taskCategory] != null 
                    && _categoryBillableDefaults[taskCategory] != taskBillable
                    )
                    _categoryBillableDefaults[taskCategory] = null;
            }
        }

        public AutoCompleteStringCollection DescriptionSource
        {
            get
            {
                return _names;
            }
        }

        public AutoCompleteStringCollection CategorySource(string Description)
        {
            AutoCompleteStringCollection outList;
            if (!string.IsNullOrEmpty(Description)
                && _nameCategories.TryGetValue(Description, out outList)
                )
                return outList;
            else
                return _categories;
        }

        internal bool? BillableFlagByDescription(string Description)
        {
            bool? billableFlagOut;
            if (!string.IsNullOrEmpty(Description)
                && _nameBillableDefaults.TryGetValue(Description, out billableFlagOut)
                )
                return billableFlagOut;
            else
                return null;
        }

        internal bool? BillableFlagByCategory(string Category)
        {
            bool? billableFlagOut;
            if (!string.IsNullOrEmpty(Category)
                && _categoryBillableDefaults.TryGetValue(Category, out billableFlagOut)
                )
                return billableFlagOut;
            else
                return null;
        }
    }
}