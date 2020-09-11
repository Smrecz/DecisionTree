using System;
using DecisionTree.Exceptions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class DecisionBase : ITitled
    {
        protected DecisionBase(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }

        internal void ChangeTitle(string newTitle)
        {
            Title = newTitle;
        }

        internal Exception GetEvaluationException(Exception exception)
        {
            if (exception is DecisionEvaluationException)
            {
                var newMessage = $"{exception.Message}" +
                                 $"{Environment.NewLine}^-- '{Title}'";

                return new DecisionEvaluationException(newMessage, exception.InnerException);
            }

            var message = "Decision evaluation failed (check inner exception for details)." +
                          $"{Environment.NewLine}Full decision tree path:" +
                          $"{Environment.NewLine}X-- '{Title}' (FAILED)";

            return new DecisionEvaluationException(message, exception);
        }
    }
}
