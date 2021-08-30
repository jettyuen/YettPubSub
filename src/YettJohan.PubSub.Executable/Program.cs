using System;
namespace YettJohan.PubSub.Executable {
    public static class Program {
        public static void Main() {
            EventHub eventHub = new();
            eventHub.CreateTopic<int>(eventHub, "MyNewTopic");
            eventHub.CreateTopic<int>(eventHub, "MyOtherTopic");
            eventHub.CreateTopic<int>(eventHub, "MyOtherOtherTopic");
            eventHub.Subscribe<int>("MyNewTopic",
                args => Console.WriteLine($"Int value received: {args}"));
            eventHub.Publish(eventHub, "MyNewTopic", 3);
        }
    }
    public class DataPassArgs<T> {
        public DataPassArgs() {
        }
        public DataPassArgs(T? value) {
            Value = value;
        }
        public T? Value { get; set; }
    }
    public class MockPublisher {
    }
    public class MockSubscriber {
        public MockSubscriber() {
            MockAction = PrintStringValue;
            AnotherMockAction = PrintIntValue;
        }
        public Action<DataPassArgs<string>> MockAction { get; }
        public Action<DataPassArgs<int>> AnotherMockAction { get; }
        private void PrintStringValue(DataPassArgs<string> args) {
            Console.WriteLine("Value:" + args.Value);
        }
        private void PrintIntValue(DataPassArgs<int> args) {
            Console.WriteLine("Value:" + args.Value);
        }
    }
}