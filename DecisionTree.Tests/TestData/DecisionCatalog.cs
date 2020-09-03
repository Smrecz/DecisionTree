using DecisionTree.Builders;
using DecisionTree.Builders.Interface.Action;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Tests.Dto;

namespace DecisionTree.Tests.TestData
{
    public class DecisionCatalog
    {
        public static readonly IDecisionResult<BoolDto> TrueResult =
            DecisionResultBuilder<BoolDto>.Create()
                .AddTitle(nameof(TrueResult))
                .AddAction(boolDto => boolDto.SetResult(true))
                .Build();

        public static readonly IDecisionResult<BoolDto> FalseResult =
            DecisionResultBuilder<BoolDto>.Create()
                .AddTitle(nameof(FalseResult))
                .AddAction(boolDto => boolDto.SetResult(false))
                .Build();

        public static readonly IDecisionResult<BoolDto> DefaultResult =
            DecisionResultBuilder<BoolDto>.Create()
                .AddTitle(nameof(DefaultResult))
                .AddAction(boolDto => boolDto.SetResult(false))
                .Build();

        public static readonly IActionPath<BoolDto> SomeAction =
            DecisionActionBuilder<BoolDto>.Create()
                .AddTitle(nameof(SomeAction))
                .AddAction(dto => dto.DoSomeAction());

        public static readonly IDecisionResult<IntDto> TwoResult =
            DecisionResultBuilder<IntDto>
                .Create()
                .AddTitle(nameof(TwoResult))
                .AddAction(boolDto => boolDto.SetResult(2))
                .Build();

        public static readonly IDecisionResult<IntDto> OneResult =
            DecisionResultBuilder<IntDto>
                .Create()
                .AddTitle(nameof(OneResult))
                .AddAction(boolDto => boolDto.SetResult(1))
                .Build();
    }
}
