using System;
namespace YettJohan.PubSub.Executable {
    public class ValueChangedArgs<T> : EventArgs {
        public ValueChangedArgs(T value) {
            Value = value;
        }
        public T? Value { get; }
    }
}