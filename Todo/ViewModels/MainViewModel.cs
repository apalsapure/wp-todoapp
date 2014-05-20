//
//  MainViewModel.cs
//  Appacitive Quickstart
//
//  Copyright 2014 Appacitive, Inc.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//


using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Appacitive.Sdk;


namespace Todo
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<TodoItem>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<TodoItem> Items { get; private set; }

        private bool _isDataLoaded = false;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public bool IsDataLoaded
        {
            get
            {
                return _isDataLoaded;
            }
            set
            {
                if (value != _isDataLoaded)
                {
                    _isDataLoaded = value;
                    NotifyPropertyChanged("IsDataLoaded");
                }
            }
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData()
        {
            try
            {
                this.IsDataLoaded = false;
                //clear old entries
                this.Items.Clear();
                this.IsDataLoaded = false;

                ///get connected todo items for current user
                var result = await AppContext.UserContext.LoggedInUser.GetConnectedObjectsAsync("owner",
                                                                            orderBy: "__utcdatecreated",
                                                                            sortOrder: Appacitive.Sdk.SortOrder.Ascending);
                //iterate over result object and add todolist item to the list 
                while (true)
                {
                    result.ForEach(r => this.Items.Add(r as TodoItem));
                    //check if all pages are retrieved
                    if (result.IsLastPage) break;
                    //fetch next page
                    result = await result.NextPageAsync();
                }

                this.IsDataLoaded = true;
            }
            catch { }
        }

        public void Add(TodoItem todoItem)
        {
            var lists = this.Items.ToList();
            this.Items.Clear();
            lists.Add(todoItem);

            foreach (var list in lists.SortByDate())
                this.Items.Add(list);
        }
        public void Remove(TodoItem todoItem)
        {
            this.Items.Remove(todoItem);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}