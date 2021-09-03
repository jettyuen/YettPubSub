using System;
namespace YettJohan.PubSub.Executable {
    public static class Program {
        public static void Main() {
            EventHub eventHub = new();
            var mockPub = new MockPublisher();
            eventHub.CreateTopic<int>(mockPub, "MyNewTopic");
            var mockSub = new MockSubscriber(eventHub);

            eventHub.Subscribe<int>("MyNewTopic", Console.WriteLine);
            eventHub.Publish(mockPub, "MyNewTopic", 3);
            eventHub.Publish(mockPub, "MyNewTopic", 4);
        }
    }
    public class MockPublisher {
    }
    public class MockSubscriber {
        public MockSubscriber(EventHub eventHub) {
            MockAction = PrintStringValue;
            AnotherMockAction = PrintIntValue;
            eventHub.Subscribe("MyNewTopic", AnotherMockAction);
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