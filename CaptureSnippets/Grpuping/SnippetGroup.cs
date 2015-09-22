using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CaptureSnippets
{
    /// <summary>
    /// A hierarchy of <see cref="SnippetSource"/>s grouped by Key > Version
    /// </summary>
    public class SnippetGroup : IEnumerable<VersionGroup>
    {
        /// <summary>
        /// Initialise a new insatnce of <see cref="SnippetGroup"/>.
        /// </summary>
        public SnippetGroup(string key, string language, IEnumerable<VersionGroup> versions)
        {
            Guard.AgainstNull(key, "key");
            Guard.AgainstNull(versions, "versions");
            Guard.AgainstNull(language, "language");
            Guard.AgainstUpperCase(language, "language");
            Key = key;
            Versions = versions.ToList();
            Language = language;
        }

        /// <summary>
        /// The key that all child <see cref="VersionGroup"/>s contain.
        /// </summary>
        public readonly string Key;
        /// <summary>
        /// The language of the snippet, extracted from the file extension of the input file.
        /// </summary>
        public readonly string Language;

        /// <summary>
        /// All the <see cref="VersionGroup"/>s with a common key.
        /// </summary>
        public readonly IEnumerable<VersionGroup> Versions;

        /// <summary>
        /// Enumerates over <see cref="Versions"/>.
        /// </summary>
        public IEnumerator<VersionGroup> GetEnumerator()
        {
            return Versions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}