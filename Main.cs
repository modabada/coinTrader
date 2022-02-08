using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace coinTrader {
    public static class TradingInfo {
        public static bool? state = false;
        public static void Trading() {
        }
    }
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
        }

        private void btnClickThis_MouseClick(object sender, MouseEventArgs e) {

        }

        private void Pause_Click(object sender, EventArgs e) {

        }
    }
}
