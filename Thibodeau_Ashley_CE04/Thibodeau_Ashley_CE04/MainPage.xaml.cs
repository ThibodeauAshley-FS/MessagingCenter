/*
Ashley Thibodeau
Interface Programming
Code Exercise 04

GitHub Link: https://github.com/InterfaceProgramming/ce4-ThibodeauAshley-FS

 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thibodeau_Ashley_CE04.Models;
using System.IO;
using Xamarin.Forms;

namespace Thibodeau_Ashley_CE04
{
    public partial class MainPage : ContentPage
    {
        private List<EventDetails> eventList = new List<EventDetails>();

        public MainPage()
        {
            InitializeComponent();

            DataTemplate dt = new DataTemplate(typeof(ImageCell));

            dt.SetBinding(ImageCell.ImageSourceProperty, "DatIMG");
            dt.SetBinding(ImageCell.TextProperty, "____");
            dt.SetBinding(ImageCell.DetailProperty, "EventTitle");
            listView.ItemTemplate = dt;

            listView.ItemSelected += ListView_ItemSelected;
            btnAdd.Clicked += BtnAdd_Clicked;

            MessagingCenter.Subscribe<String>(this, "ModifiedMessage", (sender) =>
            {
                this.ReloadListData();
            });
        }

        private void ReloadListData()
        {
            eventList.Clear();

            

            var files = Directory.EnumerateFiles(App.FolderPath,"*.CE04.txt");
            foreach( var filename in files)
            {
                string strData = File.ReadAllText(filename);
                string[] data = strData.Split('|');

                string data_Name = data[0];
                DateTime.TryParse(data[1], out DateTime data_Date);
                TimeSpan.TryParse(data[2], out TimeSpan data_Time);
                string data_wDay = data[4];

                eventList.Add(new EventDetails
                {
                    Filename = filename,
                    EventTitle = data_Name,
                    Date = data_Date,
                    Time = data_Time,
                    DayIMG = data_wDay,
                    CreationDate = File.GetCreationTime(filename)
                });
            }
           
        }

        async private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EventCreationPage
            {
                BindingContext = new EventDetails()
            }); ;

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Navigation.PushAsync(new EventCreationPage());
                MessagingCenter.Send<EventDetails>((EventDetails)e.SelectedItem, "EditItemMessage");
            }

        }
    }
}
