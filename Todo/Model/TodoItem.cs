//
//  TodoItem.cs
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

using Appacitive.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo
{
    public class TodoItem : Appacitive.Sdk.APObject
    {
        public TodoItem()
            : base("todo")
        {

        }

        //special constructor called by SDK
        public TodoItem(Appacitive.Sdk.APObject existing)
            : base(existing)
        { }

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                return this.Get<string>("title");
            }
            set
            {
                this.Set<string>("title", value);
            }
        }

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public bool IsDone
        {
            get
            {
                return this.Get<bool>("completed");
            }
            set
            {
                this.Set<bool>("completed", value);
            }
        }

        public async Task<bool> Save()
        {
            try
            {
                //If Id is empty, it means to create the todo item
                //else update
                if (string.IsNullOrEmpty(this.Id))
                {
                    //as we need to store this item in context of user
                    //we will create a connection between todo item and the user
                    //when connection is saved, todo item is automatically created
                    await Appacitive.Sdk.APConnection
                                    .New("owner")
                                    .FromExistingObject("user", AppContext.UserContext.LoggedInUser.Id)
                                    .ToNewObject("todo", this)
                                    .SaveAsync();
                }
                else
                {
                    //update the state
                    await this.SaveAsync();
                }
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Delete()
        {
            //delete list item from backend
            try
            {
                await Appacitive.Sdk.APObjects.DeleteAsync(this.Type, this.Id, true);
                return true;
            }
            catch { return false; }
        }

        public DateTime CreatedDate { get { return this.CreatedAt; } }
    }
}
