/*
Ashley Thibodeau
Interface Programming
Code Exercise 04

GitHub Link: https://github.com/InterfaceProgramming/ce4-ThibodeauAshley-FS

 */
using System;
using Xamarin.Forms;
using System.IO;
using Xamarin.Forms.Xaml;

namespace Thibodeau_Ashley_CE04
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }

        public App()
        {
            InitializeComponent();
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
