using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MySqlConnector;

namespace Shussekibo
{
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// リスト用園児情報(EnjiInfo)の定義
        /// </summary>
        public IList<EnjiInfo> Enjiinfos { get; private set; }

        /// <summary>
        /// MainPage
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);  //when use Navigation Page and タイトルバー非表示
            I_FirstTime();                                    //when use Navigation Page 

        }

        /// <summary>
        /// 最初はログイン
        /// </summary>
        void I_FirstTime()
        {
            //Navigationページをモーダレス表示する場合、非同期(async)でないとダメなのだ
            //await Navigation.PushAsync(new Login());  //when use Navigation Page

            Navigation.PushModalAsync(new Login());  //when use Navigation Page



            //モーダルにしたのに、ログインする前に下の行を実行してしまう
            //（ログインしていないんで、EnNameIDが不明な状態）！！どうする最初の本日の分？

            //await Navigation.PushModalAsync(new Login());
            //最初は今日の分
            //モーダレスだと、本日分の展開がログイン前なのでモーダルに変更
            //Enjiinfos = MySQL.GetEnjiListByDate(dtpicker.Date);
            //lstenji.ItemsSource = Enjiinfos;
        }


        //だれかさんが言ってた：遷移先からPopAsyncで戻ってきたときにイベント発生しないし。
        //https://qiita.com/smbkrysk14/items/48fb73a247091772ab66
        //これ、MVVM式なんで、難しすぎてNG

        public void CallBack(string task)
        {
            Enjiinfos = MySQL.GetEnjiListByDate(dtpicker.Date);
            lstenji.ItemsSource = Enjiinfos;
        }








        /// <summary>
        /// 日付選択イベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void DateSelected(object sender, DateChangedEventArgs e)
        {
            await DisplayAlert("Alert", "DB_Check.", "OK");

            Enjiinfos = MySQL.GetEnjiListByDate(dtpicker.Date);
            lstenji.ItemsSource = Enjiinfos;
        }

        /// <summary>
        /// 園児選択 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //EnjiInfo selectedItem = e.SelectedItem as EnjiInfo;

            Navigation.PushModalAsync(new Detail());  //when use Navigation Page


        }

        /// <summary>
        /// 園児タップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            //EnjiInfo tappedItem = e.Item as EnjiInfo;
        }
    }
}
