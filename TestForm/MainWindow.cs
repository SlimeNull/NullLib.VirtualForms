using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NullLib.TickAnimation;
using NullLib.VirtualForms;

namespace TestForm
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //g.FillRectangle(new SolidBrush(Color.Pink), new Rectangle(0, 0, 100, 100));
            
        }
        private void MainWindow_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            virtualFormHost1.Refresh();
        }

        Image myImg;

        VControl inChild;
        VControl optionBar;
        private void MainWindow_Load(object sender, EventArgs e)
        {
            myImg = Image.FromFile(@"C:\Users\Null\Desktop\點開看老婆.png");
            VControl vControl = CreateVControl();

            //VControl.SetTransparentEnabled(false);
            virtualFormHost1.Content =
                vControl;

            //MessageBox.Show(showOption.Font.Size.ToString());
        }

        private VControl CreateVControl()
        {
            VControl vControl = new VControl()
            {
                Name = "VControl - Out",
                Size = virtualFormHost1.Size,
                BackColor = Color.White,
            };

            inChild = new VImageView()
            {
                Name = "InChild",
                Left = 50,
                Top = 50,
                Size = new Size(200, 200),
                BackColor = Color.Green,
                Image = myImg,
                ImageSizingMode = ImageSizingMode.UniformToFill,
            };

            Color pink = Color.Pink;
            optionBar = new VControl()
            {
                Name = "OptionBar",
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = NullLib.VirtualForms.HorizontalAlignment.Left,
                BackColor = Color.FromArgb(100, pink)
            };

            optionBar.Children.Add(new VLabel()
            {
                Text = "This is a VLabel",
                AutoSize = true,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = NullLib.VirtualForms.HorizontalAlignment.Right
            });

            Color purple = Color.Purple;
            vControl.Children.Add(inChild);
            vControl.Children.Add(optionBar);
            vControl.Children.Add(new VControl()
            {
                Size = new Size(50, 50),
                BackColor = Color.FromArgb(100, purple),
            });

            optionBarAnimator = new TickAnimator(new CubicBezierTicker(CubicBezierCurve.Back, EasingMode.EaseInOut), optionBar, nameof(optionBar.Width));
            animator = new DrawingTickAnimator(new CubicBezierTicker(CubicBezierCurve.Back, EasingMode.EaseOut), inChild, nameof(vControl.AbsLocation));
            optionBarAnimator.SetPropertySetter((action) => Invoke(action));
            animator.SetPropertySetter((action) => Invoke(action));
            inChild.KeyDown += (s, _e) =>
            {
                if (_e.KeyCode == NullLib.VirtualForms.Keys.Left)
                    inChild.Left--;
                else if (_e.KeyCode == NullLib.VirtualForms.Keys.Right)
                    inChild.Left++;
                else if (_e.KeyCode == NullLib.VirtualForms.Keys.Up)
                    inChild.Top--;
                else if (_e.KeyCode == NullLib.VirtualForms.Keys.Down)
                    inChild.Top++;
                else
                    return;

                vControl.Refresh();
            };

            vControl.GotFocus += (s, _e) => LogLine("VControl got focus");
            inChild.GotFocus += (s, _e) => LogLine("InChild got focus");

            vControl.LostFocus += (s, _e) => LogLine("VControl lost focus");
            inChild.LostFocus += (s, _e) => LogLine("InChild lost focus");

            inChild.AbsBoundsChanged += (s, _e) => label1.Text = _e.NewValue.ToString();
            inChild.MouseMove += (s, _e) => label2.Text = _e.Location.ToString();

            inChild.MouseClick += (s, _e) => LogLine("Fuck you world");
            inChild.MouseDoubleClick += (s, _e) => LogLine("Fuck you world, double!!!");
            return vControl;
        }

        void LogLine(string text)
        {
            logBox.AppendText(text + "\r\n");
        }

        DrawingTickAnimator animator;
        TickAnimator optionBarAnimator;
        private void button1_Click(object sender, EventArgs e)
        {
            animator.Animate(new Point(50, 50), 500);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            animator.Animate(new Point(25, 25), 500);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (optionBar.Width == 0)
                optionBarAnimator.Animate(150, 500);
            else
                optionBarAnimator.Animate(0, 500);
        }

        private void showTest_Click(object sender, EventArgs e)
        {
            new TestWindow().ShowDialog();
        }
    }
}
