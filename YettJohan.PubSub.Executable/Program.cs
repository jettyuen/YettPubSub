using System;
namespace YettJohan.PubSub.Executable {
    public static class Program {
        public static void Main() {
            EventHub hub = new();
            MockPublisher mockPublisher = new(hub);
            MockSubscriber mockSubscriber = new(hub);
            string name = "Jett";
            string? inputName = null;
            while (inputName != name) {
                inputName = Console.ReadLine();
            }
            hub.Publish(mockPublisher,
                new ValueChangedArgs<string>("My name is " + inputName));
        }
    }
    public class MockPublisher {
        public MockPublisher(EventHub hub) {
            ValueChangedArgs<string?> mockArgs = new(null);
            hub.CreateTopic(this, mockArgs);
        }
    }
    public class MockSubscriber {
        public MockSubscriber(EventHub hub) {
            ValueChangedArgs<string?> mockArgs = new(null);
            hub.Subscribe(mockArgs, (o, args) => Console.WriteLine("Value: "
                + (args as ValueChangedArgs<string>)?.Value));
        }
    }
}