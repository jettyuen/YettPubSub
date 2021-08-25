using System;
namespace YettJohan.PubSub.Executable {
    public static class Program {
        public static void Main() {
            EventHub hub = new();
            MockPublisher mockPublisher = new();
            MockSubscriber mockSubscriber = new();
            MockSubscriber anotherMockSubscriber = new();
            hub.CreateTopic<DataPassArgs<string?>>(mockPublisher,
                "NameBecameJettTopic");
            hub.CreateTopic<DataPassArgs<int?>>(mockPublisher,
                "ProcessFinishedTopic");
            hub.Subscribe("NameBecameJettTopic", mockSubscriber.MockAction!);
            hub.Subscribe("NameBecameJettTopic",
                anotherMockSubscriber.MockAction!);
            hub.Unsubscribe("NameBecameJettTopic", mockSubscriber.MockAction!);
            hub.Subscribe("ProcessFinishedTopic",
                anotherMockSubscriber.AnotherMockAction!);
            const string name = "Jett";
            string? inputName = null;
            while (inputName != name) {
                inputName = Console.ReadLine();
            }
            hub.Publish(mockPublisher, "NameBecameJettTopic", new
                    DataPassArgs<string?>("My name is Jett"));
            hub.Publish(mockPublisher, "ProcessFinishedTopic", new
                    DataPassArgs<int?>(2));
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
        public Action<DataPassArgs<string?>> MockAction { get; }
        public Action<DataPassArgs<int?>> AnotherMockAction { get; }
        private void PrintStringValue(DataPassArgs<string?> args) {
            Console.WriteLine("Value:" + args.Value);
        }
        private void PrintIntValue(DataPassArgs<int?> args) {
            Console.WriteLine("Value:" + args.Value);
        }
    }
}