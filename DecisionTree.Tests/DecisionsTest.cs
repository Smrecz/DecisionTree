using DecisionTree.Exceptions;
using DecisionTree.Tests.Dto;
using System;
using System.Linq.Expressions;
using DecisionTree.Builders;
using DecisionTree.Tests.TestData;
using Xunit;

namespace DecisionTree.Tests
{
    public class DecisionsTest
    {
        [Fact]
        public void ResultNode_Should_Evaluate_Action()
        {
            //Arrange
            Expression<Func<BoolDto, BoolDto>> action = boolDto => boolDto.SetResult(true);

            var trueDto = new BoolDto(true);

            var resultNode =
                DecisionResultBuilder<BoolDto>
                    .Create()
                    .AddTitle("Result")
                    .AddAction(action)
                    .Build();

            //Act
            resultNode.Evaluate(trueDto);

            //Assert
            Assert.True(trueDto.Result);
        }

        [Fact]
        public void ActionNode_Should_Evaluate_Action()
        {
            //Arrange
            var trueDto = new BoolDto(true);

            var resultNode =
                DecisionActionBuilder<BoolDto>
                    .Create()
                    .AddTitle("Result")
                    .AddAction(boolDto => boolDto.SetResult(true))
                    .AddPath(DecisionCatalog.TrueResult)
                    .Build();

            //Act
            resultNode.Evaluate(trueDto);

            //Assert
            Assert.True(trueDto.Result);
        }

        [Fact]
        public void DecisionNode_Should_Evaluate_Proper_Path()
        {
            //Arrange
            var firstDto = new IntDto(2);
            var secondDto = new IntDto(1);

            var decisionNode =
                DecisionNodeBuilder<IntDto, int>
                    .Create()
                    .AddTitle("Title")
                    .AddCondition(intDto => intDto.IntProperty)
                    .AddPath(2, DecisionCatalog.TwoResult)
                    .AddPath(1, DecisionCatalog.OneResult)
                    .Build();

            //Act
            decisionNode.Evaluate(firstDto);
            decisionNode.Evaluate(secondDto);

            //Assert
            Assert.Equal(2, firstDto.Result);
            Assert.Equal(1, secondDto.Result);
        }

        [Fact]
        public void DecisionNode_Should_Evaluate_Proper_ActionPath()
        {
            //Arrange
            var firstDto = new IntDto(2);
            var secondDto = new IntDto(1);

            var decisionNode =
                DecisionNodeBuilder<IntDto, int>
                    .Create()
                    .AddTitle("Title")
                    .AddCondition(intDto => intDto.IntProperty)
                    .AddPath(2, DecisionCatalog.TwoResult)
                    .AddPath(1, DecisionCatalog.OneResult, DecisionCatalog.SomeIntAction)
                    .Build();

            //Act
            decisionNode.Evaluate(firstDto);
            decisionNode.Evaluate(secondDto);

            //Assert
            Assert.Equal(2, firstDto.Result);
            Assert.Equal(1, secondDto.Result);
            Assert.True(secondDto.ActionFlag);

        }

        [Fact]
        public void DecisionNode_Should_Evaluate_Null_Path()
        {
            //Arrange
            var firstDto = new NullableIntDto(2);
            var secondDto = new NullableIntDto(null);

            var decisionNode =
                DecisionNodeBuilder<NullableIntDto, int?>
                    .Create()
                    .AddTitle("Title")
                    .AddCondition(intDto => intDto.IntProperty)
                    .AddPath(2, DecisionCatalog.TwoNullableResult)
                    .AddPath(null, DecisionCatalog.NullNullableResult)
                    .Build();

            //Act
            decisionNode.Evaluate(firstDto);
            decisionNode.Evaluate(secondDto);

            //Assert
            Assert.Equal(2, firstDto.Result);
            Assert.Null(secondDto.Result);
        }

        [Fact]
        public void DecisionNode_Should_Evaluate_Null_ActionPath()
        {
            //Arrange
            var firstDto = new NullableIntDto(2);
            var secondDto = new NullableIntDto(null);

            var decisionNode =
                DecisionNodeBuilder<NullableIntDto, int?>
                    .Create()
                    .AddTitle("Title")
                    .AddCondition(intDto => intDto.IntProperty)
                    .AddPath(2, DecisionCatalog.TwoNullableResult)
                    .AddPath(null, DecisionCatalog.NullNullableResult, DecisionCatalog.SomeNullableIntAction)
                    .Build();

            //Act
            decisionNode.Evaluate(firstDto);
            decisionNode.Evaluate(secondDto);

            //Assert
            Assert.Equal(2, firstDto.Result);
            Assert.True(secondDto.ActionFlag);
            Assert.Null(secondDto.Result);
        }

        [Fact]
        public void BinaryDecisionNode_Should_Evaluate_Proper_Path()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode =
                BinaryDecisionNodeBuilder<BoolDto>
                    .Create()
                    .AddTitle("Title")
                    .AddCondition(boolDto => boolDto.BoolProperty)
                    .AddPositivePath(DecisionCatalog.TrueResult)
                    .AddNegativePath(DecisionCatalog.FalseResult)
                    .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            decisionNode.Evaluate(falseDto);

            //Assert
            Assert.True(trueDto.Result);
            Assert.False(falseDto.Result);
        }

        [Fact]
        public void DecisionNode_Should_Evaluate_Default_Path()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode =
                DecisionNodeBuilder<BoolDto, bool>
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
        public void DecisionNode_Throw_Not_Defined_Path()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode =
                DecisionNodeBuilder<BoolDto, bool>
                    .Create()
                    .AddTitle("Title")
                    .AddCondition(boolDto => boolDto.BoolProperty)
                    .AddPath(true, DecisionCatalog.TrueResult)
                    .Build();

            //Act
            decisionNode.Evaluate(trueDto);
            void FalseAction() => decisionNode.Evaluate(falseDto);

            //Assert
            var exception = Assert.Throws<DecisionEvaluationException>(FalseAction);
            Assert.IsType<MissingDecisionPathException>(exception.InnerException);
        }

        [Fact]
        public void DecisionNode_Should_Throw_DecisionEvaluationException_On_Error()
        {
            //Arrange
            var trueDto = new BoolDto(true);

            var decisionNode =
                DecisionNodeBuilder<BoolDto, bool>
                    .Create()
                    .AddTitle("Title")
                    .AddCondition(boolDto => ThrowBoolException())
                    .AddPath(true, DecisionCatalog.TrueResult)
                    .Build();

            //Act
            void Action() => decisionNode.Evaluate(trueDto);

            //Assert
            Assert.Throws<DecisionEvaluationException>(Action);
        }

        [Fact]
        public void DecisionActionNode_Should_Throw_DecisionEvaluationException_On_Error()
        {
            //Arrange
            var trueDto = new BoolDto(true);

            var decisionNode =
                DecisionNodeBuilder<BoolDto, bool>
                    .Create()
                    .AddTitle("Title")
                    .AddCondition(boolDto => ThrowBoolException())
                    .AddPath(true, DecisionCatalog.TrueResult)
                    .AddAction(dto => (BoolDto)dto.DoSomeAction())
                    .Build();

            //Act
            void Action() => decisionNode.Evaluate(trueDto);

            //Assert
            Assert.Throws<DecisionEvaluationException>(Action);
        }

        [Fact]
        public void DecisionResult_Should_Throw_DecisionEvaluationException_On_Error()
        {
            //Arrange
            var trueDto = new BoolDto(true);

            var decisionResult =
                DecisionResultBuilder<BoolDto>
                    .Create()
                    .AddTitle("Title")
                    .AddAction(dto => ThrowBoolDtoException())
                    .Build();

            //Act
            void Action() => decisionResult.Evaluate(trueDto);

            //Assert
            Assert.Throws<DecisionEvaluationException>(Action);
        }

        [Fact]
        public void DecisionAction_Should_Throw_DecisionEvaluationException_On_Error()
        {
            //Arrange
            var trueDto = new BoolDto(true);

            var decisionAction =
                DecisionActionBuilder<BoolDto>
                    .Create()
                    .AddTitle("Title")
                    .AddAction(dto => ThrowBoolDtoException())
                    .AddPath(DecisionCatalog.TrueResult)
                    .Build();

            //Act
            void Action() => decisionAction.Evaluate(trueDto);

            //Assert
            Assert.Throws<DecisionEvaluationException>(Action);
        }

        private static bool ThrowBoolException() => 
            throw new Exception("Test exception");

        private static BoolDto ThrowBoolDtoException() =>
            throw new Exception("Test exception");
    }
}
