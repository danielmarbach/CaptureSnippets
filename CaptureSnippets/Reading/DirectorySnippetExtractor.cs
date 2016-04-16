using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;

namespace CaptureSnippets
{
    /// <summary>
    /// Extracts <see cref="ReadSnippet"/>s from a given directory.
    /// </summary>
    public class DirectorySnippetExtractor
    {
        ExtractMetaData extractMetaData;
        IncludeDirectory includeDirectory;
        IncludeFile includeFile;
        FileSnippetExtractor fileExtractor;

        /// <summary>
        /// Initialise a new instance of <see cref="DirectorySnippetExtractor"/>.
        /// </summary>
        /// <param name="extractMetaData">How to extract a <see cref="SnippetMetaData"/> from a given path.</param>
        /// <param name="includeFile">Used to filter files.</param>
        /// <param name="translatePackage">How to translate a package alias to the full package name.</param>
        /// <param name="includeDirectory">Used to filter directories.</param>
        public DirectorySnippetExtractor(ExtractMetaData extractMetaData, IncludeDirectory includeDirectory, IncludeFile includeFile, TranslatePackage translatePackage = null)
        {
            Guard.AgainstNull(includeDirectory, "includeDirectory");
            Guard.AgainstNull(includeFile, "includeFile");
            Guard.AgainstNull(extractMetaData, "extractMetaData");
            this.extractMetaData = extractMetaData;
            this.includeDirectory = includeDirectory;
            this.includeFile = includeFile;
            fileExtractor = new FileSnippetExtractor(extractMetaData, translatePackage);
        }

        [Time]
        public async Task<ReadSnippets> FromDirectory(string directoryPath)
        {
            Guard.AgainstNull(directoryPath, "directoryPath");
            var snippets = new ConcurrentBag<ReadSnippet>();
            await Task.WhenAll(FromDirectory(directoryPath, snippets.Add))
                .ConfigureAwait(false);
            var readOnlyList = snippets.ToList();
            return new ReadSnippets(readOnlyList);
        }


        IEnumerable<Task> FromDirectory(string directoryPath, Action<ReadSnippet> add)
        {
            var cache = new Dictionary<string, SnippetMetaData>();
            foreach (var subDirectory in Extensions.AllDirectories(directoryPath, includeDirectory)
                .Where(s => includeDirectory(s)))
            {
                var parent = Directory.GetParent(subDirectory).FullName;
                SnippetMetaData parentInfo;
                cache.TryGetValue(parent, out parentInfo);
                var metaData = extractMetaData(subDirectory, parentInfo);
                cache.Add(subDirectory, metaData);
                foreach (var file in Directory.EnumerateFiles(subDirectory)
                    .Where(s => includeFile(s)))
                {
                    yield return FromFile(file, metaData, add);
                }
            }
        }

        async Task FromFile(string file, SnippetMetaData metaData, Action<ReadSnippet> callback)
        {
            using (var textReader = File.OpenText(file))
            {
                await fileExtractor.AppendFromReader(textReader, file, metaData, callback)
                    .ConfigureAwait(false);
            }
        }

    }
}