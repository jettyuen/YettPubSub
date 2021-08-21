using System;
namespace YettJohan.PubSub.Executable {
    public static class Program {
        public static void Main() {
            EventHub hub = new();
            ValueChangedArgs<string?> mockArgs = new();
            ValueChangedArgs<int?> anotherMockArgs = new();
            MockPublisher mockPublisher = new();
            MockSubscriber mockSubscriber = new();
            MockSubscriber anotherMockSubscriber = new();
            const string name = "Jett";
            string? inputName = null;
            while (inputName != name) {
                inputName = Console.ReadLine();
            }
            hub.CreateTopic(mockPublisher, mockArgs, "MockValueChangedString");
            hub.CreateTopic(mockPublisher, anotherMockArgs,
                "MockValueChangedInt");
            hub.Subscribe(mockArgs, "MockValueChangedString",
                mockSubscriber.MockAction);
            hub.Subscribe(mockArgs,
                        "MockValueChangedString", anotherMockSubscriber
                        .MockAction);
            hub.Subscribe(anotherMockArgs,
            "MockValueChangedInt", mockSubscriber.AnotherMockAction);
            hub.Unsubscribe(mockArgs, "MockValueChangedString",
                mockSubscriber.MockAction);
            hub.Publish(mockPublisher, "MockValueChangedInt", new
            ValueChangedArgs<int?>(3));
            hub.Publish(mockPublisher, "MockValueChangedString",
                new ValueChangedArgs<string>("My name is " + inputName));
        }
    }
    public class MockPublisher {
    }
    public class MockSubscriber {
        public Action<object, EventArgs> MockAction { get; }
        public Action<object, EventArgs> AnotherMockAction { get; }
        public MockSubscriber() {
            MockAction = PrintStringValue;
            AnotherMockAction = PrintIntValue;
        }
        private void PrintStringValue(object o, EventArgs args) {
            Console.WriteLine("Value:" + (args as ValueChangedArgs<string?>)?
            .Value);
        }
        private void PrintIntValue(object o, EventArgs args) {
            Console.WriteLine("Value:" + (args as ValueChangedArgs<int?>)?
            .Value);
        }
    }
}