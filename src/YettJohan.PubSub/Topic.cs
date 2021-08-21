using System;
namespace YettJohan.PubSub {
    public class Topic {
        public Topic(object sender, EventArgs eventArgs) {
            Sender = sender;
            EventArgsType = eventArgs.GetType();
        }
        public Type EventArgsType { get; }
        public object Sender { get; }
        public event EventHandler<EventArgs>? Event;
        public void Raise(object? sender, EventArgs args) {
            Event?.Invoke(sender, args);
        }
    }
}