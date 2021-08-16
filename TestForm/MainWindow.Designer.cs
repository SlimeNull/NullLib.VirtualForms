
namespace TestForm
{
    partial class MainWindow
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
            this.moveInChild2 = new System.Windows.Forms.Button();
            this.moveInChild = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.TextBox();
            this.showOption = new System.Windows.Forms.Button();
            this.virtualFormHost1 = new NullLib.VirtualForms.Win.VirtualControlHost();
            this.showTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // moveInChild2
            // 
            this.moveInChild2.Location = new System.Drawing.Point(12, 415);
            this.moveInChild2.Name = "moveInChild2";
            this.moveInChild2.Size = new System.Drawing.Size(80, 23);
            this.moveInChild2.TabIndex = 1;
            this.moveInChild2.Text = "Move InChild";
            this.moveInChild2.UseVisualStyleBackColor = true;
            this.moveInChild2.Click += new System.EventHandler(this.button1_Click);
            // 
            // moveInChild
            // 
            this.moveInChild.Location = new System.Drawing.Point(98, 415);
            this.moveInChild.Name = "moveInChild";
            this.moveInChild.Size = new System.Drawing.Size(81, 23);
            this.moveInChild.TabIndex = 2;
            this.moveInChild.Text = "Move InChild";
            this.moveInChild.UseVisualStyleBackColor = true;
            this.moveInChild.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 399);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "InChild AbsPosition";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 386);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mouse Position";
            // 
            // logBox
            // 
            this.logBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.logBox.Location = new System.Drawing.Point(618, 0);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(182, 450);
            this.logBox.TabIndex = 5;
            // 
            // showOption
            // 
            this.showOption.Location = new System.Drawing.Point(185, 415);
            this.showOption.Name = "showOption";
            this.showOption.Size = new System.Drawing.Size(83, 23);
            this.showOption.TabIndex = 6;
            this.showOption.Text = "Show Option";
            this.showOption.UseVisualStyleBackColor = true;
            this.showOption.Click += new System.EventHandler(this.button3_Click);
            // 
            // virtualFormHost1
            // 
            this.virtualFormHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.virtualFormHost1.BackColor = System.Drawing.Color.White;
            this.virtualFormHost1.Content = null;
            this.virtualFormHost1.Location = new System.Drawing.Point(12, 12);
            this.virtualFormHost1.Name = "virtualFormHost1";
            this.virtualFormHost1.Size = new System.Drawing.Size(600, 360);
            this.virtualFormHost1.TabIndex = 0;
            this.virtualFormHost1.Text = "virtualFormHost1";
            // 
            // showTest
            // 
            this.showTest.Location = new System.Drawing.Point(537, 415);
            this.showTest.Name = "showTest";
            this.showTest.Size = new System.Drawing.Size(75, 23);
            this.showTest.TabIndex = 7;
            this.showTest.Text = "Show Test";
            this.showTest.UseVisualStyleBackColor = true;
            this.showTest.Click += new System.EventHandler(this.showTest_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.showTest);
            this.Controls.Add(this.showOption);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.moveInChild);
            this.Controls.Add(this.moveInChild2);
            this.Controls.Add(this.virtualFormHost1);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainWindow_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NullLib.VirtualForms.Win.VirtualControlHost virtualFormHost1;
        private System.Windows.Forms.Button moveInChild2;
        private System.Windows.Forms.Button moveInChild;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button showOption;
        private System.Windows.Forms.Button showTest;
    }
}

