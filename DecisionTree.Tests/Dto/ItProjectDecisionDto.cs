using DecisionTree.Tests.Model;

namespace DecisionTree.Tests.Dto
{
    public class ItProjectDecisionDto
    {
        public ItProject Project { get; set; }
        public string Result { get; set; }

        public ItProjectDecisionDto SetResult(string value)
        {
            Result = value;
            return this;
        }

        public ItProjectDecisionDto SetIsBudgetReviewed(bool value)
        {
            Project.IsBudgetReviewed = value;
            return this;
        }

        public ItProjectDecisionDto SetIsOnHold(bool value)
        {
            Project.IsOnHold = value;
            return this;
        }
    }
}
