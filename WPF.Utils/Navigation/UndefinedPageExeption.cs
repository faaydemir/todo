using System;
using System.Runtime.Serialization;

namespace WpfUtils.Navigation
{
    [Serializable]
    internal class UndefinedPageExeption : Exception
    {
        public UndefinedPageExeption()
        {
        }

        public UndefinedPageExeption(string message) : base(message)
        {
        }

        public UndefinedPageExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UndefinedPageExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}