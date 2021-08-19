namespace YettPubSub
{
    public class Class1
    {
        public void Main()
        {
            Topic myTopic = EventBus.Bus.AddTopic(this, new MyEventArgs(2));
            myTopic.Message += (sender, args) => { Console.WriteLine("Hello!"); };
            myTopic.Raise(this, new MyEventArgs(2));
        }
    }
    public class EventBus
    {
        private static EventBus _eventBus;
        public static EventBus Bus => _eventBus ??= new EventBus();
        private Dictionary<object, List<Topic>> _topicsBySender = new();
        public Topic AddTopic(object sender, EventArgs args)
        {
            var topic = new Topic(args);
            if (!_topicsBySender.ContainsKey(sender))
            {
                _topicsBySender.Add(sender, new List<Topic> { topic });
                return topic;
            }
            _topicsBySender[sender].Add(topic);
            return topic;
        }
    }
    public class Topic
    {
        public EventArgs Arguments { get; }
        public event EventHandler<EventArgs> Message;
        public Topic(EventArgs args)
        {
            Arguments = args;
        }

        public void Raise(object sender, EventArgs args)
        {
            Message?.Invoke(sender, args);
        }
    }
    public class MyEventArgs : EventArgs
    {
        public int Value { get; }
        public MyEventArgs(int val)
        {
            Value = val; 
        }
    }
}
