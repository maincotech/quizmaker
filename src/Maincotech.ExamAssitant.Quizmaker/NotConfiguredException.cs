using System;

namespace Maincotech.Quizmaker
{
    [Serializable]
    public class NotConfiguredException : Exception
    {
        public NotConfiguredException()
        {
        }

        public NotConfiguredException(string message) : base(message)
        {
        }

        public NotConfiguredException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NotConfiguredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}