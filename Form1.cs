using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coinTrader {
    public partial class Form1: Form {
        public Form1() {
            InitializeComponent();
        }
        private void btn1(object sender, EventArgs e) {
            label1.Text = "test suc";
            Console.WriteLine("true");
            btnClickThis.Text = "test suc2";
        }

        private void btnClickThis_MouseClick(object sender, MouseEventArgs e) {

        }
    }
}
