namespace Gurobitest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackNumBox = new System.Windows.Forms.TextBox();
            this.sequenceBox = new System.Windows.Forms.RichTextBox();
            this.lenBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.timeBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dynamicShow = new System.Windows.Forms.Button();
            this.returnRoute = new System.Windows.Forms.Button();
            this.routePanel = new System.Windows.Forms.Panel();
            this.routeChartControl = new DevExpress.XtraCharts.ChartControl();
            this.showPointTimer = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.routePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.routeChartControl)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(26, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "轨迹数量：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(26, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 31);
            this.label2.TabIndex = 1;
            this.label2.Text = "碾压顺序：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(406, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 31);
            this.label3.TabIndex = 2;
            this.label3.Text = "总路径长：";
            // 
            // trackNumBox
            // 
            this.trackNumBox.Location = new System.Drawing.Point(180, 28);
            this.trackNumBox.Name = "trackNumBox";
            this.trackNumBox.Size = new System.Drawing.Size(179, 25);
            this.trackNumBox.TabIndex = 3;
            // 
            // sequenceBox
            // 
            this.sequenceBox.Location = new System.Drawing.Point(180, 59);
            this.sequenceBox.Name = "sequenceBox";
            this.sequenceBox.Size = new System.Drawing.Size(179, 54);
            this.sequenceBox.TabIndex = 4;
            this.sequenceBox.Text = "";
            // 
            // lenBox
            // 
            this.lenBox.Location = new System.Drawing.Point(530, 32);
            this.lenBox.Name = "lenBox";
            this.lenBox.Size = new System.Drawing.Size(179, 25);
            this.lenBox.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(785, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 36);
            this.button1.TabIndex = 6;
            this.button1.Text = "开始优化";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(406, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 31);
            this.label4.TabIndex = 7;
            this.label4.Text = "运行时长：";
            // 
            // timeBox
            // 
            this.timeBox.Location = new System.Drawing.Point(530, 87);
            this.timeBox.Name = "timeBox";
            this.timeBox.Size = new System.Drawing.Size(179, 25);
            this.timeBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(715, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 31);
            this.label5.TabIndex = 9;
            this.label5.Text = "ms";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(715, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 31);
            this.label6.TabIndex = 10;
            this.label6.Text = "米";
            // 
            // dynamicShow
            // 
            this.dynamicShow.AutoSize = true;
            this.dynamicShow.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dynamicShow.Location = new System.Drawing.Point(944, 24);
            this.dynamicShow.Name = "dynamicShow";
            this.dynamicShow.Size = new System.Drawing.Size(116, 36);
            this.dynamicShow.TabIndex = 11;
            this.dynamicShow.Text = "动态展示";
            this.dynamicShow.UseVisualStyleBackColor = true;
            this.dynamicShow.Click += new System.EventHandler(this.dynamicShow_Click);
            // 
            // returnRoute
            // 
            this.returnRoute.AutoSize = true;
            this.returnRoute.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.returnRoute.Location = new System.Drawing.Point(785, 81);
            this.returnRoute.Name = "returnRoute";
            this.returnRoute.Size = new System.Drawing.Size(116, 36);
            this.returnRoute.TabIndex = 12;
            this.returnRoute.Text = "生成路径";
            this.returnRoute.UseVisualStyleBackColor = true;
            this.returnRoute.Click += new System.EventHandler(this.returnRoute_Click);
            // 
            // routePanel
            // 
            this.routePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.routePanel.Controls.Add(this.routeChartControl);
            this.routePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.routePanel.Location = new System.Drawing.Point(0, 148);
            this.routePanel.Name = "routePanel";
            this.routePanel.Size = new System.Drawing.Size(1455, 446);
            this.routePanel.TabIndex = 13;
            // 
            // routeChartControl
            // 
            this.routeChartControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.routeChartControl.Legend.Name = "Default Legend";
            this.routeChartControl.Location = new System.Drawing.Point(0, 0);
            this.routeChartControl.Name = "routeChartControl";
            this.routeChartControl.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.routeChartControl.Size = new System.Drawing.Size(1453, 444);
            this.routeChartControl.TabIndex = 0;
            // 
            // showPointTimer
            // 
            this.showPointTimer.Interval = 10;
            this.showPointTimer.Tick += new System.EventHandler(this.showPointTimer_Tick);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(944, 81);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 36);
            this.button2.TabIndex = 14;
            this.button2.Text = "静态展示";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(1091, 24);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 36);
            this.button3.TabIndex = 15;
            this.button3.Text = "关闭";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.AutoSize = true;
            this.button4.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.Location = new System.Drawing.Point(1091, 81);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(164, 36);
            this.button4.TabIndex = 16;
            this.button4.Text = "传统路径展示";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1301, 22);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(125, 96);
            this.richTextBox1.TabIndex = 17;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1455, 594);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.routePanel);
            this.Controls.Add(this.returnRoute);
            this.Controls.Add(this.dynamicShow);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.timeBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lenBox);
            this.Controls.Add(this.sequenceBox);
            this.Controls.Add(this.trackNumBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.routePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.routeChartControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox trackNumBox;
        private System.Windows.Forms.RichTextBox sequenceBox;
        private System.Windows.Forms.TextBox lenBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox timeBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button dynamicShow;
        private System.Windows.Forms.Button returnRoute;
        private System.Windows.Forms.Panel routePanel;
        private DevExpress.XtraCharts.ChartControl routeChartControl;
        private System.Windows.Forms.Timer showPointTimer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

