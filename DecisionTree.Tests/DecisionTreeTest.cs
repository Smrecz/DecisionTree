using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Tree;
using Xunit;
using System;
using DecisionTree.Tests.Model;
using System.Collections.Generic;
using DecisionTree.DotTreeExtensions;

namespace DecisionTree.Tests
{
    public class DecisionTreeTest
    {
        private static readonly string[] ExpectedGraphDefinition = {
            "digraph G {",
            "rankdir = LR;",
            "\"dto => (dto.Project.ItemsToDo == 0)\" -> \"FinishResult\" [label = \"True\"]",
            "\"dto => (dto.Project.ItemsToDo == 0)\" -> \"dto => dto.Project.IsOnHold\" [label = \"False\"]",
            "\"dto => dto.Project.IsOnHold\" -> \"DoNothingResult\" [label = \"True\"]",
            "\"dto => dto.Project.IsOnHold\" -> \"dto => dto.Project.Type\" [label = \"False\"]",
            "\"dto => dto.Project.Type\" -> \"DoNothingResult\" [label = \"Internal\"]",
            "\"dto => dto.Project.Type\" -> \"dto => (dto.Project.ItemsToDo > 10)\" [label = \"#default_option\"]",
            "\"dto => (dto.Project.ItemsToDo > 10)\" -> \"dto => (dto.Project.TimeToDeadline.Days < 7)\" [label = \"True\"]",
            "\"dto => (dto.Project.TimeToDeadline.Days < 7)\" -> \"MoveDeadlineResult\" [label = \"True\"]",
            "\"dto => (dto.Project.TimeToDeadline.Days < 7)\" -> \"dto => (dto.Project.BudgetRemaining < (dto.Project.ItemsToDo * 1000))\" [label = \"False\"]",
            "\"dto => (dto.Project.BudgetRemaining < (dto.Project.ItemsToDo * 1000))\" -> \"RequestBudgetResult\" [label = \"True\"]",
            "\"dto => (dto.Project.BudgetRemaining < (dto.Project.ItemsToDo * 1000))\" -> \"DoNothingResult\" [label = \"False\"]",
            "\"dto => (dto.Project.ItemsToDo > 10)\" -> \"dto => (dto.Project.BudgetRemaining < (dto.Project.ItemsToDo * 1000))\" [label = \"False\"]",
            "}"
        };


        [Fact]
        public void DecisionTree_Should_Define_Graph()
        {
            //Arrange
            var tree = new ProjectDecisionTree<ItProjectDecisionDto>();

            //Act
            var graphDefinition = tree.ConvertToDotGraph();

            //Assert
            var expected = string.Join(Environment.NewLine, ExpectedGraphDefinition);
            Assert.Equal(expected, graphDefinition);
        }

        [Theory]
        [MemberData(nameof(DecisionTreeDtoList))]
        public void DecisionTree_Should_Follow_Paths(ItProjectDecisionDto dto, string expectedResult)
        {
            //Arrange
            var tree = new ProjectDecisionTree<ItProjectDecisionDto>();

            //Act
            tree.Trunk().Evaluate(dto);

            //Assert
            Assert.Equal(expectedResult, dto.Result);
        }

        public static IEnumerable<object[]> DecisionTreeDtoList =>
           new List<object[]>
           {
                new object[]
                {
                    new ItProjectDecisionDto()
                    {
                        Project = new ItProject()
                        {
                            ItemsToDo = 0
                        }
                    },
                    "Project is finished."
                },
                new object[]
                {
                    new ItProjectDecisionDto()
                    {
                        Project = new ItProject()
                        {
                            ItemsToDo = 5,
                            IsOnHold = true
                        }
                    },
                    null
                },
                new object[]
                {
                    new ItProjectDecisionDto()
                    {
                        Project = new ItProject()
                        {
                            ItemsToDo = 5,
                            IsOnHold = false,
                            Type = ProjectType.Internal
                        }
                    },
                    null
                },
                new object[]
                {
                    new ItProjectDecisionDto()
                    {
                        Project = new ItProject()
                        {
                            ItemsToDo = 15,
                            IsOnHold = false,
                            Type = ProjectType.Financial,
                            TimeToDeadline = TimeSpan.FromDays(1)
                        }                    
                    },
                    "We are not going to make it."
                },
                new object[]
                {
                    new ItProjectDecisionDto()
                    {
                        Project = new ItProject()
                        {
                            ItemsToDo = 5,
                            IsOnHold = false,
                            Type = ProjectType.Financial,
                            BudgetRemaining = 1000
                        }
                    },
                    "Need more money."
                },
                new object[]
                {
                    new ItProjectDecisionDto()
                    {
                        Project = new ItProject()
                        {
                            ItemsToDo = 15,
                            IsOnHold = false,
                            Type = ProjectType.Financial,
                            TimeToDeadline = TimeSpan.FromDays(10),
                            BudgetRemaining = 1000
                        }
                    },
                    "Need more money."
                },
                new object[]
                {
                    new ItProjectDecisionDto()
                    {
                        Project = new ItProject()
                        {
                            ItemsToDo = 15,
                            IsOnHold = false,
                            Type = ProjectType.Financial,
                            TimeToDeadline = TimeSpan.FromDays(10),
                            BudgetRemaining = 1000000
                        }
                    },
                    null
                }
           };
    }
}
