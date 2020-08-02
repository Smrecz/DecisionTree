using System;

namespace DecisionTree.Exceptions
{
    public class MissingDecisionPathException : Exception
    {
        public MissingDecisionPathException(string message) : base(message)
        {

        }
    }
}
