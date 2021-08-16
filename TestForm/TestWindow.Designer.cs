
namespace TestForm
{
    partial class TestWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.parentPanel = new System.Windows.Forms.Panel();
            this.tip1 = new System.Windows.Forms.Label();
            this.tip2 = new System.Windows.Forms.Label();
            this.childPanel = new System.Windows.Forms.Panel();
            this.parentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // parentPanel
            // 
            this.parentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.parentPanel.Controls.Add(this.childPanel);
            this.parentPanel.Location = new System.Drawing.Point(98, 70);
            this.parentPanel.Name = "parentPanel";
            this.parentPanel.Size = new System.Drawing.Size(472, 225);
            this.parentPanel.TabIndex = 0;
            this.parentPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.parentPanel_MouseMove);
            // 
            // tip1
            // 
            this.tip1.AutoSize = true;
            this.tip1.Location = new System.Drawing.Point(98, 319);
            this.tip1.Name = "tip1";
            this.tip1.Size = new System.Drawing.Size(27, 13);
            this.tip1.TabIndex = 1;
            this.tip1.Text = "tip 1";
            // 
            // tip2
            // 
            this.tip2.AutoSize = true;
            this.tip2.Location = new System.Drawing.Point(98, 332);
            this.tip2.Name = "tip2";
            this.tip2.Size = new System.Drawing.Size(27, 13);
            this.tip2.TabIndex = 1;
            this.tip2.Text = "tip 1";
            // 
            // childPanel
            // 
            this.childPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.childPanel.Location = new System.Drawing.Point(133, 54);
            this.childPanel.Name = "childPanel";
            this.childPanel.Size = new System.Drawing.Size(200, 100);
            this.childPanel.TabIndex = 0;
            this.childPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.childPanel_MouseMove);
            // 
            // TestWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tip2);
            this.Controls.Add(this.tip1);
            this.Controls.Add(this.parentPanel);
            this.Name = "TestWindow";
            this.Text = "TestWindow";
            this.parentPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel parentPanel;
        private System.Windows.Forms.Label tip1;
        private System.Windows.Forms.Label tip2;
        private System.Windows.Forms.Panel childPanel;
    }
}