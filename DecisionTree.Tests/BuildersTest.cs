using DecisionTree.Builders;
using DecisionTree.Exceptions;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.TestData;
using Xunit;

namespace DecisionTree.Tests
{
    public class BuildersTest
    {
        [Fact]
        public void Built_ResultNode_Should_Evaluate_Action()
        {
            //Arrange
            var trueDto = new BoolDto(true);

            //Act
            DecisionCatalog.TrueResult.Evaluate(trueDto);

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
                .AddPath(true, DecisionCatalog.TrueResult)
                .AddPath(false, DecisionCatalog.FalseResult)
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
                .AddPath(true, DecisionCatalog.TrueResult)
                .AddDefault(DecisionCatalog.DefaultResult)
                .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            decisionNode.Evaluate(falseDto);

            //Assert
            Assert.True(trueDto.Result);
            Assert.False(falseDto.Result);
        }

        [Fact]
        public void Built_DecisionNode_Should_Create_Action_From_ActionPath()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode = DecisionNodeBuilder<BoolDto, bool>.Create()
                .AddTitle("Title")
                .AddCondition(boolDto => boolDto.BoolProperty)
                .AddPath(true, DecisionCatalog.TrueResult, DecisionCatalog.SomeAction)
                .AddPath(false, DecisionCatalog.FalseResult)
                .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            decisionNode.Evaluate(falseDto);

            //Assert
            Assert.True(trueDto.Result);
            Assert.False(falseDto.Result);
            Assert.True(trueDto.ActionFlag);
            Assert.False(falseDto.ActionFlag);
        }

        [Fact]
        public void Built_DecisionNode_Should_Call_Action()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode = DecisionNodeBuilder<BoolDto, bool>.Create()
                .AddTitle("Title")
                .AddCondition(boolDto => boolDto.BoolProperty)
                .AddPath(true, DecisionCatalog.TrueResult)
                .AddPath(false, DecisionCatalog.FalseResult)
                .AddAction(boolDto => boolDto.DoSomeAction())
                .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            decisionNode.Evaluate(falseDto);

            //Assert
            Assert.True(trueDto.Result);
            Assert.False(falseDto.Result);
            Assert.True(trueDto.ActionFlag);
            Assert.True(falseDto.ActionFlag);
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
                .AddPath(true, DecisionCatalog.TrueResult)
                .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            void FalseAction() => decisionNode.Evaluate(falseDto);

            //Assert
            Assert.Throws<MissingDecisionPathException>(FalseAction);
        }
    }
}
