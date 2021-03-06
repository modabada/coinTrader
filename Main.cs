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
        public Thread trad_Thread;
        public Main() {
            FormBorderStyle = FormBorderStyle.Fixed3D;
            InitializeComponent();
            trad_Thread = new Thread(new ThreadStart(Trading));
            trad_Thread.Start();
        }
        private void Start_Click(object sender, EventArgs e) {
        }
        private void Pause_Click(object sender, EventArgs e) {

        }

        #region Trade
        /// <summary>
        /// Bithumb API 통신 모음
        /// </summary>
        private bool state = true;
        private static readonly JObject key = JObject.Parse(File.ReadAllText("/Users/inwoo/Desktop/Coding/coinTrader/key.json"));
        private readonly string ConnectionKey = (string) key["Connect Key"];
        private readonly string SecretKey = (string) key["Secret Key"];
        private readonly HttpClient client = new HttpClient {
            BaseAddress = new Uri("https://api.bithumb.com")
        };
        private void Trading() {
            bool? isIncresing = null;
            double targetPrice = double.MaxValue;
            double k = 0.5;
            while(state) {
                try {
                    DateTime time = DateTime.Now;
                    JObject ticker = Pub_Connection("public/ticker/REN_KRW");
                    double currencyPrice = (double) Pub_Connection("public/orderbook/REN_KRW?count=1")["data"]["bids"][0]["price"];
                    JObject balance = Sec_Connection("/info/balance", "currency=REN");
                    if(time.Hour < 9) {
                        isIncresing = null;
                    }
                    else if(time.Hour <= 21) {
                        if(isIncresing == null) {
                            isIncresing = false;
                            //target = 시작가 + (어제의 변동폭 * k)
                            targetPrice = (double) ticker["data"]["opening_price"]
                                + ((double) ticker["data"]["max_price"] - (double) ticker["data"]["min_price"])
                                * k;
                        }

                        if(isIncresing == false && currencyPrice > targetPrice) {
                            isIncresing = true;
                            double krw = (double) balance["data"]["total_krw"];
                            float cnt = (float) Math.Floor(krw / currencyPrice * 1000) / 1000;
                            if(cnt == 0) {
                                throw new Exception("잔고이슈");
                            }
                            //구매
                            SampleOutput.AppendText(string.Format("목표가: {0}구매가: {1} 개수: {2}\n", targetPrice, currencyPrice, cnt));
                            ResponseOutput.Text = Sec_Connection(
                                "/trade/market_buy",
                                string.Format("units={0}&order_currency=REN&payment_currency=KRW", cnt))
                                .ToString();
                        }
                    }
                    else {
                        if(isIncresing != null) {
                            float cnt = (float) Math.Floor((double) balance["data"]["total_ren"] * 1000) / 1000;
                            if(isIncresing == true && cnt != 0) {
                                SampleOutput.AppendText(string.Format("판매가: {0} 개수: {1}\n", currencyPrice, cnt));
                                ResponseOutput.Text = Sec_Connection(
                                    "/trade/market_sell",
                                    string.Format("units={0}&order_currency=REN&payment_currency=KRW", cnt))
                                    .ToString();
                            }
                            else {
                                // 코인 없음 -> 금일 매수진행 안함
                                SampleOutput.AppendText(string.Format("금일 매수진행 안함, 목표가: {0}\n", targetPrice));
                            }
                            isIncresing = null;
                        }
                    }
                    Thread.Sleep(1000 * 2);
                }
                catch(Exception e) {
                    StringBuilder output = new StringBuilder();
                    for(int i = 0; i < 5; i++) {
                        output.Append(" -> ");
                        output.Append(e.Message);
                        e = e.GetBaseException();
                    }
                    SampleOutput.AppendText(output.ToString());
                    Thread.Sleep(2 * 1000 * 60);
                }
            }
        }

        private JObject Pub_Connection(string path) {
            /// ex) Pub_Connection("public/ticker/REN_KRW")
            return JObject.Parse(client.GetStringAsync(path).Result);
        }
        private JObject Sec_Connection(string End, string Params) {
            /// ex) Sec_Connection("/info/balance", "currency=REN");
            Params += "&endpoint=" + Uri.EscapeDataString(End);
            long Nonce = MicroSecTime();
            string HMAC_Data = End + (char) 0 + Params + (char) 0 + Nonce.ToString();
            string Result = Hash_HMAC(SecretKey, HMAC_Data);
            string API_Sign = Convert.ToBase64String(Encoding.UTF8.GetBytes(Result));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Api-Key", ConnectionKey);
            client.DefaultRequestHeaders.Add("Api-Sign", API_Sign);
            client.DefaultRequestHeaders.Add("Api-Nonce", Nonce.ToString());

            HttpResponseMessage Response = client.PostAsync(End, new StringContent(Params, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            Response.EnsureSuccessStatusCode();
            return JObject.Parse(Response.Content.ReadAsStringAsync().Result);
        }

        private string ByteToString(byte[] rgbyBuff) {
            string sHexStr = "";
            foreach(byte b in rgbyBuff) {
                sHexStr += b.ToString("x2");
            }
            return (sHexStr);
        }
        private long MicroSecTime() {
            DateTime DateTimeNow = DateTime.UtcNow;
            long nEpochTicks = new DateTime(1970, 1, 1).Ticks;
            long nNowTicks = DateTimeNow.Ticks;
            long nowMiliseconds = DateTimeNow.Millisecond;
            long nUnixTimeStamp = (nNowTicks - nEpochTicks) / TimeSpan.TicksPerSecond;
            string sNonce = nUnixTimeStamp.ToString() + nowMiliseconds.ToString("D03");
            return (Convert.ToInt64(sNonce));
        }
        private string Hash_HMAC(string sKey, string sData) {
            byte[] rgbyKey = Encoding.UTF8.GetBytes(sKey);
            using(var hmacsha512 = new HMACSHA512(rgbyKey)) {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(sData));
                return (ByteToString(hmacsha512.Hash));
            }
        }
        #endregion
    }
}
