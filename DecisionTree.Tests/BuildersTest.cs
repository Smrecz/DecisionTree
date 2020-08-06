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
            new DecisionResultBuilder<BoolDto>()
            .AddTitle("True result")
            .AddAction(boolDto => boolDto.SetResult(true))
            .Build();

        private static readonly DecisionResult<BoolDto> FalseResult =
            new DecisionResultBuilder<BoolDto>()
            .AddTitle("False result")
            .AddAction(boolDto => boolDto.SetResult(false))
            .Build();

        private static readonly DecisionResult<BoolDto> DefaultResult =
            new DecisionResultBuilder<BoolDto>()
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

            var decisionNode = new DecisionNodeBuilder<BoolDto, bool>()
                .AddPath(true, TrueResult)
                .AddPath(false, FalseResult)
                .AddCondition(boolDto => boolDto.BoolProperty)
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

            var decisionNode = new DecisionNodeBuilder<BoolDto, bool>()
                .AddPath(true, TrueResult)
                .AddDefault(DefaultResult)
                .AddCondition(boolDto => boolDto.BoolProperty)
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

            var decisionNode = new DecisionNodeBuilder<BoolDto, bool>()
                .AddPath(true, TrueResult)
                .AddCondition(boolDto => boolDto.BoolProperty)
                .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            void FalseAction() => decisionNode.Evaluate(falseDto);

            //Assert
            Assert.Throws<MissingDecisionPathException>(FalseAction);
        }

        [Fact]
        public void Builder_DecisionNode_Throw_Missing_Condition()
        {
            //Arrange

            //Act
            static void BuildAction() => new DecisionNodeBuilder<BoolDto, bool>()
                .AddPath(true, TrueResult)
                .Build();

            //Assert
            Assert.Throws<MissingBuilderConfigException>(BuildAction);
        }

        [Fact]
        public void Builder_DecisionNode_Throw_Missing_Path()
        {
            //Arrange

            //Act
            static void BuildAction() => new DecisionNodeBuilder<BoolDto, bool>()
                .AddCondition(boolDto => boolDto.BoolProperty)
                .Build();

            //Assert
            Assert.Throws<MissingBuilderConfigException>(BuildAction);
        }

        [Fact]
        public void Builder_ResultNode_Throw_Missing_Config()
        {
            //Arrange

            //Act
            static void BuildAction() => new DecisionResultBuilder<BoolDto>()
                .Build();

            //Assert
            Assert.Throws<MissingBuilderConfigException>(BuildAction);
        }
    }
}
