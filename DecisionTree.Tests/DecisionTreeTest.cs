using System;
using System.Text.Json;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using DecisionTree.DotTreeExtensions;
using DecisionTree.Exceptions;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Mock;
using DecisionTree.Tests.Model;
using DecisionTree.Tests.TestData;
using DecisionTree.Tests.Tree;
using Xunit;

namespace DecisionTree.Tests
{
    [UseReporter(typeof(DiffReporter))]
    [UseApprovalSubdirectory("./approvals")]
    public class DecisionTreeTest
    {
        [Theory]
        [MemberData(nameof(DecisionTreeTestData.GraphTestDataList), MemberType = typeof(DecisionTreeTestData))]
        public void DecisionTree_Should_Define_Graph(string title, GraphOptions options)
        {
            //Arrange
            var tree = new ProjectDecisionTree();

            //Act
            var graphDefinition = tree.ConvertToDotGraph(options);

            //Assert
            NamerFactory.AdditionalInformation = title;
            Approvals.VerifyHtml(graphDefinition);
        }

        [Fact]
        public void DecisionTree_Fake_Should_Throw()
        {
            //Arrange
            var tree = new FakeTree<bool>();

            //Act
            void Evaluate() => tree.GetTrunk().Evaluate(true);

            //Assert
            Assert.Throws<NotImplementedException>(Evaluate);
        }

        [Fact]
        public void DecisionTree_Should_Throw_On_Custom_Types()
        {
            //Arrange
            var tree = new FakeTree<bool>();

            //Act
            string GraphDefinition() => tree.ConvertToDotGraph();

            //Assert
            Assert.Throws<NotPrintableTypeException>(GraphDefinition);
        }

        [Theory]
        [MemberData(nameof(DecisionTreeTestData.TreeTestDataList), MemberType = typeof(DecisionTreeTestData))]
        public void DecisionTree_Should_Follow_Paths(string title, ItProjectDecisionDto dto)
        {
            //Arrange
            var tree = new ProjectDecisionTree();

            //Act
            tree.GetTrunk().Evaluate(dto);

            //Assert
            NamerFactory.AdditionalInformation = title;
            Approvals.VerifyJson(JsonSerializer.Serialize(dto));
        }

        [Fact]
        public void DecisionTree_Should_Provide_Full_Path_On_Exception()
        {
            //Arrange
            var dto = new ItProjectDecisionDtoWithError
            {
                Project = new ItProject
                {
                    ItemsToDo = 15,
                    Type = ProjectType.External,
                    TimeToDeadline = TimeSpan.FromDays(10),
                    BudgetRemaining = 1000
                }
            };



            var tree = new ProjectDecisionTree();

            //Act
            void Result() => tree.GetTrunk().Evaluate(dto);

            //Assert
            var exception = Assert.Throws<DecisionEvaluationException>(Result);
            Assert.IsType<ArgumentException>(exception.InnerException);

            Assert.Equal(DecisionTreeTestData.ExpectedExceptionMessageWithPath, exception.Message);
        }
    }
}
