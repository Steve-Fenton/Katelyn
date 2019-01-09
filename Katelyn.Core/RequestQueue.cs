using Katelyn.Core.LinkParsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Katelyn.Core
{
    public class RequestQueue
    {
        private IDictionary<string, Uri> _queue = new Dictionary<string, Uri>();
        private readonly IDictionary<string, int> _completed = new Dictionary<string, int>();

        public void Add(Uri uri)
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

            _queue.Add(uri.AbsoluteUri, uri);
        }

        public void AddRange(ContentParser<Uri> uris)
        {
            foreach (Uri resource in uris)
            {
                Add(resource);
            }
        }

        public void AddRootAddress(Uri rootAddress)
        {
            AddCompletedKey(rootAddress.AbsoluteUri);
        }

        public Uri Pop()
        {
            string key = _queue.Keys.FirstOrDefault();

            if (key == null)
            {
                return default(Uri);
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
