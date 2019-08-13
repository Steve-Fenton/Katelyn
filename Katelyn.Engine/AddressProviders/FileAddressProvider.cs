using System;
using System.Collections.Generic;
using System.IO;

namespace Katelyn.Core
{
    public class FileAddressProvider
        : AddressProvider
    {
        private readonly string _filePath;

        public FileAddressProvider(string filePath)
        {
            _filePath = filePath;
        }

        public override IEnumerator<Uri> GetEnumerator()
        {
            foreach (var line in File.ReadLines(_filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                yield return new Uri(line);
            }
        }
    }
}