using System;
using System.Runtime.Serialization;

namespace Sitecore.Foundation.MinifyHtml.Exceptions
{
    [Serializable]
    public sealed class MinifierException : Exception
    {
        public MinifierException() : base()
        {
        }

        private MinifierException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MinifierException(string message) : base(message)
        {
        }
    }
}