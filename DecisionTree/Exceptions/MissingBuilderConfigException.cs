using System;

namespace DecisionTree.Exceptions
{
    public class MissingBuilderConfigException : Exception
    {
        public MissingBuilderConfigException(string message) : base(message) { }
    }
}
