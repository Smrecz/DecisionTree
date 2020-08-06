using DecisionTree.Builders;
using DecisionTree.Decisions;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Model;
using DecisionTree.Tree;

namespace DecisionTree.Tests.Tree
{
    public class ProjectDecisionTree : DecisionTreeBase<ItProjectDecisionDto, bool>
    {
        private static readonly DecisionResult<ItProjectDecisionDto> DoNothingResult =
            new DecisionResultBuilder<ItProjectDecisionDto>()
                .AddTitle(nameof(DoNothingResult))
                .Build();

        private static readonly DecisionResult<ItProjectDecisionDto> FinishResult =
            new DecisionResultBuilder<ItProjectDecisionDto>()
                .AddTitle(nameof(FinishResult))
                .AddAction(dto => dto.SetResult("Project is finished."))
                .Build();

        private static readonly DecisionResult<ItProjectDecisionDto> RequestBudgetResult =
            new DecisionResultBuilder<ItProjectDecisionDto>()
                .AddTitle(nameof(RequestBudgetResult))
                .AddAction(dto => 
                    dto
                    .SetResult("Not enough funds.")
                    .SetIsOnHold(true))
                .Build();

        private static readonly DecisionResult<ItProjectDecisionDto> MoveDeadlineResult =
            new DecisionResultBuilder<ItProjectDecisionDto>()
                .AddTitle(nameof(MoveDeadlineResult))
                .AddAction(dto => dto.SetResult("Timeline reevaluation needed."))
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> BudgetDecision =
            new DecisionNodeBuilder<ItProjectDecisionDto, bool>()
                .AddCondition(dto => dto.Project.BudgetRemaining < dto.Project.ItemsToDo * 1000)
                .AddPath(true, RequestBudgetResult)
                .AddPath(false, DoNothingResult)
                .AddAction(dto => dto.SetIsBudgetReviewed(true))
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> DeadlineDecision =
            new DecisionNodeBuilder<ItProjectDecisionDto, bool>()
                .AddCondition(dto => dto.Project.TimeToDeadline.Days < 7)
                .AddPath(true, MoveDeadlineResult)
                .AddPath(false, BudgetDecision)
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> ToDoDecision =
            new DecisionNodeBuilder<ItProjectDecisionDto, bool>()
                .AddCondition(dto => dto.Project.ItemsToDo > 10)
                .AddPath(true, DeadlineDecision)
                .AddPath(false, BudgetDecision)
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, ProjectType> ProjectTypeDecision =
            new DecisionNodeBuilder<ItProjectDecisionDto, ProjectType>()
                .AddCondition(dto => dto.Project.Type)
                .AddPath(ProjectType.Internal, DoNothingResult)
                .AddDefault(ToDoDecision)
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> IsOnHoldDecision =
            new DecisionNodeBuilder<ItProjectDecisionDto, bool>()
                .AddCondition(dto => dto.Project.IsOnHold)
                .AddPath(true, DoNothingResult)
                .AddPath(false, ProjectTypeDecision)
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> FinishedDecision =
            new DecisionNodeBuilder<ItProjectDecisionDto, bool>()
                .AddCondition(dto => dto.Project.ItemsToDo == 0)
                .AddPath(true, FinishResult)
                .AddPath(false, IsOnHoldDecision)
                .Build();

        public override DecisionNode<ItProjectDecisionDto, bool> GetTrunk() => FinishedDecision;
    }
}
