using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace NullLib.VirtualForms
{
    public partial class VControl     // locating and sizing properties
    {
        private int left = 0, top = 0, right = 0, bottom = 0, width = 200, height = 100;
        private bool focused;
        private int horizontalAlignment;
        private int verticalAlignment;

        public int Left
        {
            get => left;
            set => SetMarginAndNotifyAll(value, top, right, bottom, true);
        }
        public int Top
        {
            get => top;
            set => SetMarginAndNotifyAll(left, value, right, bottom, true);
        }
        public int Right
        {
            get => right;
            set => SetMarginAndNotifyAll(left, top, value, bottom, true);
        }
        public int Bottom
        {
            get => bottom;
            set => SetMarginAndNotifyAll(left, top, right, value, true);
        }
        public int Width
        {
            get => width;
            set => SetSizeAndNotifyAll(value, height, true);
        }
        public int Height
        {
            get => height;
            set => SetSizeAndNotifyAll(width, value, true);
        }

        public Thickness Margin
        {
            get => new(left, top, right, bottom);
            set => SetMarginAndNotifyAll(value.Left, value.Top, value.Right, value.Bottom, true);
        }
        public Size Size
        {
            get => new(width, height);
            set => SetSizeAndNotifyAll(value.Width, value.Height, true);
        }
        public Size AbsSize
        {
            get
            {
                return AbsBounds.Size;
            }
            set
            {
                Point absLocation = AbsLocation;
                SetAbsBoundsAndNotifyAll(absLocation.X, absLocation.Y, value.Width, value.Height, true);
            }
        }
        public Point AbsLocation
        {
            get
            {
                return AbsBounds.Location;
            }
            set
            {
                Size absSize = AbsSize;
                SetAbsBoundsAndNotifyAll(value.X, value.Y, absSize.Width, absSize.Height, true);
            }
        }

        public Rectangle AbsBounds
        {
            get => CalcAbsBounds(left, top, right, bottom, width, height);
            set => SetAbsBoundsAndNotifyAll(value.X, value.Y, value.Width, value.Height, true);
        }

        public VerticalAlignment VerticalAlignment
        {
            get => (VerticalAlignment)verticalAlignment; set
            {
                Interlocked.Exchange(ref verticalAlignment, (int)value);
            }
        }
        public HorizontalAlignment HorizontalAlignment
        {
            get => (HorizontalAlignment)horizontalAlignment; set
            {
                Interlocked.Exchange(ref horizontalAlignment, (int)value);
            }
        }

        public bool Focused
        {
            get => focused;
            set
            {
                if (focused == value)
                    return;

                focused = value;
                if (value)
                {
                    UpdateFocus(this);
                    OnGotFocus(new EventArgs());
                }
                else
                {
                    OnLostFocuse(new EventArgs());
                }
            }
        }
    }
    public partial class VControl     // Graphics drawing basic
    {
        private readonly object bufferLock = new();
        private Bitmap bufferBmp;
        private void AllocNewBuffer(Size size)
        {
            lock (bufferLock)
            {
                bufferBmp = new(Math.Max(size.Width, 1), Math.Max(size.Height, 1), PixelFormat.Format24bppRgb);
                canvas = Graphics.FromImage(bufferBmp);
                BufferSize = bufferBmp.Size;
            }
            ForceProcessPaint(true);
        }

        protected Size BufferSize { get; private set; }
        protected Bitmap BufferBitmap
        {
            get
            {
                lock (bufferLock)
                {
                    if (bufferBmp is null)
                    {
                        AllocNewBuffer(AbsSize);
                    }
                    else
                    {
                        Size bufferSize = BufferSize;
                        Size absSize = AbsSize;
                        if (bufferSize.Width < absSize.Width || bufferSize.Height < absSize.Height)
                            AllocNewBuffer(absSize);
                    }

                    return bufferBmp;
                }
            }
        }
        public VControl Parent
        {
            get => parent; protected set
            {
                if (ReferenceEquals(parent, value))
                    return;

                parent = value;
                ForceProcessPaint(true);
            }
        }
        public VirtualWindowCollection Children { get; }

        private Graphics canvas;
        private VControl parent;

        public Graphics Canvas
        {
            get
            {
                if (canvas is null)
                    _ = BufferBitmap;
                return canvas;
            }
        }

        public class VirtualWindowCollection : ICollection<VControl>
        {
            private readonly List<VControl> items = new();

            public VControl Owner { get; }
            public VirtualWindowCollection(VControl owner) => Owner = owner;
            public int Count => items.Count;

            public bool IsReadOnly => false;

            public void Add(VControl item)
            {
                if (item is null)
                    throw new ArgumentNullException(nameof(item));

                items.Add(item);
                item.Parent = Owner;
            }

            public void Clear()
            {
                foreach (var item in items)
                    item.Parent = null;
                items.Clear();
            }

            public bool Contains(VControl item)
            {
                return items.Contains(item);
            }

            public void CopyTo(VControl[] array, int arrayIndex)
            {
                items.CopyTo(array, arrayIndex);
            }

            public IEnumerator<VControl> GetEnumerator()
            {
                return items.GetEnumerator();
            }

            public IEnumerable<VControl> ReverseEnum()
            {
                for (int i = items.Count - 1; i >= 0; i--)
                    yield return items[i];
            }

            public bool Remove(VControl item)
            {
                if (items.Remove(item))
                    item.Parent = null;
                else
                    return false;
                return true;
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
    public partial class VControl     // events
    {
        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;
        public event EventHandler<KeyPressEventArgs> KeyPress;

        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseClick;
        public event EventHandler<MouseEventArgs> MouseDoubleClick;

        public event EventHandler<MouseEventArgs> MouseMove;

        public event EventHandler<MarginChangedEventArgs> MarginChanged;
        public event EventHandler<SizeChangedEventArgs> SizeChanged;
        public event EventHandler<AbsBoundsChangedEventArgs> AbsBoundsChanged;

        public event EventHandler<EventArgs> GotFocus;
        public event EventHandler<EventArgs> LostFocus;

        public event EventHandler<BackColorChangedEventArgs> BackColorChanged;
        public event EventHandler<ForeColorChangedEventArgs> ForeColorChanged;



        bool PointInClient(Point clientPoint)
        {
            Size absSize = AbsSize;
            return clientPoint.X >= 0 && clientPoint.Y >= 0 && clientPoint.X < absSize.Width && clientPoint.Y < absSize.Height;
        }
        void GetMouseEventArgsMembers(MouseEventArgs args, out MouseButtons buttons, out int clicks, out int x, out int y, out int delta)
        {
            buttons = args.Button;
            clicks = args.Clicks;
            x = args.X;
            y = args.Y;
            delta = args.Delta;
        }
        MouseEventArgs GenChildMouseEvent(VControl vw, MouseButtons buttons, int clicks, int x, int y, int delta)
        {
            vw.PointToClient(x, y, out int rx, out int ry);
            return new MouseEventArgs(buttons, clicks, rx, ry, delta);
        }

        public virtual bool ProcessKeyDown(KeyEventArgs args)
        {
            if (Focused)
                OnKeyDown(args);
            if (!args.Handled)
            {
                foreach (var child in Children)
                {
                    child.ProcessKeyDown(args);
                    if (args.Handled)
                        break;
                }
            }
            return true;
        }
        public virtual bool ProcessKeyUp(KeyEventArgs args)
        {
            if (Focused)
                OnKeyUp(args);
            if (!args.Handled)
            {
                foreach (var child in Children)
                {
                    child.ProcessKeyUp(args);
                    if (args.Handled)
                        break;
                }
            }
            return true;
        }
        public virtual bool ProcessKeyPress(KeyPressEventArgs args)
        {
            if (Focused)
                OnKeyPress(args);
            if (!args.Handled)
            {
                foreach (var child in Children)
                {
                    child.ProcessKeyPress(args);
                    if (args.Handled)
                        break;
                }
            }
            return true;
        }

        private bool ProcessChildMouseEvent(Func<VControl, Func<MouseEventArgs, bool>> childEventGetter, MouseEventArgs args)
        {
            GetMouseEventArgsMembers(args,
                                out MouseButtons buttons,
                                out int clicks,
                                out int x, out int y,
                                out int delta);

            Region topRegion = new Region();
            topRegion.MakeInfinite();
            foreach (var child in Children.ReverseEnum())
            {
                var childAbsBounds = child.AbsBounds;
                if (!topRegion.IsVisible(x, y))
                    return false;
                if (!topRegion.IsVisible(childAbsBounds))
                    continue;
                MouseEventArgs reArgs = GenChildMouseEvent(child, buttons, clicks, x, y, delta);
                childEventGetter.Invoke(child).Invoke(reArgs);
                if (args.Handled = reArgs.Handled)
                    return true;
                topRegion.Exclude(childAbsBounds);
            }

            return false;
        }
        public virtual bool ProcessMouseDown(MouseEventArgs args)
        {
            if (!PointInClient(args.Location))
                return false;
            if (ProcessChildMouseEvent((child) => child.ProcessMouseDown, args))
                return true;

            OnMouseDown(args);
            return true;
        }
        public virtual bool ProcessMouseUp(MouseEventArgs args)
        {
            if (!PointInClient(args.Location))
                return false;
            if (ProcessChildMouseEvent((child) => child.ProcessMouseUp, args))
                return true;

            OnMouseUp(args);
            return true;
        }
        public virtual bool ProcessMouseClick(MouseEventArgs args)
        {
            if (!PointInClient(args.Location))
                return false;
            if (ProcessChildMouseEvent((child) => child.ProcessMouseClick, args))
                return true;

            OnMouseClick(args);

            if (Parent is null)
                TryGiveFocus(args);

            return true;
        }
        public virtual bool ProcessMouseDoubleClick(MouseEventArgs args)
        {
            if (!PointInClient(args.Location))
                return false;
            if (ProcessChildMouseEvent((child) => child.ProcessMouseDoubleClick, args))
                return true;
            OnMouseDoubleClick(args);

            if (Parent is null)
                TryGiveFocus(args);

            return true;
        }
        public virtual bool ProcessMouseMove(MouseEventArgs args)
        {
            if (!PointInClient(args.Location))
                return false;
            if (ProcessChildMouseEvent((child) => child.ProcessMouseMove, args))
                return true;

            OnMouseMove(args);
            return true;
        }
        protected virtual void OnKeyDown(KeyEventArgs args) => KeyDown?.Invoke(this, args);
        protected virtual void OnKeyUp(KeyEventArgs args) => KeyUp?.Invoke(this, args);
        protected virtual void OnKeyPress(KeyPressEventArgs args) => KeyPress?.Invoke(this, args);
        protected virtual void OnMouseDown(MouseEventArgs args) => MouseDown?.Invoke(this, args);
        protected virtual void OnMouseUp(MouseEventArgs args) => MouseUp?.Invoke(this, args);
        protected virtual void OnMouseClick(MouseEventArgs args) => MouseClick?.Invoke(this, args);
        protected virtual void OnMouseDoubleClick(MouseEventArgs args) => MouseDoubleClick?.Invoke(this, args);
        protected virtual void OnMouseMove(MouseEventArgs args) => MouseMove?.Invoke(this, args);

        private void ProcessRenderRequest(bool render)
        {
            if (render)
            {
                RequestRender();
                if (Parent is VControl parent)
                    parent.ForceProcessPaint(true);
            }
        }
        protected virtual void ProcessMarginChanged(MarginChangedEventArgs args, bool render)
        {
            ProcessRenderRequest(render);
            OnMarginChanged(args);
        }
        protected virtual void ProcessSizeChanged(SizeChangedEventArgs args, bool render)
        {
            ProcessRenderRequest(render);
            OnSizeChanged(args);
        }
        protected virtual void ProcessAbsBoundsChanged(AbsBoundsChangedEventArgs args, bool render)
        {
            ProcessRenderRequest(render);
            OnAbsBoundsChanged(args);
        }
        protected virtual void OnMarginChanged(MarginChangedEventArgs args) => MarginChanged?.Invoke(this, args);
        protected virtual void OnSizeChanged(SizeChangedEventArgs args) => SizeChanged?.Invoke(this, args);
        protected virtual void OnAbsBoundsChanged(AbsBoundsChangedEventArgs args) => AbsBoundsChanged?.Invoke(this, args);
        protected virtual void OnGotFocus(EventArgs args) => GotFocus?.Invoke(this, args);
        protected virtual void OnLostFocuse(EventArgs args) => LostFocus?.Invoke(this, args);

        public virtual void ProcessForeColorChanged(ForeColorChangedEventArgs args, bool render)
        {
            ProcessRenderRequest(render);
            OnForeColorChanged(args);
        }
        public virtual void ProcessBackColorChanged(BackColorChangedEventArgs args, bool render)
        {
            ProcessRenderRequest(render);
            OnBackColorChanged(args);
        }
        protected virtual void OnForeColorChanged(ForeColorChangedEventArgs args) => ForeColorChanged?.Invoke(this, args);
        protected virtual void OnBackColorChanged(BackColorChangedEventArgs args) => BackColorChanged?.Invoke(this, args);
    }
    public partial class VControl     // back color and render paint core
    {
        private bool needReRender = true;
        private Color backColor;
        private Color foreColor;

        protected void RequestRender() => needReRender = true;
        protected void ForceProcessPaint(bool rerender)
        {
            if (rerender)
                RequestRender();
            ProcessPaint(GetPaintEventArgs());
        }

        public Color BackColor
        {
            get => backColor; set
            {
                BackColorChangedEventArgs args = new(backColor, foreColor);
                backColor = value;
                ProcessBackColorChanged(args, true);
            }
        }
        public Color ForeColor
        {
            get => foreColor; set
            {
                ForeColorChangedEventArgs args = new(foreColor, value);
                foreColor = value;
                ProcessForeColorChanged(args, true);
            }
        }
        protected PaintEventArgs GetPaintEventArgs()
        {
            Rectangle absBounds = AbsBounds;
            return new PaintEventArgs(Canvas, Parent?.Canvas, new Rectangle(Point.Empty, absBounds.Size), absBounds);
        }

        public event EventHandler<PaintEventArgs> Paint;
        public event EventHandler<PaintEventArgs> PaintBackground;
        public event EventHandler<PaintEventArgs> PaintContent;

        protected virtual void RenderBackgroundAndNotify(PaintEventArgs args)
        {
            Graphics canvas = args.Canvas;

            Size abssize = args.Clip.Size;
            Rectangle selfAbsBounds = args.SelfBounds;
            Color backColor = BackColor;

            if (Parent is VControl parent)
            {
                if (TransparentEffectEnabled)
                    canvas.DrawImage(parent.BufferBitmap, new Rectangle(Point.Empty, abssize), selfAbsBounds, GraphicsUnit.Pixel);
                else
                    canvas.Clear(backColor);
            }
            else
            {
                canvas.Clear(backColor);
            }

            canvas.FillRectangle(new SolidBrush(backColor), new Rectangle(Point.Empty, abssize));

            OnPaintBackground(args);
        }
        protected virtual void RenderContentAndNotify(PaintEventArgs args)
        {
            if (TransparentEffectEnabled)
                foreach (VControl child in Children)
                    child.ForceProcessPaint(true);
            else
                foreach (VControl child in Children)
                    child.ForceProcessPaint(false);

            needReRender = false;
        }
        protected virtual void RenderBufferToParent(PaintEventArgs args)
        {
            Size abssize = args.Clip.Size;
            Rectangle selfAbsBounds = args.SelfBounds;

            if (args.ParentCanvas is Graphics parentCanvas)
                parentCanvas.DrawImage(BufferBitmap, selfAbsBounds, new Rectangle(Point.Empty, abssize), GraphicsUnit.Pixel);
        }

        public virtual void ProcessPaint(PaintEventArgs args)
        {
            if (needReRender)
            {
                RenderBackgroundAndNotify(args);
                RenderContentAndNotify(args);
            }

            OnPaint(args);

            RenderBufferToParent(args);
        }
        protected virtual void OnPaint(PaintEventArgs args) => Paint?.Invoke(this, args);
        protected virtual void OnPaintBackground(PaintEventArgs args) => PaintBackground?.Invoke(this, args);
        protected virtual void OnPaintContent(PaintEventArgs args) => PaintContent?.Invoke(this, args);
    }
    public partial class VControl     // functions
    {
        public void Refresh()
        {
            ForceProcessPaint(needReRender);
        }
        public void Refresh(bool render)
        {
            ForceProcessPaint(render);
        }
        public Point PointToClient(Point point)
        {
            Point absLocation = AbsLocation;
            return new Point(point.X - absLocation.X, point.Y - absLocation.Y);
        }
        public Point PointToClient(int x, int y)
        {
            Point absLocation = AbsLocation;
            return new Point(x - absLocation.X, y - absLocation.Y);
        }
        public void PointToClient(int x, int y, out int rx, out int ry)
        {
            Point absLocation = AbsLocation;
            rx = x - absLocation.X;
            ry = y - absLocation.Y;
        }
        public Point PointToParent(Point point)
        {
            Point absLocation = AbsLocation;
            return new Point(point.X + absLocation.X, point.Y + absLocation.Y);
        }
        public Point PointToParent(int x, int y)
        {
            Point absLocation = AbsLocation;
            return new Point(x + absLocation.X, y + absLocation.Y);
        }
        public void PointToParent(int x, int y, out int rx, out int ry)
        {
            Point absLocation = AbsLocation;
            rx = x + absLocation.X;
            ry = y + absLocation.Y;
        }

        private bool TryGiveFocus(MouseEventArgs args)
        {
            if (!PointInClient(args.Location))
                return false;

            foreach (var child in Children.ReverseEnum())
                if (child.TryGiveFocus(args))
                    return true;

            Focused = true;
            return true;
        }
        public void GiveFocus()
        {
            Focused = true;
            UpdateFocus(this);
        }
        public void ClearAllFocus()
        {
            Focused = false;
            UpdateFocus(null);
        }

        protected void UpdateFocus(VControl focusedWindow)
        {
            if (Parent is VControl)
                Parent.UpdateFocus(focusedWindow);
            else
                NotifyFocus(focusedWindow);
        }
        private void NotifyFocus(VControl focusedWindow)
        {
            if (ReferenceEquals(this, focusedWindow))
                Focused = true;
            else
                Focused = false;

            foreach (VControl child in Children)
                child.NotifyFocus(focusedWindow);
        }

        private bool SetMarginAndNotify(int left, int top, int right, int bottom, bool render)
        {
            if (this.left == left &&
                this.top == top &&
                this.right == right &&
                this.bottom == bottom)
                return false;

            MarginChangedEventArgs args = new MarginChangedEventArgs(Margin, new Thickness(left, top, right, bottom));
            Interlocked.Exchange(ref this.left, left);
            Interlocked.Exchange(ref this.top, top);
            Interlocked.Exchange(ref this.right, right);
            Interlocked.Exchange(ref this.bottom, bottom);
            ProcessMarginChanged(args, render);

            return true;
        }
        private bool SetMarginAndNotifyAll(int left, int top, int right, int bottom, bool render)
        {
            if (!SetMarginAndNotify(left, top, right, bottom, false))
                return false;

            ProcessAbsBoundsChanged(new AbsBoundsChangedEventArgs(AbsBounds, CalcAbsBounds(left, top, right, bottom, width, height)), render);
            return true;
        }
        private bool SetSizeAndNotify(int width, int height, bool render)
        {
            if (this.width == width &&
                this.height == height)
                return false;

            SizeChangedEventArgs sargs = new SizeChangedEventArgs(Size, new Size(width, height));
            Interlocked.Exchange(ref this.width, width);
            Interlocked.Exchange(ref this.height, height);
            ProcessSizeChanged(sargs, render);

            return true;
        }
        private bool SetSizeAndNotifyAll(int width, int height, bool render)
        {
            if (!SetSizeAndNotify(width, height, false))
                return false;

            ProcessAbsBoundsChanged(new AbsBoundsChangedEventArgs(AbsBounds, CalcAbsBounds(left, top, right, bottom, width, height)), render);
            return true;
        }
        private bool SetMarginSizeAndNotifyAll(int left, int top, int right, int bottom, int width, int height, bool render)
        {
            if (!(SetMarginAndNotify(left, top, right, bottom, false) | SetSizeAndNotify(width, height, false)))
                return false;

            ProcessAbsBoundsChanged(new AbsBoundsChangedEventArgs(AbsBounds, CalcAbsBounds(left, top, right, bottom, width, height)), render);
            return true;
        }
        private bool SetAbsBoundsAndNotifyAll(int left, int top, int width, int height, bool render)
        {
            if (Parent is VControl parent)
            {
                Size parentAbsSize = parent.AbsSize;
                int
                    parentWidth = parentAbsSize.Width,
                    parentHeight = parentAbsSize.Height;

                int
                    right = parentWidth - left - width,
                    bottom = parentHeight - top - height;

                int outleft, outtop, outright, outbottom, outwidth, outheight;

                (outleft, outwidth, outright) = HorizontalAlignment switch
                {
                    HorizontalAlignment.Left => (left, width, right),
                    HorizontalAlignment.Right => (left, width, right),
                    HorizontalAlignment.Center => left < right ? (0, width, right - left) : (left - right, width, 0),
                    HorizontalAlignment.Stretch => (left, width, right),
                    _ => (left, width, right)
                };
                (outtop, outheight, outbottom) = VerticalAlignment switch
                {
                    VerticalAlignment.Top => (top, height, bottom),
                    VerticalAlignment.Bottom => (top, height, bottom),
                    VerticalAlignment.Center => top < bottom ? (0, height, bottom - top) : (top - bottom, height, 0),
                    VerticalAlignment.Stretch => (top, height, bottom),
                    _ => (top, height, bottom)
                };

                return SetMarginSizeAndNotifyAll(outleft, outtop, outright, outbottom, outwidth, outheight, render);
            }
            else
            {
                return SetMarginSizeAndNotifyAll(left, top, right, bottom, width, height, render);
            }
        }
        protected virtual Rectangle CalcAbsBounds(int left, int top, int right, int bottom, int width, int height)
        {
            if (Parent is VControl parent)
            {
                Size parentAbsSize = parent.AbsSize;
                int
                    parentWidth = parentAbsSize.Width,
                    parentHeight = parentAbsSize.Height;

                int outleft, outtop, outwidth, outheight;

                (outleft, outwidth) = HorizontalAlignment switch
                {
                    HorizontalAlignment.Left => (left, width),
                    HorizontalAlignment.Right => (parentWidth - width - right, width),
                    HorizontalAlignment.Center => ((parentWidth - width - left - right) / 2 + left, width),
                    HorizontalAlignment.Stretch => (left, parentWidth - left - right),
                    _ => (left, width)
                };
                (outtop, outheight) = VerticalAlignment switch
                {
                    VerticalAlignment.Top => (top, height),
                    VerticalAlignment.Bottom => (parentHeight - height - bottom, height),
                    VerticalAlignment.Center => ((parentHeight - height - top - bottom) / 2 + top, height),
                    VerticalAlignment.Stretch => (top, parentHeight - top - bottom),
                    _ => (top, height),
                };

                return new Rectangle(outleft, outtop, outwidth, outheight);
            }
            else
            {
                return new Rectangle(left, top, width, height);
            }
        }
        public static void DrawControl(Graphics canvas, VControl control)
        {
            if (control.needReRender)
                control.ForceProcessPaint(false);
            canvas.DrawImage(control.BufferBitmap, Point.Empty);
        }
    }
    public partial class VControl : IDisposable    // basic core
    {
        public VControl()
        {
            Children = new VirtualWindowCollection(this);
        }
        ~VControl()
        {
            Children.Clear();
            Dispose();
        }

        private string name;
        public string Name { get => name; set => Interlocked.Exchange(ref name, value); }

        private static bool transparentEnabled = true;
        private static bool mouseMultipleEventsEnabled;
        public static bool TransparentEffectEnabled => transparentEnabled;
        public static bool MouseMultipleEventsEnabled => mouseMultipleEventsEnabled;

        public static void SetMouseMultipleEventsEnabled(bool value)
        {
            mouseMultipleEventsEnabled = value;
        }

        public static void SetTransparentEnabled(bool value)
        {
            transparentEnabled = value;
        }

        public void Dispose()
        {
            bufferBmp?.Dispose();
        }
    }
}
