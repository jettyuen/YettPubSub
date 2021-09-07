using System;
namespace YettJohan.PubSub.Executable {
    public static class Program {
        public static void Main() {
            var mockPub = new MockPublisher();
            EventHub.Instance.CreateTopic<int>(mockPub, "MyNewTopic");
            var mockSub = new MockSubscriber();
            EventHub.Instance.Subscribe<int>("MyNewTopic", Console.WriteLine);
            EventHub.Instance.Publish(mockPub, "MyNewTopic", 3);
            EventHub.Instance.Publish(mockPub, "MyNewTopic", 4);
        }
    }
    public class MockPublisher {
    }
    public class MockSubscriber {
        public MockSubscriber() {
            MockAction = PrintStringValue;
            AnotherMockAction = PrintIntValue;
            EventHub.Instance.Subscribe("MyNewTopic", AnotherMockAction);
        }
        private Action<string> MockAction { get; }
        private Action<int> AnotherMockAction { get; }
        private void PrintStringValue(string args) {
            Console.WriteLine("Value:" + args);
        }
        private void PrintIntValue(int args) {
            Console.WriteLine("Value:" + args);
        }
    }
}