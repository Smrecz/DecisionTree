using DecisionTree.Builders;
using DecisionTree.Decisions;
using DecisionTree.Exceptions;
using DecisionTree.Tests.Dto;
using Xunit;

namespace DecisionTree.Tests
{
    public class BuildersTest
    {
        private static readonly DecisionResult<BoolDto> TrueResult = 
            DecisionResultBuilder<BoolDto>.Create()
            .AddTitle("True result")
            .AddAction(boolDto => boolDto.SetResult(true))
            .Build();

        private static readonly DecisionResult<BoolDto> FalseResult =
            DecisionResultBuilder<BoolDto>.Create()
            .AddTitle("False result")
            .AddAction(boolDto => boolDto.SetResult(false))
            .Build();

        private static readonly DecisionResult<BoolDto> DefaultResult =
            DecisionResultBuilder<BoolDto>.Create()
            .AddTitle("False result")
            .AddAction(boolDto => boolDto.SetResult(false))
            .Build();


        [Fact]
        public void Built_ResultNode_Should_Evaluate_Action()
        {
            //Arrange
            var trueDto = new BoolDto(true);

            //Act
            TrueResult.Evaluate(trueDto);

            //Assert
            Assert.True(trueDto.Result);
        }

        [Fact]
        public void Built_DecisionNode_Should_Evaluate_Proper_Path()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode = DecisionNodeBuilder<BoolDto, bool>.Create()
                .AddTitle("Title")
                .AddCondition(boolDto => boolDto.BoolProperty)
                .AddPath(true, TrueResult)
                .AddPath(false, FalseResult)
                .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            decisionNode.Evaluate(falseDto);

            //Assert
            Assert.True(trueDto.Result);
            Assert.False(falseDto.Result);
        }

        [Fact]
        public void Built_DecisionNode_Should_Use_Default_Path()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode = DecisionNodeBuilder<BoolDto, bool>
                .Create()
                .AddTitle("Title")
                .AddCondition(boolDto => boolDto.BoolProperty)
                .AddPath(true, TrueResult)
                .AddDefault(DefaultResult)
                .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            decisionNode.Evaluate(falseDto);

            //Assert
            Assert.True(trueDto.Result);
            Assert.False(falseDto.Result);
        }

        [Fact]
        public void Built_DecisionNode_Throw_Not_Defined_Path()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode = DecisionNodeBuilder<BoolDto, bool>
                .Create()
                .AddTitle("Title")
                .AddCondition(boolDto => boolDto.BoolProperty)
                .AddPath(true, TrueResult)
                .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            void FalseAction() => decisionNode.Evaluate(falseDto);

            //Assert
            Assert.Throws<MissingDecisionPathException>(FalseAction);
        }
    }
}
