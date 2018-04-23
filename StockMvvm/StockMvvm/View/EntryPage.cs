using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StockMvvm.View
{
    public class EntryPage : ContentPage
    {
        internal EntryPage(object sender, SelectedItemChangedEventArgs e)
        {
            var itemindex = ((Price)e.SelectedItem).Idindex;
            var itemname = ((Price)e.SelectedItem).Name;
            var itemstock = ((Price)e.SelectedItem).Stocks;
            var itemprice = ((Price)e.SelectedItem).Itemprice;

            // ナビゲーションバーの右上にボタンを追加する。
            //ToolbarItems.Add(new ToolbarItem("Name", "icon.png", async () =>
            //{
            //    // 何か処理を記述
            //}));

            //NavigationPage.SetHasNavigationBar(this, false);
            var ID = new Label { Text = itemindex.ToString(), BackgroundColor = Color.Gray, TextColor = Color.White };
            Entry usercode = new Entry { Text = itemname, BackgroundColor = Color.Gray, TextColor = Color.White, Keyboard = Keyboard.Default };
            Entry usercost = new Entry { Text = itemstock.ToString(), BackgroundColor= Color.Gray, TextColor = Color.White, Keyboard = Keyboard.Numeric };
            Entry usershares = new Entry { Text = itemprice.ToString(), BackgroundColor=Color.Gray, TextColor = Color.White, Keyboard = Keyboard.Numeric };


            Content = new StackLayout
            {
                Padding = 20,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                Children = { ID, usercode, usercost, usershares,

                }
            };




            ToolbarItems.Add(new ToolbarItem
            { // <-2
                Text = "Cansel", // <-3
                Command = new Command(() => Navigation.PopAsync())
            });

            //ToolbarItems.Add(new ToolbarItem
            //{
            //    Text = "Edit",
            //    Command = new Command(() => DisplayAlert("Selected", "Edit", "OK"))
            //});

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Save",
                Command = new Command(() => SetData(itemindex, usercode,usercost,usershares))
            });

        
        }



        public void SetData(int index, Entry code, Entry cost, Entry shares)
        {
            if (code.Text != "" && cost.Text != "" && shares.Text != "")
            {
                string SaveData = code.Text + "," + cost.Text + "," + shares.Text;
                ListViewPageViewModel.DataSave(index, SaveData);
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Entry Command Meseg", "未入力の項目があります。", "OK");
            }

        }

    }
}

