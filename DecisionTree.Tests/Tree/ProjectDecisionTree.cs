using DecisionTree.Builders;
using DecisionTree.Decisions;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Model;
using DecisionTree.Tree;

namespace DecisionTree.Tests.Tree
{
    public class ProjectDecisionTree<T> : DecisionTreeBase<T, bool>
        where T : ItProjectDecisionDto
    {
        private static readonly DecisionResult<T> DoNothingResult =
            new DecisionResultBuilder<T>()
                .AddTitle(nameof(DoNothingResult))
                .Build();

        private static readonly DecisionResult<T> FinishResult =
            new DecisionResultBuilder<T>()
                .AddTitle(nameof(FinishResult))
                .AddAction(dto => dto.Result = "Project is finished.")
                .Build();

        private static readonly DecisionResult<T> RequestBudgetResult =
            new DecisionResultBuilder<T>()
                .AddTitle(nameof(RequestBudgetResult))
                .AddAction(dto => dto.Result = "Need more money.")
                .Build();

        private static readonly DecisionResult<T> MoveDeadlineResult =
            new DecisionResultBuilder<T>()
                .AddTitle(nameof(MoveDeadlineResult))
                .AddAction(dto => dto.Result = "We are not going to make it.")
                .Build();

        private static readonly DecisionNode<T, bool> BudgetDecision =
            new DecisionNodeBuilder<T, bool>()
                .AddCondition(dto => dto.Project.BudgetRemaining < dto.Project.ItemsToDo * 1000)
                .AddPath(true, RequestBudgetResult)
                .AddPath(false, DoNothingResult)
                .Build();

        private static readonly DecisionNode<T, bool> DeadlineDecision =
            new DecisionNodeBuilder<T, bool>()
                .AddCondition(dto => dto.Project.TimeToDeadline.Days < 7)
                .AddPath(true, MoveDeadlineResult)
                .AddPath(false, BudgetDecision)
                .Build();

        private static readonly DecisionNode<T, bool> ToDoDecision =
            new DecisionNodeBuilder<T, bool>()
                .AddCondition(dto => dto.Project.ItemsToDo > 10)
                .AddPath(true, DeadlineDecision)
                .AddPath(false, BudgetDecision)
                .Build();

        private static readonly DecisionNode<T, ProjectType> ProjectTypeDecision =
            new DecisionNodeBuilder<T, ProjectType>()
                .AddCondition(dto => dto.Project.Type)
                .AddPath(ProjectType.Internal, DoNothingResult)
                .AddDefault(ToDoDecision)
                .Build();

        private static readonly DecisionNode<T, bool> IsOnHoldDecision =
            new DecisionNodeBuilder<T, bool>()
                .AddCondition(dto => dto.Project.IsOnHold)
                .AddPath(true, DoNothingResult)
                .AddPath(false, ProjectTypeDecision)
                .Build();

        private static readonly DecisionNode<T, bool> FinishedDecision =
            new DecisionNodeBuilder<T, bool>()
                .AddCondition(dto => dto.Project.ItemsToDo == 0)
                .AddPath(true, FinishResult)
                .AddPath(false, IsOnHoldDecision)
                .Build();

        public override DecisionNode<T, bool> Trunk() => FinishedDecision;
    }
}
