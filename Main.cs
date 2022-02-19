using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace coinTrader {

    public partial class Main: Form {
        public Main() {
            FormBorderStyle = FormBorderStyle.Fixed3D;
            InitializeComponent();
            Thread trader = new Thread(new ThreadStart(new TradingInfo().Trading));
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
                string sEndPoint = "/info/balance";


                string sParams = "currency=REN";
                string sPostData = sParams;
                sPostData += "&endpoint=" + Uri.EscapeDataString(sEndPoint);
                long nNonce = MicroSecTime();

                string sHMAC_Key = (string) key.GetValue("Secret Key");
                string sHMAC_Data = sEndPoint + (char) 0 + sPostData + (char) 0 + nNonce.ToString();
                string sResult = Hash_HMAC(sHMAC_Key, sHMAC_Data);
                string sAPI_Sign = Convert.ToBase64String(StringToByte(sResult));
                byte[] rgbyData = Encoding.ASCII.GetBytes(sPostData);
                
                HttpClient client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
                client.BaseAddress = new Uri(page);
                client.DefaultRequestHeaders.Add("Api-Key", (string) key.GetValue("Connect Key"));
                client.DefaultRequestHeaders.Add("Api-Sign", sAPI_Sign);
                client.DefaultRequestHeaders.Add("Api-Nonce", nNonce.ToString());

                HttpResponseMessage response = await client.PostAsync(sEndPoint, new StringContent(sPostData, Encoding.UTF8, "application/x-www-form-urlencoded"));
                response.EnsureSuccessStatusCode();
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
            }
            catch(WebException webEx) {
                using(HttpWebResponse Response = (HttpWebResponse) webEx.Response) {
                    // nCode = Response.StatusCode;

                    using(StreamReader reader = new StreamReader(Response.GetResponseStream())) {
                        string body = reader.ReadToEnd();

                        SampleOutput.Text = "ERR\n" + body;
                    }
                }
            }

        }

        private void Pause_Click(object sender, EventArgs e) {

        }
    }
    public class TradingInfo {
        public bool? state = false;
        public void Trading() {
        }
        private string ByteToString(byte[] rgbyBuff) {
            string sHexStr = "";
            for(int nCnt = 0; nCnt < rgbyBuff.Length; nCnt++) {
                sHexStr += rgbyBuff[nCnt].ToString("x2"); // Hex format
            }
            return (sHexStr);
        }
        private byte[] StringToByte(string sStr) {
            byte[] rgbyBuff = Encoding.UTF8.GetBytes(sStr);
            return (rgbyBuff);
        }
        private long MicroSecTime() {
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
        private string Hash_HMAC(string sKey, string sData) {
            byte[] rgbyKey = Encoding.UTF8.GetBytes(sKey);
            using(var hmacsha512 = new HMACSHA512(rgbyKey)) {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(sData));
                return (ByteToString(hmacsha512.Hash));
            }
        }
    }
}
