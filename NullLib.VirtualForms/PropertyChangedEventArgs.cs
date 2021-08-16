using System;

namespace NullLib.VirtualForms
{
    public class PropertyChangedEventArgs<T> : EventArgs
    {
        public PropertyChangedEventArgs(T oldValue, T newValue) => (OldValue, NewValue) = (oldValue, newValue);

        public T NewValue { get; }
        public T OldValue { get; }
    }
}
