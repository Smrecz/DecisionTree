using DecisionTree.Builders;
using DecisionTree.Decisions;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Model;
using DecisionTree.Tree;

namespace DecisionTree.Tests.Tree
{
    public class ProjectDecisionTree : DecisionTreeBase<ItProjectDecisionDto>
    {
        private static readonly DecisionResult<ItProjectDecisionDto> DoNothingResult =
            DecisionResultBuilder<ItProjectDecisionDto>
                .Create()
                .AddTitle(nameof(DoNothingResult))
                .Build();

        private static readonly DecisionResult<ItProjectDecisionDto> FinishResult =
            DecisionResultBuilder<ItProjectDecisionDto>
                .Create()
                .AddTitle(nameof(FinishResult))
                .AddAction(dto => dto.SetResult("Project is finished."))
                .Build();

        private static readonly DecisionResult<ItProjectDecisionDto> RequestBudgetResult =
            DecisionResultBuilder<ItProjectDecisionDto>
                .Create()
                .AddTitle(nameof(RequestBudgetResult))
                .AddAction(dto => dto
                    .SetResult("Not enough funds.")
                    .SetIsOnHold(true))
                .Build();

        private static readonly DecisionResult<ItProjectDecisionDto> MoveDeadlineResult =
            DecisionResultBuilder<ItProjectDecisionDto>
                .Create()
                .AddTitle(nameof(MoveDeadlineResult))
                .AddAction(dto => dto.SetResult("Timeline reevaluation needed."))
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> BudgetDecision =
            DecisionNodeBuilder<ItProjectDecisionDto, bool>
                .Create()
                .AddTitle(nameof(BudgetDecision))
                .AddCondition(dto => dto.Project.BudgetRemaining < dto.Project.ItemsToDo * 1000)
                .AddPath(true, RequestBudgetResult)
                .AddPath(false, DoNothingResult)
                .AddAction(dto => dto.SetIsBudgetReviewed(true))
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> DeadlineDecision =
            DecisionNodeBuilder<ItProjectDecisionDto, bool>
                .Create()
                .AddTitle(nameof(DeadlineDecision))
                .AddCondition(dto => dto.Project.TimeToDeadline.Days < 7)
                .AddPath(true, MoveDeadlineResult)
                .AddPath(false, BudgetDecision)
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> ToDoDecision =
            DecisionNodeBuilder<ItProjectDecisionDto, bool>
                .Create()
                .AddTitle(nameof(ToDoDecision))
                .AddCondition(dto => dto.Project.ItemsToDo > 10)
                .AddPath(true, DeadlineDecision)
                .AddPath(false, BudgetDecision)
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, ProjectType> ProjectTypeDecision =
            DecisionNodeBuilder<ItProjectDecisionDto, ProjectType>
                .Create()
                .AddTitle(nameof(ProjectTypeDecision))
                .AddCondition(dto => dto.Project.Type)
                .AddPath(ProjectType.Internal, DoNothingResult)
                .AddDefault(ToDoDecision)
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> IsOnHoldDecision =
            DecisionNodeBuilder<ItProjectDecisionDto, bool>
                .Create()
                .AddTitle(nameof(IsOnHoldDecision))
                .AddCondition(dto => dto.Project.IsOnHold)
                .AddPath(true, DoNothingResult)
                .AddPath(false, ProjectTypeDecision)
                .Build();

        private static readonly DecisionNode<ItProjectDecisionDto, bool> FinishedDecision =
            DecisionNodeBuilder<ItProjectDecisionDto, bool>
                .Create()
                .AddTitle(nameof(FinishedDecision))
                .AddCondition(dto => dto.Project.ItemsToDo == 0)
                .AddPath(true, FinishResult)
                .AddPath(false, IsOnHoldDecision)
                .Build();

        public override IDecision<ItProjectDecisionDto> GetTrunk() => FinishedDecision;
    }
}
