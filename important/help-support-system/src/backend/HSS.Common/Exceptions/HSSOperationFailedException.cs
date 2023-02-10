using System.Runtime.Serialization;

namespace HSS.Common.Exceptions
{
    public class HSSOperationFailedException : Exception
    {
        public HSSOperationFailedException()
        {
        }

        public HSSOperationFailedException(string? message) : base(message)
        {
        }

        public HSSOperationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected HSSOperationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
