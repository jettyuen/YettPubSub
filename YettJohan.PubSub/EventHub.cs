using System;
using System.Collections.Generic;
using System.Linq;
namespace YettJohan.PubSub {
    public class EventHub {
        private readonly Dictionary<object, List<Topic>>
                _topicsBySender = new();
        public void Publish(object sender, EventArgs eventArgs = default!) {
            if (!TopicExists(sender, eventArgs)) {
                return;
            }
            try {
                _topicsBySender[sender].Find(topic => topic.EventArgsType ==
                        eventArgs.GetType())?.Raise(sender, eventArgs);
            }
            catch {
                throw new Exception("Exception was thrown on an" +
                        " invoked topic event");
            }
        }
        public void Subscribe(EventArgs eventArgs,
                Action<object, EventArgs> action) {
            foreach (List<Topic> topics in _topicsBySender.Values) {
                foreach (Topic topic in topics) {
                    if (topic.EventArgsType == eventArgs.GetType()) {
                        topic.Event += (o, args) => action(o!, args);
                    }
                }
            }
        }
        public void CreateTopic(object sender, EventArgs eventArgs) {
            if (_topicsBySender.ContainsKey(sender)) {
                _topicsBySender[sender].Add(new Topic(sender, eventArgs));
                return;
            }
            _topicsBySender.Add(sender, new List<Topic>());
            if (TopicExists(sender, eventArgs)) {
                throw new Exception("Topic already exists");
            }
            _topicsBySender[sender].Add(new Topic(sender, eventArgs));
        }
        public List<Topic> GetTopics(object sender) {
            if (_topicsBySender.ContainsKey(sender)) {
                return _topicsBySender[sender];
            }
            throw new ArgumentException("Sender does not exist in dictionary");
        }
        private bool TopicExists(object sender, EventArgs args) {
            return _topicsBySender[sender].Any(topic =>
                    topic.EventArgsType == args.GetType());
        }
    }
}