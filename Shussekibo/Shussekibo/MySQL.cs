using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace Shussekibo
{
    //モジュールのまとめ方がわからない
    //データベースIOは纏めたい
    class MySQL
    {
        /// <summary>
        /// EnNameID / Global
        /// </summary>
        public static int XX_iEnNameID = 0;

        /// <summary>
        /// コネクションストリングを得る
        /// </summary>
        /// <returns></returns>
        public static MySqlConnectionStringBuilder GetCS()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "XXX",
                Port = 3306,
                Database = "XXX",
                UserID = "XXX",
                Password = "XXX",
                CharacterSet = "utf8",
            };

            return builder;
        }

        /// <summary>
        /// 指定した UserID, Password から XX_iEnNameID を設定する
        /// </summary>
        /// <param name="strID">UserID</param>
        /// <param name="strPass">Password</param>
        /// <returns>bRet:true=成功, false=失敗</returns>
        public static bool GetEnNameID(String strID, string strPass)
        {
            //return init
            bool bRet = false;
            XX_iEnNameID = 0;

            MySqlConnectionStringBuilder builder = GetCS();
            try
            {
                MySqlConnection cn;
                cn = new MySqlConnection(builder.ConnectionString);
                MySqlCommand cmd = cn.CreateCommand();
                string pw = "";

                cn.Open();
                cmd.CommandText = "select ifnull(convert(aes_decrypt(unhex(PasswordSSB),'KINGoo') using utf8), PasswordSSB) as pw" +
                            " from UserTable" +
                            " where UserID = " + strID;
                pw = Convert.ToString(cmd.ExecuteScalar());
                
                if(pw == strPass)       //パスワード一致!
                {
                    cmd.CommandText = "select EnNameID from EnNameTable" +
                                    " where (LicenseID, EnCD) in (" +
                                        "select LicenseID, EnCD from UserTable" +
                                        " where UserID = " + strID +
                                        ")";
                    //成功したので、Globalに展開
                    XX_iEnNameID = Convert.ToInt32(cmd.ExecuteScalar());
                    if (XX_iEnNameID > 0)
                    {
                        bRet = true;
                    }
                }

                cn.Close();
            }
            catch
            {
                bRet = false;
            }
            return bRet;
        }

        /// <summary>
        /// 指定した日付の園児リストを作成して返す
        /// </summary>
        /// <param name="dtParam"></param>
        /// <returns></returns>
        public static IList<EnjiInfo> GetEnjiListByDate(DateTime dtParam)
        {
            IList<EnjiInfo> Enjiinfos;
            Enjiinfos = new List<EnjiInfo>();

            MySqlConnectionStringBuilder builder = MySQL.GetCS();
            try
            {
                MySqlConnection cn;
                ////cn = new MySqlConnection(_connectioninfo);
                ////cn = new MySqlConnection("Server=ksinfo.biz;Port=3306;User Id=kyouik;Password=ohaa+tbo;Database=kyouik_other;Character Set=utf8;");
                ////cn = new MySqlConnection("server=ksinfo.biz;user=kyouik;database=kyouik_other;port=3306;password=ohaa+tbo");
                //cn = new MySqlConnection("server=localhost;user=kyouik;database=kyouik;port=3306;password=ohaa+tbo");

                cn = new MySqlConnection(builder.ConnectionString);
                MySqlCommand cmd = cn.CreateCommand();
                MySqlDataReader reader;

                cn.Open();
                cmd.CommandText = "select UserTable.UserID as UID," +
                                " convert(aes_decrypt(unhex(UserName),'KINGoo'), char) as UName," +
                                " case when ChkStat_01 = 1 then '欠席'" +
                                     " when ChkStat_02 = 1 then '遅刻'" +
                                     " when ChkStat_03 = 1 then '早迎'" +
                                     " else 'その他'" +
                                " end as Stat" +
                                " from SSBDataTable, UserTable" +
                                " where TargetDate = '" + dtParam.ToString("yyyy/MM/dd") + "'" +
                                " and SSBDataTable.UserID = UserTable.UserID" +
                                " and SSBDataTable.UserID in (" +
                                    "select UserID from UserTable, EnNameTable" +
                                    " where UserTable.LicenseID = EnNameTable.LicenseID" +
                                    " and UserTable.EnCD = EnNameTable.EnCD" +
                                    " and EnNameID = " + Convert.ToString(XX_iEnNameID) +
                                " )" +
                                " order by SSBDataID desc";
                using (reader = cmd.ExecuteReader())        //なんで、reader.close が書けないんだろう・・
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("UID={0} Name={1} Stat={2}", reader[0], reader[1], reader[2]);

                        Enjiinfos.Add(new EnjiInfo
                        {
                            iID = Convert.ToInt32(reader[0]),
                            Name = Convert.ToString(reader[1]),
                            Stat = Convert.ToString(reader[2]),
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/13/Gelada-Pavian.jpg/320px-Gelada-Pavian.jpg"
                        });
                    }
                }
                cn.Close();
            }
            catch
            {

            }
            return Enjiinfos;
        }
    }
}
