/*
Ashley Thibodeau
Interface Programming
Code Exercise 04

GitHub Link: https://github.com/InterfaceProgramming/ce4-ThibodeauAshley-FS

 */
using System;
using Thibodeau_Ashley_CE04.Models;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Thibodeau_Ashley_CE04
{
    public partial class EventCreationPage : ContentPage
    {
        EventDetails editEvent;
        public EventCreationPage()
        {
            InitializeComponent();

            btnSave.ImageSource = ImageSource.FromFile("save48.png");
            btnDelete.ImageSource = ImageSource.FromFile("delete48.png");


            btnSave.Clicked += BtnSave_Clicked;
            btnDelete.Clicked += BtnDelete_Clicked;
            datePicker.DateSelected += DatePicker_DateSelected;

            MessagingCenter.Subscribe<EventDetails>(this, "EditItemMessage", (sender) =>
            {
                editEvent = sender;
                eventEntry.Text = sender.EventTitle;

            });
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            var dateSelected = datePicker.Date;

            string wDay = dateSelected.DayOfWeek.ToString();

            switch(wDay)
            {
                case "":

                    break;
            }

        }

        async private void BtnDelete_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete Event", "Are you sure you would like to delete this Event?", "Yes", "No");

            Debug.WriteLine("Popup Answer: " + answer);

            if(answer)
            {
                if(editEvent != null)
                {
                    if(File.Exists(editEvent.Filename))
                    {
                        File.Delete(editEvent.Filename);
                    }
                }
            }
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            string filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.CE04.txt");
            string messageType = "New";

            if(editEvent != null)
            {
                //Save
                filename = editEvent.Filename;
                messageType = "Edit";
            }
            File.WriteAllText(filename,$"{eventEntry.Text},{datePicker.Date},{TimePicker.TimeProperty}");
            MessagingCenter.Send<String>(messageType, "ModifiedMessage");
            Navigation.PopAsync();
        }
    }
}
