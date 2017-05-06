using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.ViewModels;

using Xamarin.Forms;

namespace TestApp.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();            
            BindingContext = viewModel = new ItemsViewModel(UserDialogs.Instance);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            viewModel.Dialogs.ShowLoading("Loading...");

            await Task.Delay(2000);
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item, UserDialogs.Instance)));

            viewModel.Dialogs.HideLoading();

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }
        
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            
                viewModel.LoadItemsCommand.Execute(null);
        }        
    }
}
