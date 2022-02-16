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
            Thread trader = new Thread(new ThreadStart(TradingInfo.Trading));
        }
        private void Start_Click(object sender, EventArgs e) {
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


                string sParams = "currency=ALL";
                // string sParams = "currency=ALL";
                string sPostData = sParams;
                sPostData += "&endpoint=" + Uri.EscapeDataString(sEndPoint);
                long nNonce = MicroSecTime();

                string sHMAC_Key = (string) key.GetValue("Secret Key");
                string sHMAC_Data = sEndPoint + (char) 0 + sPostData + (char) 0 + nNonce.ToString();
                string sResult = Hash_HMAC(sHMAC_Key, sHMAC_Data);
                string sAPI_Sign = Convert.ToBase64String(StringToByte(sResult));

                // HttpWebRequest 방식으로 private, 안쓰고싶음
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(page + sEndPoint);
                byte[] rgbyData = Encoding.ASCII.GetBytes(sPostData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = rgbyData.Length;

                request.Headers.Add("Api-Key", (string)key.GetValue("Connect Key"));
                request.Headers.Add("Api-Sign", sAPI_Sign);
                request.Headers.Add("Api-Nonce", nNonce.ToString());

                using(var stream = request.GetRequestStream()) {
                    stream.Write(rgbyData, 0, rgbyData.Length);
                }

                var Response = (HttpWebResponse) request.GetResponse();

                string body = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                this.SampleOutput.Text = body;

                // HttpClient 방식으로 private, (Auth data 문제)
                /*
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(page);
                client.DefaultRequestHeaders.Clear();
                ProductHeaderValue header = new ProductHeaderValue("MyAwesomeLibrary", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                // ProductInfoHeaderValue userAgent = new ProductInfoHeaderValue(header);
                // client.DefaultRequestHeaders.UserAgent.Add(userAgent);
                // client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.87 Safari/537.36");
                Dictionary<string, string> values = new Dictionary<string, string> {
                    {"Api-Key", (string) key.GetValue("Connect Key") },
                    {"Api-Sign", sAPI_Sign},
                    {"Api-Nonce", nNonce.ToString() }
                };
                FormUrlEncodedContent data = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync(sEndPoint, data);
                // response.EnsureSuccessStatusCode();
                Console.WriteLine(key.GetValue("Connect Key"));
                Console.WriteLine(key.GetValue("Secret Key"));
                Console.WriteLine(response.StatusCode);
                SampleOutput.Text = await response.Content.ReadAsStringAsync();
                */




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
    public static class TradingInfo {
        public static bool? state = false;
        public static void Trading() {
        }
    }
}
