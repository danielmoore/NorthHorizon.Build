using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace NorthHorizon.Build.Tasks
{
    public static class FileHashProvider
    {
        public static byte[] HashFiles(string hashName, IEnumerable<string> filePaths)
        {
            var fileStreams = filePaths.OrderBy(p => p, StringComparer.OrdinalIgnoreCase).Select(File.OpenRead);

            using (var hash = HashAlgorithm.Create(hashName))
            using (var stream = new ConcatenatedStream(fileStreams))
            {
                if (hash == null)
                    throw new ArgumentException("Unknown hash: " + hashName, "hashName");

                return hash.ComputeHash(stream);
            }
        }
    }
}