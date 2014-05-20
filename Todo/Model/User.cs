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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Todo
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public User(string email, string password, string firstName, string lastName)
        {
            this.Username = email;
            this.Email = email;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public static string Authenticate(string email, string password)
        {
            var faileMessage = "Authentication failed";
            try
            {
                //Code to authenticate user on Appacitive will go here
                //For now get the user from store and check the credentials
                //remove this hard coded user
                var user = new User(email, password, "test user", email);

                return null;
            }
            catch { }
            return faileMessage;
        }

        public bool Save()
        {
            //Save user in the backend
            return true;
        }

        public bool Logout()
        {
            //Invalidate user token in Appacitive API
            return true;
        }
    }
}
