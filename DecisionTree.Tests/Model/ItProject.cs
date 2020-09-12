using System;

namespace DecisionTree.Tests.Model
{
    public class ItProject
    {
        public ProjectType? Type { get; set; }
        public ProjectSubType? SubType { get; set; }
        public ProjectArea? Area { get; set; }
        public bool IsOnHold { get; set; }
        public bool IsBudgetReviewed { get; set; }
        public int ItemsToDo { get; set; }
        public int BudgetRemaining { get; set; }
        public TimeSpan TimeToDeadline { get; set; }
        public bool SendNotification { get; set; }
    }
}
