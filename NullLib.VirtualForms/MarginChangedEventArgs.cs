namespace NullLib.VirtualForms
{
    public class MarginChangedEventArgs : PropertyChangedEventArgs<Thickness>
    {
        public MarginChangedEventArgs(Thickness oldMargin, Thickness newMargin) : base(oldMargin, newMargin) { }
    }
}
