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
    public partial class Main: Form {
        public Main() {
            FormBorderStyle = FormBorderStyle.Fixed3D;
            InitializeComponent();
        }
        private void btn1(object sender, EventArgs e) {
            Console.WriteLine("true");
        }

        private void btnClickThis_MouseClick(object sender, MouseEventArgs e) {

        }
    }
}
