namespace ThunderFighter.Common.Exceptions
{
    using System;

    public class NameFormatException : ArgumentException
    {
        public const int MinNameLength = 3;
        public const int MaxNameLength = 10;

        public NameFormatException(string message, string name)
        {
        }

        public NameFormatException(string name) : this(string.Format("Name must be between {0} and {1} symbols long!", MinNameLength, MaxNameLength), name)
        {
        }

        public NameFormatException(string message, string name, Exception innerException) : base(message, name, innerException)
        {
        }
    }
}
