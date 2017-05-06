using Acr.UserDialogs;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using TestApp.Helpers;
using TestApp.Models;
using TestApp.Views;

using Xamarin.Forms;

namespace TestApp.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public IUserDialogs Dialogs { get; set; }

        private bool activityRunning { get; set; }

        public bool IsActivityRunning
        {
            get { return activityRunning; }
            set
            {
                if (activityRunning == value)
                    return;

                activityRunning = value;
                OnPropertyChanged("IsActivityRunning");
            }
        }

        private string listViewVisibleText { get; set; }
        public string ListViewVisibleText
        {
            get { return listViewVisibleText; }
            set
            {
                if (listViewVisibleText == value)
                    return;

                listViewVisibleText = value;
                OnPropertyChanged("ListViewVisibleText");
            }
        }



        private bool listViewLabelEmptyVisible { get; set; }

        public bool IsListViewLabelEmptyVisible
        {
            get
            {
                if (Items == null || Items.Count == 0)
                {
                    if (IsBusy)
                    {
                        ListViewVisibleText = "Loading...";
                    }
                    else
                    {

                        ListViewVisibleText = "No Items found";
                       

                    }
                    listViewLabelEmptyVisible = true;
                }
                else
                {
                    ListViewVisibleText = string.Empty;
                    listViewLabelEmptyVisible = false;
                }

                OnPropertyChanged("IsListViewLabelEmptyVisible");
                return listViewLabelEmptyVisible;
            }
        }

        private bool listViewVisible { get; set; }

        public bool IsListViewVisible
        {
            get
            {
                if (Items == null || Items.Count == 0)
                {
                    listViewVisible = false;
                }
                else
                {
                    listViewVisible = true;
                }

                OnPropertyChanged("IsListViewVisible");
                return listViewVisible;
            }
        }

        public ItemsViewModel(IUserDialogs dialogs)
        {
            Dialogs = dialogs;
            Title = "Browse";
            Items = new ObservableRangeCollection<Item>();

            IsActivityRunning = true;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Item;
                Items.Add(_item);
                await DataStore.AddItemAsync(_item);
            });

            IsActivityRunning = false;
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();

                await Task.Delay(5000);
                var items = await DataStore.GetItemsAsync(true);
                Items.ReplaceRange(items);

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}