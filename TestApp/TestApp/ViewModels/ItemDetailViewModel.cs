using Acr.UserDialogs;
using TestApp.Models;

namespace TestApp.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }

        public IUserDialogs Dialogs { get; set; }
        public ItemDetailViewModel(Item item, IUserDialogs dialogs)
        {
            Dialogs = dialogs;
            Title = item.Text;
            Item = item;
            dialogs.HideLoading();
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
    }
}