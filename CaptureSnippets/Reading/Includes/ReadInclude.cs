using System;
using System.Diagnostics;
using NuGet.Versioning;

namespace CaptureSnippets
{
    /// <summary>
    /// A sub item of <see cref="ReadInclude"/>.
    /// </summary>
    [DebuggerDisplay("Key={Key}, Path={Path}, Error={Error}, Package={Package.ValueOrUndefined}")]
    public class ReadInclude
    {

        /// <summary>
        /// Initialise a new instance of <see cref="ReadInclude"/>.
        /// </summary>
        public ReadInclude(string key, string path, string error)
        {
            Guard.AgainstNullAndEmpty(key, "key");
            Guard.AgainstNullAndEmpty(error, "error");
            Key = key;
            IsInError = true;
            Path = path;
            Error = error;
        }

        /// <summary>
        /// Initialise a new instance of <see cref="ReadSnippet"/>.
        /// </summary>
        public ReadInclude(string key, string value, string path, VersionRange version, Package package)
        {
            Guard.AgainstNullAndEmpty(key, "key");
            Guard.AgainstUpperCase(key, "key");
            Guard.AgainstNull(package, "package");
            Value = value;
            ValueHash = value.RemoveWhitespace().GetHashCode();
            Key = key;
            Path = path;
            this.version = version;
            this.package = package;
        }

        public readonly string Error;

        public readonly bool IsInError;


        /// <summary>
        /// A hash of the <see cref="Value"/>.
        /// </summary>
        public readonly int ValueHash;

        /// <summary>
        /// The contents of the snippet
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// The key used to identify the snippet
        /// </summary>
        public readonly string Key;

        /// <summary>
        /// The path the snippet was read from.
        /// </summary>
        public readonly string Path;

        VersionRange version;

        Package package;

        /// <summary>
        /// The <see cref="VersionRange"/> that was inferred for the snippet.
        /// </summary>
        public VersionRange Version
        {
            get
            {
                if (IsInError)
                {
                    throw new Exception("Cannot access Version when IsInError.");
                }
                return version;
            }
        }

        /// <summary>
        /// The Package that was inferred for the snippet.
        /// </summary>
        public Package Package
        {
            get
            {
                if (IsInError)
                {
                    throw new Exception("Cannot access Package when IsInError.");
                }
                return package;
            }
        }
    }
}