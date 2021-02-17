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
            dt.SetBinding(ImageCell.ImageSourceProperty, "DayIMG");
            dt.SetBinding(ImageCell.TextProperty, "DisplayDate");
            dt.SetBinding(ImageCell.DetailProperty, "EventName");
            listView.ItemTemplate = dt;

            listView.ItemSelected += ListView_ItemSelected;
            btnAdd.Clicked += BtnAdd_Clicked;
            btnDeleteAll.Clicked += BtnDeleteAll_Clicked;

            MessagingCenter.Subscribe<String>(this, "ModifiedMessage", (sender) =>
            {
                this.ReloadListData();
            });

            this.ReloadListData();

        }

        private void BtnDeleteAll_Clicked(object sender, EventArgs e)
        {
            var files = Directory.EnumerateFiles(App.FolderPath, "*.CE04.txt");

            foreach(var filename in files)
            {
                File.Delete(filename);
            }

            ReloadListData();

        }

        

        private void ReloadListData()
        {
            eventList.Clear();
            DeleteAll_Button();


            var files = Directory.EnumerateFiles(App.FolderPath,"*.CE04.txt");
            foreach( var filename in files)
            {
                
                string strData = File.ReadAllText(filename);
                string[] data = strData.Split(',');

                
                if(data.Length == 5)
                {
                    string name = data[0];
                    string date = data[1];
                    DateTime.TryParse(data[2], out DateTime dateTime);
                    TimeSpan.TryParse(data[3], out TimeSpan timeSpan);
                    string data_dIMG = data[4];

                    eventList.Add(new EventDetails
                    {
                        Filename = filename,
                        EventName = name,
                        DisplayDate = date,
                        Date = dateTime,
                        Time = timeSpan,
                        DayIMG = data_dIMG,
                    });
                }
                else
                {
                    File.Delete(filename);
                }

            }


            listView.ItemsSource = eventList.OrderBy(d => d.Date).ToList();

            DeleteAll_Button();

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


        private void DeleteAll_Button()
        {
            if (eventList.Count >= 1)
            {
                btnDeleteAll.IsVisible = true;
            }
            else if (eventList.Count < 1)
            {
                btnDeleteAll.IsVisible = false;
            }
        }

    }
}
