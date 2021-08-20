using System;
namespace YettJohan.PubSub {
    public class ValueChangedArgs<T> : EventArgs {
        public ValueChangedArgs(T value) {
            Value = value;
        }
        public T? Value { get; }
    }
}