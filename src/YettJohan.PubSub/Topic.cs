using System.Collections.Generic;
namespace YettJohan.PubSub {
    public class Topic {
        public Topic(string name) {
            Name = name;
        }
        public string Name { get;  }
        public List<object> Actions { get; } = new();
    }
}