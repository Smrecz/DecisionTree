using System;
using DecisionTree.Exceptions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class DecisionBase
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

        internal void HandleEvaluationException(Exception exception)
        {
            if (exception is DecisionEvaluationException)
            {
                var newMessage = $"{exception.Message}" +
                                 $"{Environment.NewLine}^-- '{Title}'";

                throw new DecisionEvaluationException(newMessage, exception.InnerException);
            }

            var message = "Decision evaluation failed (check inner exception for details)." +
                          $"{Environment.NewLine}Full decision tree path:" +
                          $"{Environment.NewLine}X-- '{Title}' (FAILED)";

            throw new DecisionEvaluationException(message, exception);
        }
    }
}
