using System.Runtime.Serialization;

namespace EStore.Common.Exceptions
{
    public class KSEOperationFailedException : Exception
    {
        public KSEOperationFailedException()
        {
        }

        public KSEOperationFailedException(string? message) : base(message)
        {
        }

        public KSEOperationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected KSEOperationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
