namespace NullLib.VirtualForms
{
    public class TextChangedEventArgs : PropertyChangedEventArgs<string>
    {
        public TextChangedEventArgs(string oldText, string newText) : base(oldText, newText) { }
    }
}