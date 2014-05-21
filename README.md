# Windows Phone Todo App

A simple todo app backed by [Appacitive Cloud Platform](http://www.appacitive.com) and uses [Appacitive .Net SDK](https://github.com/appacitive/appacitive-dotnet-sdk) for managing application data. 

This app demonstrates ***Data Store*** and ***Users*** features provided by the Appacitive .Net SDK.

### Getting Started

To execute this app you will require <a target="_blank" href="http://www.visualstudio.com/">Visual Studio 2012</a>, <a target="_blank" href="https://dev.windowsphone.com/en-us/downloadsdk">Windows Phone SDK</a> and <a target="_blank" href="https://portal.appacitive.com/">Appacitive Account</a>.

If you don't have an Appacitive Account, [sign up](https://portal.appacitive.com/signup.html) for a free account today.

##### Step 1: Modeling the Backend
To model your application backend on Appacitive Platform, please watch [this](http://devcenter.appacitive.com/windows/samples/todo/#model-backend) video. If you already have the backend ready with you; you can jump to the next step.

##### Step 2: Download Source Code
You can download the source code for this sample Todo App from https://github.com/apalsapure/wp-todoapp/archive/master.zip.

##### Step 3: Authenticating App
Once you have the source code, launch the solution and open the App.xaml.cs and inside `Application_Launching` replace the `{{App Id}}` and `{{API Key}}` by your Appacitive Application Id and API Key.

```c#
// Code to execute when the application is launching (eg, from Start)
private void Application_Launching(object sender, LaunchingEventArgs e)
{
  	//Initializing Appacitive .Net SDK
    Appacitive.Sdk.AppContext.Initialize("{{App Id}}", 
                                         "{{API Key}}", 
                                         Appacitive.Sdk.Environment.Sandbox);
}
```

To get these details, open your app on [Appacitive Portal](https://portal.appacitive.com). API key for the app is available on your app's home page at the bottom. To get the App Id, open application details view by clicking on edit icon near your app's name.

Once your done, run the application.

### Build your own Todo App 

If you want to build your own Todo App, please visit http://devcenter.appacitive.com/windows/samples/todo/. This tutorial will explain each and every step required to build this sample app.
