﻿/*
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


            MessagingCenter.Subscribe<EventDetails>(this, "EditItemMessage", (sender) =>
            {
                editEvent = sender;
                eventEntry.Text = sender.EventName;
                datePicker.Date = sender.Date;
                timePicker.Time = sender.Time;
            });
        }

        private string AssignIMG()
        {
            var dateSelected = datePicker.Date;

            string wDay = dateSelected.DayOfWeek.ToString();
            string imgFileName = null;


            switch (wDay)
            {
                case "Monday":
                    imgFileName = "monday.png";
                    break;
                case "Tuesday":
                    imgFileName = "tuesday.png";
                    break;
                case "Wednesday":
                    imgFileName = "wednesday.png";
                    break;
                case "Thursday":
                    imgFileName = "thursday.png";
                    break;
                case "Friday":
                    imgFileName = "friday.png";
                    break;
                case "Saturday":
                    imgFileName = "saturday.png";
                    break;
                 case "Sunday":
                    imgFileName = "sunday.png";
                    break;

            }

            return imgFileName;

        }

        async private void BtnDelete_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete Event", "Are you sure you would like to delete this Event?", "Yes", "No");
            Debug.WriteLine("Popup Answer: " + answer);

            if (answer)
            {
                if (editEvent != null)
                {
                    if (File.Exists(editEvent.Filename))
                    {
                        File.Delete(editEvent.Filename);
                    }
                }
                MessagingCenter.Send<String>("Delete", "ModifiedMessage");
                await Navigation.PopAsync();
            }
        }

        async private void BtnSave_Clicked(object sender, EventArgs e)
        {
            string filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.CE04.txt");

            string messageType = "New";
            var eventName = eventEntry.Text;
            var eventDate = datePicker.Date;

            string strDate = $"{ eventDate.Date:d} {timePicker.Time:t}";
            string date = eventDate.Date.ToString("d");
            string time = timePicker.Time.ToString();
            var dayIMG = AssignIMG();
            bool pass = true;

            if (editEvent != null)
            {
                bool answer = await DisplayAlert("Confirm Recent Changes", "You are about to make changes to your saved information. Would you like to continue?", "Yes", "No");
                Debug.WriteLine("Popup Save Answer: " + answer);
                if (answer)
                {
                    //Save
                    filename = editEvent.Filename;
                    messageType = "Edit";
                    File.WriteAllText(filename, $"{eventName},{strDate},{date},{time},{dayIMG}");
                    MessagingCenter.Send<String>(messageType, "ModifiedMessage");
                    await Navigation.PopAsync();
                }
                else
                {
                    pass = false;
                }

            }

            if(pass)
            {

                File.WriteAllText(filename, $"{eventName},{strDate},{date},{time},{dayIMG}");
                MessagingCenter.Send<String>(messageType, "ModifiedMessage");
                await Navigation.PopAsync();

            }


        }


    }
}
