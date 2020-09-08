using DecisionTree.Tests.Model;

namespace DecisionTree.Tests.Dto
{
    public class ItProjectDecisionDto
    {
        public ItProject Project { get; set; }
        public string Result { get; set; }

        public virtual ItProjectDecisionDto SetResult(string value)
        {
            Result = value;
            return this;
        }

        public virtual ItProjectDecisionDto SetIsBudgetReviewed(bool value)
        {
            Project.IsBudgetReviewed = value;
            return this;
        }

        public virtual ItProjectDecisionDto SetIsOnHold(bool value)
        {
            Project.IsOnHold = value;
            return this;
        }

        public virtual ItProjectDecisionDto SetBudgetRemaining(int value)
        {
            Project.BudgetRemaining = value;
            return this;
        }

        public virtual ItProjectDecisionDto SetItemsToDo(int value)
        {
            Project.ItemsToDo = value;
            return this;
        }

        public virtual ItProjectDecisionDto SetSendNotification(bool value)
        {
            Project.SendNotification = value;
            return this;
        }
    }
}
