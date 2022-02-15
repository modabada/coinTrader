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
                    this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                    return true;
                }
                if(keyData.Equals(Keys.Tab)) {
                    this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                    return true;
                }
                if(keyData.Equals(Keys.Enter)) {
                    // start
                    return true;
                }
            }
            return false;
        }

        #region Windows Form 디자이너에서 생성한 코드
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        private void InitializeComponent() {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.background = new System.Windows.Forms.Panel();
            this.Pause = new System.Windows.Forms.Button();
            this.Start = new System.Windows.Forms.Button();
            this.SampleOutput = new System.Windows.Forms.TextBox();
            this.background.SuspendLayout();
            this.SuspendLayout();
            // 
            // background
            // 
            this.background.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.background.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.background.Controls.Add(this.SampleOutput);
            this.background.Controls.Add(this.Pause);
            this.background.Controls.Add(this.Start);
            this.background.ForeColor = System.Drawing.Color.Black;
            this.background.Location = new System.Drawing.Point(0, 0);
            this.background.Margin = new System.Windows.Forms.Padding(0);
            this.background.Name = "background";
            this.background.Padding = new System.Windows.Forms.Padding(3);
            this.background.Size = new System.Drawing.Size(784, 462);
            this.background.TabIndex = 2;
            // 
            // Pause
            // 
            this.Pause.BackColor = System.Drawing.Color.Black;
            this.Pause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Pause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Pause.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Pause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Pause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Pause.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Pause.ForeColor = System.Drawing.Color.DarkGray;
            this.Pause.Location = new System.Drawing.Point(379, 309);
            this.Pause.Margin = new System.Windows.Forms.Padding(0);
            this.Pause.Name = "Pause";
            this.Pause.Padding = new System.Windows.Forms.Padding(1);
            this.Pause.Size = new System.Drawing.Size(120, 40);
            this.Pause.TabIndex = 1;
            this.Pause.TabStop = false;
            this.Pause.Text = "Pause";
            this.Pause.UseVisualStyleBackColor = false;
            this.Pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // Start
            // 
            this.Start.BackColor = System.Drawing.Color.Black;
            this.Start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Start.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Start.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Start.ForeColor = System.Drawing.Color.DarkGray;
            this.Start.Location = new System.Drawing.Point(172, 309);
            this.Start.Margin = new System.Windows.Forms.Padding(0);
            this.Start.Name = "Start";
            this.Start.Padding = new System.Windows.Forms.Padding(1);
            this.Start.Size = new System.Drawing.Size(120, 40);
            this.Start.TabIndex = 0;
            this.Start.TabStop = false;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = false;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // SampleOutput
            // 
            this.SampleOutput.Location = new System.Drawing.Point(172, 46);
            this.SampleOutput.Multiline = true;
            this.SampleOutput.Name = "SampleOutput";
            this.SampleOutput.Size = new System.Drawing.Size(363, 186);
            this.SampleOutput.TabIndex = 2;
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
            this.background.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel background;
        private Button Start;
        private Button Pause;
        private TextBox SampleOutput;
    }
}

