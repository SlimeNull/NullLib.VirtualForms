using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NullLib.VirtualForms
{
    public abstract class VAutoSizeControl : VControl
    {
        public bool AutoSize { get; set; }
        protected Graphics GetTempCanvas()
        {
            return Graphics.FromHwnd(IntPtr.Zero);
        }
        protected abstract Size CalcAutoSize();
        protected override Rectangle CalcAbsBounds(int left, int top, int right, int bottom, int width, int height)
        {
            if (AutoSize)
                Size = CalcAutoSize();
            return base.CalcAbsBounds(left, top, right, bottom, width, height);
        }
    }
}
