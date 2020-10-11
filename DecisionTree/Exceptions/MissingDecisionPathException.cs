using System;

namespace DecisionTree.Exceptions
{
    public class MissingDecisionPathException : Exception
    {
        public MissingDecisionPathException(string result) 
            : base($"Decision path not defined for result: {result}") { }
    }
}
