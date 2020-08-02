using System;

namespace DecisionTree.Decisions
{
    public class DecisionResult<T> : IDecision<T>
    {
        public DecisionResult(string title, Action<T> action = null)
        {
            _action = action;
            Title = title;
        }

        public string Title { get; }
        
        private readonly Action<T> _action;

        public void Evaluate(T dto) => _action?.Invoke(dto);
    }
}
