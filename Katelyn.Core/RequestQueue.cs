using Katelyn.Core.LinkParsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Katelyn.Core
{
    public class QueueItem
    {
        public Uri Address { get; set; }

        public Uri ParentAddress { get; set; }
    }

    public class RequestQueue
    {
        private IDictionary<string, QueueItem> _queue = new Dictionary<string, QueueItem>();
        private readonly IDictionary<string, int> _completed = new Dictionary<string, int>();

        public void Add(Uri uri, Uri parent)
        {
            if (_completed.ContainsKey(uri.AbsoluteUri))
            {
                _completed[uri.AbsoluteUri]++;
                return;
            }

            if (_queue.ContainsKey(uri.AbsoluteUri))
            {
                return;
            }

            _queue.Add(uri.AbsoluteUri, new QueueItem { Address = uri, ParentAddress = parent });
        }

        public void AddRange(ContentParser<Uri> uris, Uri parent)
        {
            foreach (Uri resource in uris)
            {
                Add(resource, parent);
            }
        }

        public void AddRootAddress(Uri rootAddress)
        {
            AddCompletedKey(rootAddress.AbsoluteUri);
        }

        public QueueItem Pop()
        {
            string key = _queue.Keys.FirstOrDefault();

            if (key == null)
            {
                return default(QueueItem);
            }

            AddCompletedKey(key);

            var uri = _queue[key];
            _queue.Remove(key);

            return uri;
        }

        private void AddCompletedKey(string key)
        {
            if (_completed.ContainsKey(key))
            {
                _completed[key]++;
            }
            else
            {
                _completed.Add(key, 1);
            }
        }
    }
}
