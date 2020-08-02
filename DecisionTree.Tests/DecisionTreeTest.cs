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
        private static readonly string[] expectedGraphDefinition = new string[]
        {
            "digraph G {",
            "rankdir = LR;",
            "\"x => (x.Project.ItemsToDo == 0)\" -> \"FinishResult\" [label = \"True\"]",
            "\"x => (x.Project.ItemsToDo == 0)\" -> \"x => x.Project.IsOnHold\" [label = \"False\"]",
            "\"x => x.Project.IsOnHold\" -> \"DoNothingResult\" [label = \"True\"]",
            "\"x => x.Project.IsOnHold\" -> \"x => x.Project.Type\" [label = \"False\"]",
            "\"x => x.Project.Type\" -> \"DoNothingResult\" [label = \"Internal\"]",
            "\"x => x.Project.Type\" -> \"x => (x.Project.ItemsToDo > 10)\" [label = \"#default_option\"]",
            "\"x => (x.Project.ItemsToDo > 10)\" -> \"x => (x.Project.TimeToDeadline.Days < 7)\" [label = \"True\"]",
            "\"x => (x.Project.TimeToDeadline.Days < 7)\" -> \"MoveDeadlineResult\" [label = \"True\"]",
            "\"x => (x.Project.TimeToDeadline.Days < 7)\" -> \"x => (x.Project.BudgetRemaining < (x.Project.ItemsToDo * 1000))\" [label = \"False\"]",
            "\"x => (x.Project.BudgetRemaining < (x.Project.ItemsToDo * 1000))\" -> \"RequestBudgetResult\" [label = \"True\"]",
            "\"x => (x.Project.BudgetRemaining < (x.Project.ItemsToDo * 1000))\" -> \"DoNothingResult\" [label = \"False\"]",
            "\"x => (x.Project.ItemsToDo > 10)\" -> \"x => (x.Project.BudgetRemaining < (x.Project.ItemsToDo * 1000))\" [label = \"False\"]",
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
            var expected = string.Join(Environment.NewLine, expectedGraphDefinition);
            Assert.Equal(expected, graphDefinition);
        }

        [Theory]
        [MemberData(nameof(DecisionTreeDtos))]
        public void DecisionTree_Should_Follow_Paths(ItProjectDecisionDto dto, string expectedResult)
        {
            //Arrange
            var tree = new ProjectDecisionTree<ItProjectDecisionDto>();

            //Act
            tree.Trunk().Evaluate(dto);

            //Assert
            Assert.Equal(expectedResult, dto.Result);
        }

        public static IEnumerable<object[]> DecisionTreeDtos =>
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
