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
        public void LoadData()
        {
            //clear old entries
            this.Items.Clear();
            this.IsDataLoaded = false;

            //dummy data for demo purpose,
            //replace this code with the code which makes call to Appacitive platform
            this.Items.Add(new TodoItem { Id = "1", Name = "Runtime list one", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "2", Name = "Runtime list two", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "3", Name = "Runtime list three", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "4", Name = "Runtime list four", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "5", Name = "Runtime list five", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "6", Name = "Runtime list six", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "7", Name = "Runtime list seven", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "8", Name = "Runtime list eight", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "9", Name = "Runtime list nine", CreatedDate = DateTime.Now });
            this.Items.Add(new TodoItem { Id = "10", Name = "Runtime list ten", CreatedDate = DateTime.Now });

            this.IsDataLoaded = true;
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