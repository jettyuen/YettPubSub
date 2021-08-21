using System;
using System.Collections.Generic;
namespace YettJohan.PubSub {
    public class Topic {
        public Topic(EventArgs eventArgs, string name) {
            EventArgsType = eventArgs.GetType();
            Name = name;
        }
        public string Name { get;  }
        public Type EventArgsType { get; }
        public List<Action<object?, EventArgs>> Actions { get; set; } = new();
        public void Raise(object? sender, EventArgs args) {
            foreach (Action<object?, EventArgs> action in Actions) {
                action.Invoke(sender, args);
            }
        }
    }
}