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

        protected string DecisionExceptionMessage => 
            $"Title: '{Title}' - Type: {GetType().Name} - Decision evaluation failed, check inner exception for details.";
    }
}
