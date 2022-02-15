using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace coinTrader {

    public partial class Main: Form {
        public Main() {
            FormBorderStyle = FormBorderStyle.Fixed3D;
            InitializeComponent();
            Thread trader = new Thread(new ThreadStart(TradingInfo.Trading));
        }
        private async void Start_Click(object sender, EventArgs e) {
            JObject key = JObject.Parse(File.ReadAllText("/Users/inwoo/Desktop/Coding/coinTrader/Key.json"));
            try {
                // 메소드 선언
                string ByteToString(byte[] rgbyBuff) {
                    string sHexStr = "";
                    for(int nCnt = 0; nCnt < rgbyBuff.Length; nCnt++) {
                        sHexStr += rgbyBuff[nCnt].ToString("x2"); // Hex format
                    }
                    return (sHexStr);
                }
                byte[] StringToByte(string sStr) {
                    byte[] rgbyBuff = Encoding.UTF8.GetBytes(sStr);
                    return (rgbyBuff);
                }
                long MicroSecTime() {
                    long nEpochTicks = 0;
                    long nUnixTimeStamp = 0;
                    long nNowTicks = 0;
                    long nowMiliseconds = 0;
                    string sNonce = "";
                    DateTime DateTimeNow;
                    nEpochTicks = new DateTime(1970, 1, 1).Ticks;
                    DateTimeNow = DateTime.UtcNow;
                    nNowTicks = DateTimeNow.Ticks;
                    nowMiliseconds = DateTimeNow.Millisecond;
                    nUnixTimeStamp = ((nNowTicks - nEpochTicks) / TimeSpan.TicksPerSecond);
                    sNonce = nUnixTimeStamp.ToString() + nowMiliseconds.ToString("D03");
                    return (Convert.ToInt64(sNonce));
                }
                string Hash_HMAC(string sKey, string sData) {
                    byte[] rgbyKey = Encoding.UTF8.GetBytes(sKey);
                    using(var hmacsha512 = new HMACSHA512(rgbyKey)) {
                        hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(sData));
                        return (ByteToString(hmacsha512.Hash));
                    }
                }


                string page = "https://api.bithumb.com";
                string sEndPoint = "/info/account";

                string sParams = "order_currency=BTC&payment_currency=KRW";
                // string sParams = "currency=ALL";
                string sPostData = sParams + "&endpoint=" + Uri.EscapeDataString(sEndPoint);

                long nNonce = MicroSecTime();


                HttpClient client = new HttpClient();
                string sHMAC_Key = (string)( key.GetValue("Secret Key"));
                string sHMAC_Data = sEndPoint + (char) 0 + sPostData + (char) 0 + nNonce.ToString();
                string sResult = Hash_HMAC(sHMAC_Key, sHMAC_Data);
                string Api_sign = Convert.ToBase64String(StringToByte(sResult));

                Dictionary<string, string> values = new Dictionary<string, string> {
                    {"Api-Key", (string)key.GetValue("Connect Key") },
                    {"Api-Sign", Api_sign},
                    {"currency", nNonce.ToString() }
                };
                FormUrlEncodedContent data = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync(page, data);
                // response.EnsureSuccessStatusCode();

                SampleOutput.Text = await response.Content.ReadAsStringAsync();




                // public
                /*
                string page = "https://api.bithumb.com/public/ticker/BTC";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(page);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                SampleOutput.Text = responseBody;
                */

                //   HttpClient client = new HttpClient();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString());
            }

        }

        private void Pause_Click(object sender, EventArgs e) {

        }
    }
    public static class TradingInfo {
        public static bool? state = false;
        public static void Trading() {
        }
    }
}
