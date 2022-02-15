using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
        private void Start_Click(object sender, EventArgs e) {
            Console.WriteLine("start");
            Console.WriteLine(sender);
            Console.WriteLine(e);
            JObject key = JObject.Parse(File.ReadAllText("/Users/inwoo/Desktop/Coding/coinTrader/Key.json"));
            Console.WriteLine(key["Secret Key"]);
            Console.WriteLine(key.GetValue("Connect Key"));
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://api.bithumb.com/info/balance");
            request.Method = "POST";
            request.Headers.Add("apiKey", (string)key.GetValue("Connect Key"));
            request.Headers.Add("secretKey", (string)key.GetValue("Secret Key"));
            request.Headers.Add("currency", "ALL");
            request.ContentType = "application/json";


            SampleOutput.Text = "";
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
