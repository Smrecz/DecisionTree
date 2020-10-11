using System;
using System.Reflection;

namespace DecisionTree.Exceptions
{
    public class NotPrintableTypeException : Exception
    {
        public NotPrintableTypeException(MemberInfo type) 
            : base($"Printing of type {type.Name} not supported.") { }
    }
}
