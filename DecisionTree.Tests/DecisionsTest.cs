using DecisionTree.Decisions;
using DecisionTree.Exceptions;
using DecisionTree.Tests.Dto;
using System;
using System.Linq.Expressions;
using DecisionTree.Builders;
using Xunit;

namespace DecisionTree.Tests
{
    public class DecisionsTest
    {
        private static readonly IDecisionResult<BoolDto> TrueResult =
            DecisionResultBuilder<BoolDto>
                .Create()
                .AddTitle("True result")
                .AddAction(boolDto => boolDto.SetResult(true))
                .Build();

        private static readonly IDecisionResult<BoolDto> FalseResult =
            DecisionResultBuilder<BoolDto>
                .Create()
                .AddTitle("False result")
                .AddAction(boolDto => boolDto.SetResult(false))
                .Build();

        private static readonly IDecisionResult<BoolDto> DefaultResult =
            DecisionResultBuilder<BoolDto>
                .Create()
                .AddTitle("False result")
                .AddAction(boolDto => boolDto.SetResult(false))
                .Build();

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
                    .AddPath(TrueResult)
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
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode =
                DecisionNodeBuilder<BoolDto, bool>
                    .Create()
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
        public void DecisionNode_Should_Default_Path()
        {
            //Arrange
            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode =
                DecisionNodeBuilder<BoolDto, bool>
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
