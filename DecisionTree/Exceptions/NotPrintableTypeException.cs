using System;

namespace DecisionTree.Exceptions
{
    public class NotPrintableTypeException : Exception
    {
        public NotPrintableTypeException(string message) : base(message) { }
    }
}
