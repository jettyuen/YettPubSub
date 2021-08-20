/*
MIT License

Copyright(c)[year][fullname]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files(the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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