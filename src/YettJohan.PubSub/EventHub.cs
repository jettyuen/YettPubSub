using System;
using System.Collections.Generic;
using System.Linq;
namespace YettJohan.PubSub {
    public class EventHub {
        private readonly Dictionary<object, List<Topic>>
                _topicsByPublisher = new();
        public void Publish(object sender, string name, EventArgs eventArgs) {
            if (!TopicExists(sender, eventArgs, name)) {
                return;
            }
            try {
                _topicsByPublisher[sender].Find(topic =>
                        topic.EventArgsType == eventArgs.GetType()
                        && topic.Name == name)?.Raise(sender, eventArgs);
            }
            catch {
                throw new Exception("Exception was thrown on an" +
                        " invoked topic event");
            }
        }
        public void Subscribe(EventArgs eventArgs,
                string name, Action<object, EventArgs> action) {
            foreach (List<Topic> topics in _topicsByPublisher.Values) {
                foreach (Topic topic in topics) {
                    if (topic.Name == name &&
                        topic.EventArgsType == eventArgs.GetType()) {
                        topic.Actions.Add(action!);
                    }
                }
            }
        }
        public void Unsubscribe(EventArgs eventArgs, string name,
            Action<object, EventArgs> action) {
            foreach (List<Topic> topics in _topicsByPublisher.Values) {
                foreach (Topic topic in topics) {
                    if (topic.Name == name && eventArgs.GetType() == topic
                            .EventArgsType) {
                        topic.Actions.Remove(action!);
                        return;
                    }
                }
            }
        }
        public void CreateTopic(object sender, EventArgs eventArgs, string name) {
            if (_topicsByPublisher.ContainsKey(sender)) {
                _topicsByPublisher[sender].Add(new Topic(eventArgs, name));
                return;
            }
            _topicsByPublisher.Add(sender, new List<Topic>());
            if (TopicExists(sender, eventArgs, name)) {
                throw new Exception("Topic already exists");
            }
            _topicsByPublisher[sender].Add(new Topic(eventArgs, name));
        }
        public void DeleteTopic(object sender, EventArgs eventArgs,
                string name) {
            if (!TopicExists(sender, eventArgs, name)) {
                throw new Exception("Topic does not exist!");
            }
            foreach (var topic in GetTopics(sender).Where(topic =>
                    topic.Name == name &&
                    eventArgs.GetType() == topic.EventArgsType)) {
                _topicsByPublisher[sender].Remove(topic);
                return;
            }
            throw new Exception("Topic does not exist!");
        }
        private List<Topic> GetTopics(object sender) {
            if (_topicsByPublisher.ContainsKey(sender)) {
                return _topicsByPublisher[sender];
            }
            throw new ArgumentException("Sender does not exist in dictionary");
        }
        private bool TopicExists(object sender, EventArgs args, string name) {
            return _topicsByPublisher[sender].Any(topic =>
                    topic.EventArgsType == args.GetType() &&
                    topic.Name == name);
        }
    }
}