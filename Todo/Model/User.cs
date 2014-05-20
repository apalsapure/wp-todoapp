//
//  User.cs
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Todo
{
    public class User : Appacitive.Sdk.APUser
    {
        //special constructors called by SDK
        public User() : base() { }

        public User(Appacitive.Sdk.APObject existing)
            : base(existing)
        { }

        public User(string email, string password, string firstName, string lastName)
        {
            this.Username = email;
            this.Email = email;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public async static Task<string> Authenticate(string email, string password)
        {
            var faileMessage = "Authentication failed";
            try
            {
                //authenticate user on Appacitive
                var credentials = new UsernamePasswordCredentials(email, password)
                {
                    TimeoutInSeconds = int.MaxValue,
                    MaxAttempts = int.MaxValue
                };

                await Appacitive.Sdk.AppContext.LoginAsync(credentials);

                return null;
            }
            catch { }
            return faileMessage;
        }

        public static async Task<bool> IsLoggedIn()
        {
            try
            {
                var user = await Appacitive.Sdk.APUsers.GetLoggedInUserAsync();
                if (user == null) return false;
                return true;
            }
            catch (AppacitiveApiException) { return false; }
        }

        public async Task<bool> Save()
        {
            try
            {
                //Save user in the backend
                await this.SaveAsync();
                return true;
            }
            catch { return false; }
        }

        public bool Logout()
        {
            //Logout user
            Appacitive.Sdk.AppContext.LogoutAsync();
            return true;
        }
    }
}
