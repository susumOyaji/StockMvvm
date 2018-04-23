using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Globalization;

//using StockMvvm.ViewModels;



namespace StockMvvm
{

    public class Models : ContentPage
    {

        static void ModelS() { }//MainModel to end

        internal static int Incriment(int number)
        {
            return number + 1;
            //DateTime time = DateTime.Now;//new System.DateTime("yyyy", 1, 1, 0, 0, 0, 0);
        }














        /// <summary>
        /// Getserchi the specified code.
        /// </summary>
        /// <param name="code">Code.</param>
        public static async Task<Price> Getserchi(string code)
        {
            string Value = null;
            string ValueRatio = null;
            string PercentRatio = null;

            Price cityprice = new Price();

            string url = "http://stocks.finance.yahoo.co.jp/stocks/detail/?code=" + code;// +".T";

            var httpClient = new HttpClient();
            string str = await httpClient.GetStringAsync(url);

            string searchWord = "stoksPrice";    //検索する文字列 ="stoksPrice"> 
            int foundIndex = str.IndexOf(searchWord);//始めの位置を探す
                                                     //次の検索開始位置
            int nextIndex = foundIndex + searchWord.Length;
            try
            {
                //次の位置を探す
                foundIndex = str.IndexOf(searchWord, nextIndex);
                if (foundIndex != -1)
                {
                    int i = searchWord.Length + 2;//pricedata to point
                    for (; Convert.ToString(str[foundIndex + i]) != "<"; i++)
                    {
                        Value = Value + str[foundIndex + i];//current value 現在値
                    }
                }
                else
                {
                    cityprice.Realprice = 0;// "Error";
                }

                string searchWord1 = "yjMSt"; //検索する文字列前日比
                int foundIndex1 = str.IndexOf(searchWord1);//始めの位置を探す
                int i1 = searchWord1.Length + 2;

                for (; Convert.ToString(str[foundIndex1 + i1]) != "（"; i1++)
                {
                    ValueRatio = ValueRatio + str[foundIndex1 + i1];//previous 前日比? ¥
                }

                if (Convert.ToString(str[foundIndex1 + i1 + 1]) == "-")//(－)下落
                {
                    cityprice.Polar = "Green";
                }
                else
                {
                    cityprice.Polar = "Red";
                }


                i1++;
                for (; Convert.ToString(str[foundIndex1 + i1]) != "）"; i1++)
                {
                    PercentRatio = PercentRatio + str[foundIndex1 + i1];//previous 前日比? %
                }
                cityprice.Name = code;
                cityprice.Realprice = Convert.ToDecimal(Value);//現在値
                cityprice.Prev_day = ValueRatio;//前日比±
                cityprice.Percent = PercentRatio; //前日比％


            }
            catch (Exception)
            {
                cityprice.Percent = "Close"; //前日比％
                cityprice.Polar = "Gray";

            }
            return cityprice;// polarity;
        }










        ///
        /// <summary>
        /// Pasonals the getserchi.
        /// </summary>
        /// <returns>The getserchi.</returns>
        public static async Task<List<Price>> PasonalGetserchi()//企業名を設定して現在値を取得する
        {
            string Value = null;
            string YenRatio = null;
            string PercentRatio = null;
            int index = 0;

            // UTF8のファイルの読み込み Edit.        
            string responce = await StorageControl.PCLLoadCommand();//登録データ読み込み

            List<Price> prices = Finance.Parse(responce);

            foreach (Price price in prices)
            {
                string url = "http://stocks.finance.yahoo.co.jp/stocks/detail/?code=" + price.Name;// +".T";

                var httpClient = new HttpClient();
                string str = await httpClient.GetStringAsync(url);

                string searchWord = "stoksPrice";    //検索する文字列 ="stoksPrice"> 
                int foundIndex = str.IndexOf(searchWord);//始めの位置を探す
                                                         //次の検索開始位置
                int nextIndex = foundIndex + searchWord.Length;
                try
                {
                    //次の位置を探す
                    foundIndex = str.IndexOf(searchWord, nextIndex);
                    if (foundIndex != -1)
                    {
                        int i = searchWord.Length + 2;//pricedata to point
                        for (; Convert.ToString(str[foundIndex + i]) != "<"; i++)
                        {
                            Value = Value + str[foundIndex + i];//current value 現在値
                        }
                    }
                    else
                    {
                        //price[0] = "Error";
                    }

                    string searchWord1 = "yjMSt"; //検索する文字列前日比
                    int foundIndex1 = str.IndexOf(searchWord1);//始めの位置を探す
                    int i1 = searchWord1.Length + 2;

                    for (; Convert.ToString(str[foundIndex1 + i1]) != "（"; i1++)
                    {
                        YenRatio = YenRatio + str[foundIndex1 + i1];//previous 前日比? ¥
                    }

                    if (Convert.ToString(str[foundIndex1 + i1 + 1]) == "-")//(－)下落
                    {
                        price.Polar = "Green";//(-)
                    }
                    else
                    {
                        price.Polar = "Red";//(+)
                    }


                    i1++;
                    for (; Convert.ToString(str[foundIndex1 + i1]) != "）"; i1++)
                    {
                        PercentRatio = PercentRatio + str[foundIndex1 + i1];//previous 前日比? %
                    }


                    if (Value == "---")
                    {
                        price.Realprice = 000;
                    }
                    else
                    {
                        price.Realprice = Convert.ToDecimal(Value);//現在値
                    }
                    price.Prev_day = YenRatio;//前日比±
                    price.Percent = PercentRatio; //前日比％
                    price.PayAssetprice = price.Stocks * price.Itemprice;//株数*購入単価
                    price.Gain = (price.Realprice - price.Itemprice) * price.Stocks;//損益
                    price.RealValue = (price.Stocks * price.Realprice);//個別利益


                    Value = "";
                    YenRatio = "";
                    PercentRatio = "";
                    index = index + 1;
                }
                catch (Exception)
                {
                    price.Prev_day = "Close";
                    price.Polar = "Gray";
                }
            }

            return prices;//polarity;

        }//class to end 






        /// <summary>
        /// Ons the label clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async static void DataStorage(int ID, string sender)
        {

            //List<string> listdata = new List<string>();
            var listdata = await StorageControl.PCLLoadCommand();//登録データ読み込み

            // カンマ区切りで分割して配列に格納する
            string[] stArrayData = listdata.Split('\n');
            stArrayData[ID] = sender;

            //// 配列内のデータをすべてカンマ区切りで連結する
            string CsvData = string.Join("\n", stArrayData);
                     
            // UTF8のファイルの書き込み Edit. 
            string write = await StorageControl.PCLSaveCommand(CsvData);//登録データ書き込み
          



            //string Usercost = usercost.Text;
            //string Usershares = usershares.Text;
            //string[] cols;
            ////string[] savedata;
            //string[] entrydata = new string[3];
            //string[] mergedArray = entrydata;//連結データ
            //string[] returncode = new string[] { "\n" };


            //    string response = await saveLoadCS.DataLoadAsync();//.Replace("\r", "").Split('\n');
            //    string[] rows = response.Split('\r');
            //    cols = rows;
            //    entrydata[0] = Usercode;//usercode.Text = "Code or Cost or Usershares failed";
            //    entrydata[1] = Usercost;
            //    entrydata[2] = Usershares;

            //    if (response != "")
            //    {
            //        foreach (string row in rows)
            //        {
            //            if (string.IsNullOrEmpty(row)) continue;
            //            cols = row.Split(',');
            //        }
            //        //Concatで連結して、ToArrayで配列にする
            //        mergedArray = cols.Concat(entrydata).ToArray();
            //    }

            //}
            //// 配列内のデータをすべてカンマ区切りで連結する
            //string CsvData = string.Join(",", mergedArray);
            //await saveLoadCS.DataSaveAsync(CsvData + "\n");
            //await DisplayAlert("SAVE", " is InputData", "OK");
            ////InsertRange メソッド

        }


        public async static void DataStorageAdd(string[] Adddata )
        {

            //// カンマ区切りで分割して配列に格納する
            //string[] stArrayData = listdata.Split('\n');
            ////stArrayData[ID] = sender;

            ////// 配列内のデータをすべてカンマ区切りで連結する
            string CsvData = string.Join("\n", Adddata);

            //// UTF8のファイルの書き込み Edit. 
            string write = await StorageControl.PCLSaveCommand(CsvData);//登録データ書き込み

        }



        /// <summary>
        /// Ons the label clicked.
        /// </summary>
        /// <param name="ID"></param>
        public async static void Deleet(int ID )
        {

            //List<string> listdata = new List<string>();
            var listdata = await StorageControl.PCLLoadCommand();//登録データ読み込み

            // カンマ区切りで分割して配列に格納する
            string[] stArrayData = listdata.Split('\n');
            stArrayData[ID] = null;//削除行をnullで消す

            int i = 0;


            foreach (string row in stArrayData)
            {
                if (string.IsNullOrEmpty(row) != true)//null行を読み飛ばす
                {
                    stArrayData[i] = row;
                    i++;
                }
            }
            stArrayData[i] = "null";//最下行を消去する

            //要素数5の配列
            //int[] intArray = { 0, 1, 2, 3, 4 };
            //要素数を10に増やす
            Array.Resize(ref stArrayData, i);




            //// 配列内のデータをすべて\n区切りで連結する
            string CsvData = string.Join("\n", stArrayData);

            // UTF8のファイルの書き込み Edit. 
            string write = await StorageControl.PCLSaveCommand(CsvData);//登録データ書き込み


        }







        #region UpPersonaldata
        public static async Task UpPersonaldata(string regdata)
        {
            int index = 0;
            int i = 0;
            Decimal PayAssetprice = 0;
            Decimal TotalAssetprice = 0;
            string NameMulti = "";
            string TrgetWord = null, TrgetWord1 = null, TrgetWord2 = null;

            // UTF8のファイルの読み込み Edit.        
            string responce = regdata;// await mainViewModel.PCLLoadCommand();//登録データ読み込み         

            /*List<Price>*/
            //var resPrice = await PasonalGetserchi();//登録件数設定
            ////////////////////////////////////////////////////////////////////
            List<Price> prices = Finance.Parse(responce);

            foreach (Price price in prices)
            {
                NameMulti = NameMulti + price.Name + ",";
            }

            string url = "https://info.finance.yahoo.co.jp/search/?query=" + NameMulti;//6758%2C9837%2C6976%2C%2C%2C%2C"
            var httpClient = new HttpClient();
            string str = await httpClient.GetStringAsync(url);

            foreach (Price price in prices)
            {
                string searchWord = "slk:stock_price;pos:" + Convert.ToSingle(prices.Count);//最初の検索する企業 
                int foundIndex = str.IndexOf(searchWord);   //始めの位置を探す

                int nextIndex = foundIndex + searchWord.Length; //現在値の検索開始位置
                try
                {
                    //次の位置を探す
                    string pricesearchWord = "price yjXXL";
                    foundIndex = str.IndexOf(pricesearchWord, nextIndex);
                    if (foundIndex != -1)
                    {
                        i = foundIndex + pricesearchWord.Length + 2; //pricedata to point 14209
                        for (; Convert.ToString(str[i]) != "<"; i++)
                        {
                            TrgetWord = TrgetWord + str[i]; //current value 現在値
                        }
                    }
                    else
                    {
                        //price[0] = "Error";
                    }

                    string searchWord1 = "strong greenFin"; //検索する文字列前日比
                    int foundIndex1 = str.IndexOf(searchWord1);//始めの位置を探す
                    int i1 = searchWord1.Length + 2;

                    for (; Convert.ToString(str[foundIndex1 + i1]) != "<"; i1++)
                    {
                        TrgetWord1 = TrgetWord1 + str[foundIndex1 + i1];//previous 前日比? ¥
                    }



                    if (Convert.ToString(str[foundIndex1 + i1 + 1]) == "-")//(－)下落
                    {
                        price.Polar = "-";
                    }
                    else
                    {
                        price.Polar = "+";
                    }


                    i1++;

                    string searchWord2 = "strong greenFin"; //検索する文字列前日比
                    int foundIndex2 = str.IndexOf(searchWord2, foundIndex1 + searchWord2.Length);//始めの位置を探す
                    int i2 = searchWord2.Length + 2;

                    for (; Convert.ToString(str[foundIndex2 + i2]) != "<"; i2++)
                    {
                        TrgetWord2 = TrgetWord2 + str[foundIndex2 + i2];//previous 前日比? %
                    }
                    prices[index].Name = NameMulti;//Code Namber
                    prices[index].Stocks = 123;
                    prices[index].Itemprice = 1111;
                    prices[index].Realprice = Convert.ToDecimal(TrgetWord);//現在値
                    prices[index].RealValue = 1212;
                    prices[index].Percent = "%";
                    prices[index].Prev_day = TrgetWord1;//前日比±
                    prices[index].Percent = TrgetWord2; //前日比％

                    TrgetWord = "";
                    TrgetWord1 = "";
                    TrgetWord2 = "";
                    ///
                    //Pasonalresponce[index] = price.Name + ","+ Convert.ToString(price.Stocks) + "," + Convert.ToString(price.Itemprice);
                    index = index + 1;
                }
                catch (Exception e)
                {
                    //var accepted = await DisplayAlert(e.Message, "市場が開始していません。", "Ok", "Cancel");
                    MessagingCenter.Send(e, "progress_dialog", true);
                    //if (accepted == true)
                    //{   
                    //    Application.Current.MainPage = new WebSerchi();
                    //    //Navigation.InsertPageBefore(new WebSerchi(), this);
                    //    //buttonDialog1.Text = "Accepted!";
                    //    //break;
                    //}
                }

                /// }
                // return prices;//polarity;



                /////////////////////////////////////////////////////////////
                ///  foreach (Price price in prices)
                ///  {
                price.PayAssetprice = PayAssetprice + Convert.ToDecimal(price.Stocks) * Convert.ToDecimal(price.Itemprice);//保有数*購入価格=投資総額

                price.Investmen = (price.PayAssetprice);//投資総額

                if (price.Realprice.ToString() != "---")
                {
                    TotalAssetprice = TotalAssetprice + Convert.ToDecimal(price.Stocks) * Convert.ToDecimal(price.Realprice);//保有数*時価=時価総額
                    price.TotalAsset = Convert.ToDecimal(TotalAssetprice);// Convert.ToString(TotalAssetprice);//時価総額
                }
                else
                {
                    price.TotalAsset = 0;
                }

                if (price.Polar != "-")
                {
                    //UptoButtons[i].BackgroundColor = Color.Red;//各銘柄
                }
                else
                {
                    //UptoButtons[i].BackgroundColor = Color.Green;
                }

                //AddGoingprice[i].Text = price.Realprice;//時価
                i = i + 1;
                index = index + 1;
            }

            //price.RealValue = Convert.ToInt64(TotalAssetprice - PayAssetprice);

            //price.TotalAsset = String.Format("{0:#,0}", Convert.ToInt64(TotalAssetprice - PayAssetprice));//利益総額

            //Decimal pasent = Convert.ToInt16((TotalAssetprice / PayAssetprice) * 100);
            //string pasenttext = Convert.ToString(pasent) + "%";

            //if (PayAssetprice <= TotalAssetprice)//
            //{
            //    //UptoAsset.BackgroundColor = Color.Red;//総合銘柄
            //}
            //else
            //{
            //    //UptoAsset.BackgroundColor = Color.Green;
            //}

            //return price;
        }

        #endregion
    }
}

