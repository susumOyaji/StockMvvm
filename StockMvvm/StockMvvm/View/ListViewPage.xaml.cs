using System;
using StockMvvm.View;
using Xamarin.Forms;

namespace StockMvvm
{
    public partial class ListViewPage : ContentPage
    {
        // internal ListViewPageViewModel Vm { get; set; }


       
        public ListViewPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);//1st.Pageのバーを消す

        }



        public void OnEdit(object sender, EventArgs e)
        {

            var newPage = new ContentPage();
            Navigation.PushAsync(newPage);

            var poppedPage = Navigation.PopAsync();


            var usercode = new Entry { Placeholder = "Code", Keyboard = Keyboard.Text, };



            //mi.CommandParameter as ContactHistoryItem

            MenuItem mi = ((MenuItem)sender);
            var par = mi.CommandParameter;
            var selectedText = this.DisplayActionSheet("Edit", "Close", "Chancel", new string[] { "qqqq", "すいか", "ぶどう" });

            if (selectedText != null)
            {
                //buttonDialog2.Text = selectedText;
            }

            DisplayAlert("Edit Context Action", e.ToString() + " edit context action", "OK");

            //ListViewPageViewModel.OnLabelClicked(mi);


        }


        internal void CanselCommand(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new EntryPage(sender, e));
            var mi = ((MenuItem)sender);

            //var itemindex = mi.CommandParameter;
            var itemindex = ((Price)mi.CommandParameter).Idindex;
            DisplayAlert("CanselCOmmand Context Action", itemindex + " cansel context action", "OK");
        }



        internal void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            var mi = ((MenuItem)sender);
            var option = mi.Text;

            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            DisplayAlert("Item Selected", e.SelectedItem.ToString(), "Ok");
            //((ListViewPage)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.

        }


        /// <summary>
        /// ListViewの項目選択時に呼ばれる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new EntryPage(sender, e));
      
        }


        /// <summary>
        /// 項目タップ時に呼ばれる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //this.Navigation.PushAsync(new EntryPage(e));
            //Navigation.PopAsync();
            //DisplayAlert("Item Tapped", item.ToString(), "Ok");

        }

    }

}
