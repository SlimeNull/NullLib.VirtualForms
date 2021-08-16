namespace NullLib.VirtualForms
{
    public struct Thickness
    {
        private int bottom;
        private int right;
        private int top;
        private int left;

        public int Left { get => left; set => left = value; }
        public int Top { get => top; set => top = value; }
        public int Right { get => right; set => right = value; }
        public int Bottom { get => bottom; set => bottom = value; }

        public Thickness(int all) => bottom = right = top = left = all;
        public Thickness(int x, int y) => (left, top, right, bottom) = (x, y, x, y);
        public Thickness(int left, int y, int right) => (this.left, top, this.right, bottom) = (left, y, right, y);
        public Thickness(int left, int top, int right, int bottom) => (this.left, this.top, this.right, this.bottom) = (left, top, right, bottom);


        /// <summary>Returns the hash code of the structure.</summary>
        /// <returns>A hash code for this instance of <see cref="T:System.Windows.Thickness" />.</returns>
        public override int GetHashCode()
        {
            return left.GetHashCode() ^ top.GetHashCode() ^ right.GetHashCode() ^ bottom.GetHashCode();
        }
    }
}
