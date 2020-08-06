using System;
using System.Collections.Generic;
using System.Text.Json;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using DecisionTree.DotTreeExtensions;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Model;
using DecisionTree.Tests.Tree;
using Xunit;

namespace DecisionTree.Tests
{
    [UseReporter(typeof(DiffReporter))]
    [UseApprovalSubdirectory("./approvals")]
    public class DecisionTreeTest
    {
        [Fact]
        public void DecisionTree_Should_Define_Graph()
        {
            //Arrange
            var tree = new ProjectDecisionTree();

            //Act
            var graphDefinition = tree.ConvertToDotGraph();

            //Assert
            Approvals.VerifyHtml(graphDefinition);
        }

        [Theory]
        [MemberData(nameof(DecisionTreeDtoList))]
        public void DecisionTree_Should_Follow_Paths(int counter, ItProjectDecisionDto dto)
        {
            //Arrange
            var tree = new ProjectDecisionTree();

            //Act
            tree.GetTrunk().Evaluate(dto);

            //Assert
            NamerFactory.AdditionalInformation = counter.ToString();
            Approvals.VerifyJson(JsonSerializer.Serialize(dto));
        }

        public static IEnumerable<object[]> DecisionTreeDtoList =>
           new List<object[]>
           {
               CreateDtoTestData(0,
                   new ItProject
                   {
                       ItemsToDo = 0
                   }),
               CreateDtoTestData(1,
                   new ItProject
                   {
                       ItemsToDo = 5,
                       IsOnHold = true
                   }),
               CreateDtoTestData(2,
                   new ItProject
                   {
                       ItemsToDo = 5,
                       Type = ProjectType.Internal
                   }),
               CreateDtoTestData(3,
                   new ItProject
                   {
                       ItemsToDo = 15,
                       Type = ProjectType.Financial,
                       TimeToDeadline = TimeSpan.FromDays(1)
                   }),
               CreateDtoTestData(4,
                   new ItProject
                   {
                       ItemsToDo = 5,
                       Type = ProjectType.Financial,
                       BudgetRemaining = 1000
                   }),
               CreateDtoTestData(5,
                   new ItProject
                   {
                       ItemsToDo = 15,
                       Type = ProjectType.Financial,
                       TimeToDeadline = TimeSpan.FromDays(10),
                       BudgetRemaining = 1000
                   }),
               CreateDtoTestData(6,
                   new ItProject
                   {
                       ItemsToDo = 15,
                       Type = ProjectType.Financial,
                       TimeToDeadline = TimeSpan.FromDays(10),
                       BudgetRemaining = 1000000
                   })
           };

        private static object[] CreateDtoTestData(int counter, ItProject project) =>
            new object[] {counter, new ItProjectDecisionDto { Project = project } };
    }
}
