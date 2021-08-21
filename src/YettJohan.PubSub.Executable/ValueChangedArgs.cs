using System;
namespace YettJohan.PubSub.Executable {
    public class ValueChangedArgs<T> : EventArgs {
        public ValueChangedArgs(T value) {
            Value = value;
        }
        public ValueChangedArgs() {
        }
        public T? Value { get; }
    }
}