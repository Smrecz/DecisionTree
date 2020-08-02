using DecisionTree.Decisions;
using DecisionTree.Exceptions;
using DecisionTree.Tests.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace DecisionTree.Tests
{
    public class DecisionsTest
    {
        private static readonly DecisionResult<BoolDto> TrueResult = 
            new DecisionResult<BoolDto>("True result", boolDto => boolDto.Result = true);

        private static readonly DecisionResult<BoolDto> FalseResult = 
            new DecisionResult<BoolDto>("False result", boolDto => boolDto.Result = false);

        private static readonly DecisionResult<BoolDto> DefaultResult = 
            new DecisionResult<BoolDto>("False result", boolDto => boolDto.Result = false);


        [Fact]
        public void ResultNode_Should_Evaluate_Action()
        {
            //Arrange
            static void Action(BoolDto x) => x.Result = true;

            var trueDto = new BoolDto(true);

            var resultNode = new DecisionResult<BoolDto>("Result", Action);

            //Act
            resultNode.Evaluate(trueDto);

            //Assert
            Assert.True(trueDto.Result);
        }

        [Fact]
        public void DecisionNode_Should_Evaluate_Proper_Path()
        {
            //Arrange
            Expression<Func<BoolDto, bool>> condition = boolDto => boolDto.BoolProperty;       
            var paths = new Dictionary<bool, IDecision<BoolDto>>()
            {
                {true, TrueResult },
                {false, FalseResult }
            };

            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode = new DecisionNode<BoolDto, bool>(condition, paths);

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
            Expression<Func<BoolDto, bool>> condition = boolDto => boolDto.BoolProperty;
            var paths = new Dictionary<bool, IDecision<BoolDto>>()
            {
                {true, TrueResult }
            };

            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode = new DecisionNode<BoolDto, bool>(condition, paths, DefaultResult);

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
            Expression<Func<BoolDto, bool>> condition = boolDto => boolDto.BoolProperty;
            var paths = new Dictionary<bool, IDecision<BoolDto>>
            {
                {true, TrueResult }
            };

            var trueDto = new BoolDto(true);
            var falseDto = new BoolDto(false);

            var decisionNode = new DecisionNode<BoolDto, bool>(condition, paths);

            //Act
            decisionNode.Evaluate(trueDto);
            void FalseAction() => decisionNode.Evaluate(falseDto);

            //Assert
            Assert.Throws<MissingDecisionPathException>(FalseAction);
        }
    }
}
