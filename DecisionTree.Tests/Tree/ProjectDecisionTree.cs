using DecisionTree.Builders.Interface.Action;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Model;
using DecisionTree.Tree;

namespace DecisionTree.Tests.Tree
{
    public class ProjectDecisionTree : DecisionTreeBase<ItProjectDecisionDto>
    {
        private static readonly IActionPath<ItProjectDecisionDto> SendNotificationAction =
            GetActionBuilder()
                .AddTitle(nameof(SendNotificationAction))
                .AddAction(dto => dto.SetSendNotification(true));

        private static readonly IDecision<ItProjectDecisionDto> DoNothingResult =
            GetResultBuilder()
                .AddTitle(nameof(DoNothingResult))
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> FinishResult =
            GetResultBuilder()
                .AddTitle(nameof(FinishResult))
                .AddAction(dto => dto.SetResult("Project is finished."))
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> RequestBudgetResult =
            GetResultBuilder()
                .AddTitle(nameof(RequestBudgetResult))
                .AddAction(dto => dto
                    .SetResult("Not enough funds.")
                    .SetIsOnHold(true))
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> MoveDeadlineResult =
            GetResultBuilder()
                .AddTitle(nameof(MoveDeadlineResult))
                .AddAction(dto => dto.SetResult("Timeline reevaluation needed."))
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> BudgetDecision =
            GetBinaryNodeBuilder()
                .AddTitle(nameof(BudgetDecision))
                .AddCondition(dto => dto.Project.BudgetRemaining < dto.Project.ItemsToDo * 1000)
                .AddPositivePath(RequestBudgetResult)
                .AddNegativePath(DoNothingResult)
                .AddAction(dto => dto.SetIsBudgetReviewed(true))
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> DeadlineDecision =
            GetBinaryNodeBuilder()
                .AddTitle(nameof(DeadlineDecision))
                .AddCondition(dto => dto.Project.TimeToDeadline.Days < 7)
                .AddPositivePath(MoveDeadlineResult, SendNotificationAction)
                .AddNegativePath(BudgetDecision)
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> ToDoDecision =
            GetBinaryNodeBuilder()
                .AddTitle(nameof(ToDoDecision))
                .AddCondition(dto => dto.Project.ItemsToDo > 10)
                .AddPositivePath(DeadlineDecision)
                .AddNegativePath(BudgetDecision)
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> ResetInternalAction =
            GetActionBuilder()
                .AddTitle(nameof(ResetInternalAction))
                .AddAction(dto => dto
                    .SetItemsToDo(0)
                    .SetBudgetRemaining(0))
                .AddPath(DoNothingResult)
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> ProjectTypeDecision =
            GetNodeBuilder<ProjectType?>()
                .AddTitle(nameof(ProjectTypeDecision))
                .AddCondition(dto => dto.Project.Type)
                .AddPath(ProjectType.Internal, ResetInternalAction, SendNotificationAction)
                .AddDefault(ToDoDecision)
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> IsOnHoldDecision =
            GetBinaryNodeBuilder()
                .AddTitle(nameof(IsOnHoldDecision))
                .AddCondition(dto => dto.Project.IsOnHold)
                .AddPositivePath(DoNothingResult)
                .AddNegativePath(ProjectTypeDecision)
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> FinishedDecision =
            GetBinaryNodeBuilder()
                .AddTitle(nameof(FinishedDecision))
                .AddCondition(dto => dto.Project.ItemsToDo == 0)
                .AddPositivePath(FinishResult)
                .AddNegativePath(IsOnHoldDecision, SendNotificationAction)
                .Build();

        public override IDecision<ItProjectDecisionDto> GetTrunk() => FinishedDecision;
    }
}
