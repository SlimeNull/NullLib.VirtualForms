using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NullLib.VirtualForms.Win
{
    public partial class VirtualControlHost : Control
    {
        private VControl content;

        public VirtualControlHost() : base()
        {
            paintCallback = (s, e) => VControl.DrawControl(CreateGraphics(), content);
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            base.OnPreviewKeyDown(e);
        }


        private readonly EventHandler<PaintEventArgs> paintCallback;
        private void RegistEvent()
        {
            if (content is VControl)
                content.Paint += paintCallback;
        }

        private void UnRegistEvent()
        {
            if (content is VControl)
                content.Paint -= paintCallback;
        }

        public VControl Content
        {
            get => content;
            set
            {
                if (value is null)
                    return;
                if (content == value)
                    return;
                if (content is VControl)
                    UnRegistEvent();
                content = value;
                content.Size = Size;
                RegistEvent();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Content is VControl ctrl)
                ctrl.Size = Size;
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Content is VControl vw)
            {
                vw.Refresh();
                VControl.DrawControl(e.Graphics, vw);
            }
        }

        MouseEventArgs ConvertMouseEventArgs(System.Windows.Forms.MouseEventArgs e) => new((MouseButtons)e.Button, e.Clicks, e.X, e.Y, e.Delta);
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            Content?.ProcessMouseDown(ConvertMouseEventArgs(e));
            
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Content?.ProcessMouseUp(ConvertMouseEventArgs(e));
        }
        protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Content?.ProcessMouseClick(ConvertMouseEventArgs(e));
        }
        protected override void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            Content?.ProcessMouseDoubleClick(ConvertMouseEventArgs(e));
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Content?.ProcessMouseMove(ConvertMouseEventArgs(e));
        }

        KeyEventArgs ConvertKeyEventArgs(System.Windows.Forms.KeyEventArgs e) => new KeyEventArgs((Keys)e.KeyCode);
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Content?.ProcessKeyDown(ConvertKeyEventArgs(e));
            e.Handled = true;
        }
        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyUp(e);
            Content?.ProcessKeyUp(ConvertKeyEventArgs(e));
            e.Handled = true;
        }
        KeyPressEventArgs ConvertKeyPressEventArgs(System.Windows.Forms.KeyPressEventArgs e) => new KeyPressEventArgs(e.KeyChar);
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            Content?.ProcessKeyPress(ConvertKeyPressEventArgs(e));
            e.Handled = true;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Content?.ClearAllFocus();
        }
    }
    public partial class VirtualControlHost
    {
        static VirtualControlHost()
        {

        }
    }
}
