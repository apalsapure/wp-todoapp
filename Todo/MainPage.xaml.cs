//
//  MainPage.xaml.cs
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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using Appacitive.Sdk;

namespace Todo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool _isFirstTime = true;
        #region Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;
        }
        #endregion

        // Load data for the ViewModel Items
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_isFirstTime)
            {
                _isFirstTime = false;
                progress.Visibility = System.Windows.Visibility.Visible;
                if (await User.IsLoggedIn())
                {
                    gTodoList.Visibility = System.Windows.Visibility.Visible;
                    ShowList();
                    return;
                }
                gSingIn.Visibility = System.Windows.Visibility.Visible;
            }
            //hide progress bar
            progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        #region Event Handlers
        //user is signing in
        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //disable sign in button
            btnSignIn.IsEnabled = false;
            progress.Visibility = System.Windows.Visibility.Visible;

            //Logic to Authenticate User will go here
            //Once user is authenticated, show todo list
            var result = await User.Authenticate(txtEmail.Text, txtPassword.Password);
            if (string.IsNullOrEmpty(result) == false)
            {
                MessageBox.Show("Oops please check your email address and password", "Sign in Failed", MessageBoxButton.OK);
                progress.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                ShowList();
            }

            //enable sign in
            btnSignIn.IsEnabled = true;
        }

        //User is signing up
        private async void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            //disable signup button
            btnSignUp.IsEnabled = false;
            //show progress bar
            progress.Visibility = System.Windows.Visibility.Visible;

            //create a user object and persist locally
            var split = txtRName.Text.Split(' ');
            string lastName = string.Empty;
            if (split.Length > 1) lastName = string.Join(" ", split.Skip(1));

            //save user
            var user = new User(txtREmail.Text, txtRPassword.Password, split[0], lastName);
            if (await user.Save() == false)
            {
                MessageBox.Show("Oops some thing went wrong, check your network connection.", "Sign up failed", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Login with your email address and password.", "Sign up successful", MessageBoxButton.OK);
                txtEmail.Text = txtREmail.Text;
                txtREmail.Text = txtRName.Text = txtRPassword.Password = string.Empty;
                lnkSignIn_Click();
            }

            //hide progress bar
            progress.Visibility = System.Windows.Visibility.Collapsed;
            //enable signup button
            btnSignUp.IsEnabled = true;
        }

        //user clicked signup link
        private void lnkSignUp_Click(object sender, RoutedEventArgs e)
        {
            gSingIn.Visibility = System.Windows.Visibility.Collapsed;
            gSingUp.Visibility = System.Windows.Visibility.Visible;
        }

        //user clicked signin link
        private void lnkSignIn_Click(object sender = null, RoutedEventArgs e = null)
        {
            gSingIn.Visibility = System.Windows.Visibility.Visible;
            gSingUp.Visibility = System.Windows.Visibility.Collapsed;
        }

        #region Event Handlers
        //check box clicked
        private async void checkBox_Click(object sender, RoutedEventArgs e)
        {
            progress.Visibility = System.Windows.Visibility.Visible;
            var selectedItem = ((System.Windows.Controls.Primitives.ToggleButton)(sender));
            var selectedListItem = App.ViewModel.Items.Where(i => i.Id == selectedItem.CommandParameter.ToString()).FirstOrDefault();
            if (selectedListItem == null) return;
            selectedListItem.IsDone = selectedItem.IsChecked == true;
            if (await selectedListItem.Save() == false)
                MessageBox.Show("Failed to save the state of item.", "Operation failed", MessageBoxButton.OK);
            progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        //app bar add button clicked
        private void appBarAdd_Click(object sender, EventArgs e)
        {
            gTodoList.Visibility = System.Windows.Visibility.Collapsed;
            gAddList.Visibility = System.Windows.Visibility.Visible;

            txtListItemName.Focus();
            ApplicationBar.Buttons.RemoveAt(0);
            ApplicationBarIconButton appBarSaveButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/save.png", UriKind.Relative));
            appBarSaveButton.Text = "save";
            appBarSaveButton.Click += appBarSave_Click;
            ApplicationBar.Buttons.Add(appBarSaveButton);

            ApplicationBarIconButton appBarCancelButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/cancel.png", UriKind.Relative));
            appBarCancelButton.Text = "cancel";
            appBarCancelButton.Click += appBarCancel_Click;
            ApplicationBar.Buttons.Add(appBarCancelButton);
        }

        //save button clicked
        private async void appBarSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtListItemName.Text) == false)
            {
                progress.Visibility = System.Windows.Visibility.Visible;
                (sender as ApplicationBarIconButton).IsEnabled = false;
                try
                {
                    //save the list item
                    var todoItem = new TodoItem { Name = txtListItemName.Text.Trim() };
                    await todoItem.Save();

                    //clear the text field
                    txtListItemName.Text = string.Empty;

                    //update the local list
                    App.ViewModel.Add(todoItem);
                    App.ViewModel.Items.SortByDate();
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to save the todo item", "Operation failed", MessageBoxButton.OK);
                }
                progress.Visibility = System.Windows.Visibility.Collapsed;
                (sender as ApplicationBarIconButton).IsEnabled = true;
            }

            Cancel();
        }

        //cancel button clicked
        private void appBarCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        //context menu item clicked
        private async void menuItem_Click(object sender, RoutedEventArgs e)
        {
            progress.Visibility = System.Windows.Visibility.Visible;

            //get the list item
            var todoItem = App.ViewModel.Items.Where(i => i.Id.Equals(((Microsoft.Phone.Controls.MenuItem)(sender)).CommandParameter)).FirstOrDefault();
            if (todoItem != null)
            {
                //delete the list item
                if (await todoItem.Delete() == false)
                    MessageBox.Show("Failed to delete the list item.", "Operation failed", MessageBoxButton.OK);
                else
                    App.ViewModel.Items.Remove(todoItem);
            }

            progress.Visibility = System.Windows.Visibility.Collapsed;
        }

        //delete all clicked from the menu
        private void menuDeleteAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("All the items will be deleted forever.\n\nAre you sure?", "Warning !!!", MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK != result) return;

            progress.Visibility = System.Windows.Visibility.Visible;

            foreach (var item in App.ViewModel.Items)
            {
                //delete individual items
                item.Delete();
            }
            progress.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.GoBack();
        }

        //delete all clicked from the menu
        private void menuRefresh_Click(object sender, EventArgs e)
        {
            App.ViewModel.LoadData();
        }        

        //log out user
        private async void menuLogout_Click(object sender, EventArgs e)
        {
            try
            {
                //log out user
                await AppContext.LogoutAsync();
            }
            catch { }
            gAddList.Visibility = System.Windows.Visibility.Collapsed;
            gTodoList.Visibility = System.Windows.Visibility.Collapsed;
            gSingUp.Visibility = System.Windows.Visibility.Collapsed;
            gSingIn.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        #endregion

        #region Private Helper Function
        private void Cancel()
        {
            gTodoList.Visibility = System.Windows.Visibility.Visible;
            gAddList.Visibility = System.Windows.Visibility.Collapsed;

            lock (DataContext)
            {
                ApplicationBar.Buttons.RemoveAt(0);
                ApplicationBar.Buttons.RemoveAt(0);
            }

            ApplicationBarIconButton appBarAddButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            appBarAddButton.Text = "cancel";
            appBarAddButton.Click += appBarAdd_Click;
            ApplicationBar.Buttons.Add(appBarAddButton);
        }
        private void ShowList()
        {
            //hide remaining views
            gSingIn.Visibility = System.Windows.Visibility.Collapsed;
            gSingUp.Visibility = System.Windows.Visibility.Collapsed;

            //show todo list view
            gTodoList.Visibility = System.Windows.Visibility.Visible;

            //load todo items
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            //show the application bar
            ApplicationBar.IsVisible = true;
            progress.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion


    }
}