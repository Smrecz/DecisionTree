using System;
using System.Diagnostics;
using System.Text.Json;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Model;
using DecisionTree.Tests.Tree;
using Xunit;
using Xunit.Abstractions;

namespace DecisionTree.Tests
{
    public class PerformanceTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ItProjectDecisionDto _dtoInstance1 = new ItProjectDecisionDto
        {
            Project = new ItProject
            {
                ItemsToDo = 15,
                Type = ProjectType.Financial,
                TimeToDeadline = TimeSpan.FromDays(10),
                BudgetRemaining = 1000000
            }
        };

        private readonly ItProjectDecisionDto _dtoInstance2 = new ItProjectDecisionDto
        {
            Project = new ItProject
            {
                ItemsToDo = 15,
                Type = ProjectType.Financial,
                TimeToDeadline = TimeSpan.FromDays(10),
                BudgetRemaining = 1000000
            }
        };

        private readonly IDecision<ItProjectDecisionDto> _treeTrunk = new ProjectDecisionTree().GetTrunk();

        public PerformanceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact(Skip = "Used to measure rough performance estimate against pure IF statements - skipped for day to day test usage.")]
        public void Check_DecisionTree_Performance_Against_Pure_Conditions()
        {
            //Act
            _treeTrunk.Evaluate(_dtoInstance1);
            PureConditionalCheck(_dtoInstance2);

            var time1 = RunInTimedLoop(100_000, _dtoInstance1, dto => _treeTrunk.Evaluate(dto));

            var time2 = RunInTimedLoop(100_000, _dtoInstance2, PureConditionalCheck);

            _testOutputHelper.WriteLine($"Time1: {time1:F} {Environment.NewLine}Time2: {time2:F}");

            Assert.Equal(JsonSerializer.Serialize(_dtoInstance1), JsonSerializer.Serialize(_dtoInstance2));
        }

        private static void PureConditionalCheck(ItProjectDecisionDto dto)
        {
            if (dto.Project.ItemsToDo != 0)
            {
                dto.SetSendNotification(true);

                if (dto.Project.IsOnHold == false)
                    if(dto.Project.Type == ProjectType.Financial)
                        if(dto.Project.ItemsToDo > 10)
                            if(dto.Project.TimeToDeadline.Days >= 7)
                                if (dto.Project.BudgetRemaining >= dto.Project.ItemsToDo * 1000)
                                    dto.SetIsBudgetReviewed(true);
            }
        }

        private static long RunInTimedLoop(int count, ItProjectDecisionDto dto, Action<ItProjectDecisionDto> action)
        {
            var timer = new Stopwatch();

            timer.Start();

            for (var i = 0; i < count; i++)
                action.Invoke(dto);

            timer.Stop();

            return timer.ElapsedMilliseconds;
        }
    }
}
