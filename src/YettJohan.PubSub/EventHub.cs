using System;
using System.Collections.Generic;
namespace YettJohan.PubSub {
    public class EventHub {
        private readonly Dictionary<object, List<Topic>>
                _topicsByPublisher = new();
        private readonly Dictionary<Type, List<Topic>>
                _topicsByType = new();
        public void Publish<T>(object sender, string name, T? args) {
            if (!TopicExists(sender, name)) {
                throw new ArgumentException($"{name} does not exist!");
            }
            Raise(sender, name, args);
        }
        private void Raise<T>(object sender, string name, T args) {
            foreach (var topic in _topicsByPublisher[sender]) {
                if (topic.Name != name) {
                    continue;
                }
                var actions = topic.Actions;
                foreach (var unknownAction in actions) {
                    var castedAction = unknownAction as Action<T>;
                    castedAction!.Invoke(args);
                }
            }
        }
        public void Subscribe<T>(string name, Action<T?> action) {
            if (!TopicExists<T>(name)) {
                throw new ArgumentException(
                    $"{name} does not exist or type does not match" +
                    $" {name}'s type!");
            }
            foreach (var topic in _topicsByType[typeof(T)]) {
                if (topic.Name != name) {
                    continue;
                }
                topic.Actions.Add(action);
                return;
            }
        }
        public void Unsubscribe<T>(string name,
                Action<T?> action) {
            if (!TopicExists<T>(name)) {
                throw new ArgumentException(
                    $"{name} does not exist or type does not match +" +
                    $"{name}'s type!");
            }
            foreach (var topic in _topicsByType[typeof(T)]) {
                if (topic.Name != name) {
                    continue;
                }
                foreach (var topicAction in topic.Actions) {
                    var castedAction = topicAction as Action<T?>;
                    if (ReferenceEquals(castedAction, action)) {
                        topic.Actions.Remove(topicAction);
                        return;
                    }
                }
            }
        }
        public void CreateTopic<T>(object sender, string name) {
            if (TopicExists(sender, name)) {
                throw new ArgumentException($"{name} already exists!");
            }
            Topic topic;
            if (_topicsByPublisher.ContainsKey(sender)) {
                topic = new Topic(name);
                _topicsByPublisher[sender].Add(topic);
                if (_topicsByType.ContainsKey(typeof(T?))) {
                    _topicsByType[typeof(T)].Add(topic);
                }
                else {
                    _topicsByType.Add(typeof(T), new List<Topic> { topic });
                }
                return;
            }
            topic = new Topic(name);
            _topicsByPublisher.Add(sender, new List<Topic> { topic });
            _topicsByType.Add(typeof(T), new List<Topic> { topic });
        }
        public void DeleteTopic<T>(object sender, string name) {
            if (!TopicExists(sender, name)) {
                throw new ArgumentException("Topic does not exist!");
            }
            foreach (var topic in GetTopics(sender)) {
                if (topic.Name == name) {
                    _topicsByPublisher[sender].Remove(topic);
                    _topicsByType[typeof(T)].Remove(topic);
                    return;
                }
            }
        }
        private List<Topic> GetTopics(object sender) {
            if (_topicsByPublisher.ContainsKey(sender)) {
                return _topicsByPublisher[sender];
            }
            throw new ArgumentException("Sender does not exist in dictionary");
        }
        private bool TopicExists(object sender, string name) {
            if (!_topicsByPublisher.ContainsKey(sender)) {
                return false;
            }
            foreach (var topic in _topicsByPublisher[sender]) {
                if (topic.Name == name) {
                    return true;
                }
            }
            return false;
        }
        private bool TopicExists<T>(string name) {
            if (!_topicsByType.ContainsKey(typeof(T))) {
                return false;
            }
            foreach (var topic in _topicsByType[typeof(T)]) {
                if (topic.Name == name) {
                    return true;
                }
            }
            return false;
        }
    }
}