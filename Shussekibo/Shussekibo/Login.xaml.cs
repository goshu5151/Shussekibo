using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shussekibo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);  //when use Navigation Page and タイトルバー非表示
        }
        //void OnLoginClicked(object sender, EventArgs e)       //モーダルの場合非同期(async)である必要は無い
        void OnLoginClicked(object sender, EventArgs e)
        {
            short sErr = 0;
            if (UserID.Text != null)
            {
                if (PassWD.Text != null)
                {
                    if (MySQL.GetEnNameID(UserID.Text, PassWD.Text) == false)
                    {
                        sErr = 3;   //そんな奴はいない
                    }
                }
                else
                {
                    sErr = 2;   //password 未入力
                }
            }
            else
            {
                sErr = 1;   //userid 未入力
            }

            //判定
            switch (sErr)
            {
                case 0:
                    // ログイン画面終了
                    //await Navigation.PopAsync();
                    //Application.Current.MainPage = new MainPage();    //when MainPage = new Login();
                    Navigation.PopModalAsync();

                    break;
                case 1:
                    DisplayAlert("ログインエラー", "UserIDを入力してください。", "OK");
                    break;
                case 2:
                    DisplayAlert("ログインエラー", "Passwordを入力してください。", "OK");
                    break;
                case 3:
                    DisplayAlert("ログインエラー", "UserID又はPasswordに誤りがあります。", "OK");
                    break;
            }
        }
    }
}