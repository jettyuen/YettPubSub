using System;
using System.Collections.Generic;
namespace YettJohan.PubSub {
    public class EventHub {
        private readonly Dictionary<object, Dictionary<string, Topic>>
                _topicsByPublisher = new();
        private readonly Dictionary<Type, Dictionary<string, Topic>>
                _topicsByType = new();
        public void Publish<T>(object sender, string name, T args) {
            if (!TopicExists(sender, name)) {
                throw new ArgumentException($"{name} does not exist!");
            }
            Raise(sender, name, args);
        }
        private void Raise<T>(object sender, string name, T args) {
            var actions = _topicsByPublisher[sender][name].Actions;
            foreach (var inputAction in actions) {
                (inputAction as Action<T>)!.Invoke(args);
            }
        }
        public void Subscribe<T>(string name, Action<T> action) {
            if (!TopicExists<T>(name)) {
                throw new ArgumentException(
                    $"{name} does not exist or type does not match" +
                    $" {name}'s type!");
            }
            _topicsByType[typeof(T)][name].Actions.Add(action);
        }
        public void Unsubscribe<T>(string name,
                Action<T?> action) {
            if (!TopicExists<T>(name)) {
                throw new ArgumentException(
                    $"{name} does not exist or type does not match +" +
                    $"{name}'s type!");
            }
            var castedAction = _topicsByType[typeof(T)][name]
                    .Actions.Find(obj =>
                            obj is Action<T> action1 && action1 == action);
            _topicsByType[typeof(T)][name].Actions.Remove(castedAction);
        }
        public void CreateTopic<T>(object sender, string name) {
            if (TopicExists(sender, name)) {
                throw new ArgumentException($"{name} already exists!");
            }
            Topic topic = new();
            if (_topicsByPublisher.ContainsKey(sender)) {
                _topicsByPublisher[sender].Add(name, topic);
            }
            else if (!_topicsByPublisher.ContainsKey(sender)) {
                _topicsByPublisher.Add(sender, new Dictionary<string, Topic>
                        { { name, topic } });
            }
            if (_topicsByType.ContainsKey(typeof(T))) {
                _topicsByType[typeof(T)].Add(name, topic);
            }
            else if (!_topicsByType.ContainsKey(typeof(T))) {
                _topicsByType.Add(typeof(T), new Dictionary<string, Topic> {
                        { name, topic }
                });
            }
        }
        public void DeleteTopic<T>(object sender, string name) {
            if (!TopicExists(sender, name)) {
                throw new ArgumentException("Topic does not exist!");
            }
            _topicsByPublisher[sender].Remove(name);
            _topicsByType[typeof(T)].Remove(name);
        }
        private bool TopicExists(object sender, string name) {
            return _topicsByPublisher.ContainsKey(sender) &&
                    _topicsByPublisher[sender].ContainsKey(name);
        }
        private bool TopicExists<T>(string name) {
            return _topicsByType.ContainsKey(typeof(T)) &&
                    _topicsByType[typeof(T)].ContainsKey(name);
        }
    }
}