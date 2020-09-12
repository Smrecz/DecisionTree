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

        public static readonly IActionPath<BoolDto> SomeBoolAction =
            DecisionActionBuilder<BoolDto>.Create()
                .AddTitle(nameof(SomeBoolAction))
                .AddAction(dto => (BoolDto)dto.DoSomeAction());

        public static readonly IActionPath<IntDto> SomeIntAction =
            DecisionActionBuilder<IntDto>.Create()
                .AddTitle(nameof(SomeIntAction))
                .AddAction(dto => (IntDto)dto.DoSomeAction());

        public static readonly IActionPath<NullableIntDto> SomeNullableIntAction =
            DecisionActionBuilder<NullableIntDto>.Create()
                .AddTitle(nameof(SomeNullableIntAction))
                .AddAction(dto => (NullableIntDto)dto.DoSomeAction());

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

        public static readonly IDecisionResult<NullableIntDto> TwoNullableResult =
            DecisionResultBuilder<NullableIntDto>
                .Create()
                .AddTitle(nameof(TwoNullableResult))
                .AddAction(boolDto => boolDto.SetResult(2))
                .Build();

        public static readonly IDecisionResult<NullableIntDto> NullNullableResult =
            DecisionResultBuilder<NullableIntDto>
                .Create()
                .AddTitle(nameof(NullNullableResult))
                .AddAction(boolDto => boolDto.SetResult(null))
                .Build();
    }
}
