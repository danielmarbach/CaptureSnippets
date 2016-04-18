using System;
using System.Collections.Generic;
using System.Text;

namespace CaptureSnippets
{
    public class GroupingException : Exception
    {
        public readonly IReadOnlyList<string> Errors;

        /// <summary>
        /// Initialise a new instance of <see cref="ReadSnippetsException"/>.
        /// </summary>
        public GroupingException(IReadOnlyList<string> errors)
        {
            Guard.AgainstNull(errors, "errors");
            Errors = errors;
        }

        public override string ToString()
        {
            return Message;
        }

        public override string Message
        {
            get
            {
                var stringBuilder = new StringBuilder("Errors occurred Grouping snippets:\r\n");
                foreach (var error in Errors)
                {
                    stringBuilder.AppendLine(error);
                }
                return stringBuilder.ToString();
            }
        }
    }
}