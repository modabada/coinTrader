using System.Windows.Forms;

namespace coinTrader {
    partial class Main {
        /// 필수 디자이너 변수입니다.
        private System.ComponentModel.IContainer components = null;

        /// 사용 중인 모든 리소스를 정리합니다.
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // 단축키 설정
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if(!base.ProcessCmdKey(ref msg, keyData)) {
                if(keyData.Equals(Keys.Escape)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return true;
            }
        }

        #region Windows Form 디자이너에서 생성한 코드
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        private void InitializeComponent() {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.background = new System.Windows.Forms.Panel();
            this.Start = new System.Windows.Forms.Button();
            this.background.SuspendLayout();
            this.SuspendLayout();
            // 
            // background
            // 
            this.background.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.background.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.background.Controls.Add(this.Start);
            this.background.ForeColor = System.Drawing.Color.Black;
            this.background.Location = new System.Drawing.Point(0, 0);
            this.background.Margin = new System.Windows.Forms.Padding(0);
            this.background.Name = "background";
            this.background.Padding = new System.Windows.Forms.Padding(3);
            this.background.Size = new System.Drawing.Size(784, 462);
            this.background.TabIndex = 2;
            // 
            // Start
            // 
            this.Start.BackColor = System.Drawing.Color.Black;
            this.Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Start.ForeColor = System.Drawing.Color.DarkGray;
            this.Start.Location = new System.Drawing.Point(347, 206);
            this.Start.Margin = new System.Windows.Forms.Padding(0);
            this.Start.Name = "Start";
            this.Start.Padding = new System.Windows.Forms.Padding(1);
            this.Start.Size = new System.Drawing.Size(75, 23);
            this.Start.TabIndex = 0;
            this.Start.Text = "button1";
            this.Start.UseVisualStyleBackColor = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.background);
            this.Name = "Main";
            this.Text = "Form1";
            this.background.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel background;
        private Button Start;
    }
}

