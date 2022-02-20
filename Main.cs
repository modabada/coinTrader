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
            // SampleOutput.Text = new Trade().Trading();
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
        private void Pause_Click(object sender, EventArgs e) {

        }

        #region Trade
        /// <summary>
        /// Bithumb API 통신 모음
        /// </summary>
        private bool state = true;
        private static readonly JObject key = JObject.Parse(File.ReadAllText("/Users/inwoo/Desktop/Coding/coinTrader/Key.json"));
        private readonly string ConnectionKey = (string) key.GetValue("Connection Key");
        private readonly string SecretKey = (string) key.GetValue("Secret Key");
        private HttpClient client = new HttpClient {
            BaseAddress = new Uri("https://api.bithumb.com")
        };
        private void Trading() {
            while(state) {
                DateTime time = DateTime.Now;
                if(time.Hour == 9) {

                }
                // SampleOutput.Text = Sec_Connection("/info/balance", "currency=REN").ToString();
                SampleOutput.Text = Pub_Connection("public/ticker/REN_KRW").ToString();
                Thread.Sleep(1000);
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
            StringBuilder sb = new StringBuilder();
            foreach(byte b in rgbyBuff) {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
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
            ByteToString(new HMACSHA512(Encoding.UTF8.GetBytes(sKey)).ComputeHash(Encoding.UTF8.GetBytes(sData)));
            byte[] rgbyKey = Encoding.UTF8.GetBytes(sKey);
            using(var hmacsha512 = new HMACSHA512(rgbyKey)) {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(sData));
                return (ByteToString(hmacsha512.Hash));
            }
        }
        #endregion
    }
}
