using DecisionTree.Builders.Interface;
using DecisionTree.Decisions;
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
            GetNodeBuilder<bool>()
                .AddTitle(nameof(BudgetDecision))
                .AddCondition(dto => dto.Project.BudgetRemaining < dto.Project.ItemsToDo * 1000)
                .AddPath(true, RequestBudgetResult)
                .AddPath(false, DoNothingResult)
                .AddAction(dto => dto.SetIsBudgetReviewed(true))
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> DeadlineDecision =
            GetNodeBuilder<bool>()
                .AddTitle(nameof(DeadlineDecision))
                .AddCondition(dto => dto.Project.TimeToDeadline.Days < 7)
                .AddPath(true, MoveDeadlineResult)
                .AddPath(false, BudgetDecision)
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> ToDoDecision =
            GetNodeBuilder<bool>()
                .AddTitle(nameof(ToDoDecision))
                .AddCondition(dto => dto.Project.ItemsToDo > 10)
                .AddPath(true, DeadlineDecision)
                .AddPath(false, BudgetDecision)
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
            GetNodeBuilder<ProjectType>()
                .AddTitle(nameof(ProjectTypeDecision))
                .AddCondition(dto => dto.Project.Type)
                .AddPath(ProjectType.Internal, ResetInternalAction, SendNotificationAction)
                .AddDefault(ToDoDecision)
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> IsOnHoldDecision =
            GetNodeBuilder<bool>()
                .AddTitle(nameof(IsOnHoldDecision))
                .AddCondition(dto => dto.Project.IsOnHold)
                .AddPath(true, DoNothingResult)
                .AddPath(false, ProjectTypeDecision)
                .Build();

        private static readonly IDecision<ItProjectDecisionDto> FinishedDecision =
            GetNodeBuilder<bool>()
                .AddTitle(nameof(FinishedDecision))
                .AddCondition(dto => dto.Project.ItemsToDo == 0)
                .AddPath(true, FinishResult)
                .AddPath(false, IsOnHoldDecision, SendNotificationAction)
                .Build();

        public override IDecision<ItProjectDecisionDto> GetTrunk() => FinishedDecision;
    }
}
