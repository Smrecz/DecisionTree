using System;

namespace DecisionTree.Exceptions
{
    public class DecisionEvaluationException : Exception
    {
        public DecisionEvaluationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
