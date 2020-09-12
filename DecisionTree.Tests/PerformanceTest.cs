using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
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
                Type = ProjectType.External,
                TimeToDeadline = TimeSpan.FromDays(10),
                BudgetRemaining = 1000000
            }
        };

        private readonly ItProjectDecisionDto _dtoInstance2 = new ItProjectDecisionDto
        {
            Project = new ItProject
            {
                ItemsToDo = 15,
                Type = ProjectType.External,
                TimeToDeadline = TimeSpan.FromDays(10),
                BudgetRemaining = 1000000
            }
        };

        private readonly IDecision<ItProjectDecisionDto> _treeTrunk = new ProjectDecisionTree().GetTrunk();

        public PerformanceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact(Skip = "Use to get rough idea about DecisionTree performance against 'if' spam.")]
        public void Check_DecisionTree_Performance_Against_Pure_Conditions()
        {
            //Arrange
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);

            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            RunInTimedLoop(1000, 10, DecisionTreeCheck);
            RunInTimedLoop(1000, 10, PureConditionalCheck);

            //Act
            var time1 = RunInTimedLoop(100_000, 10, DecisionTreeCheck);
            var time2 = RunInTimedLoop(1_000_000, 10, PureConditionalCheck) / 10;

            //Assert
            _testOutputHelper.WriteLine($"Time1: {time1:F} {Environment.NewLine}Time2: {time2:F}");
            Assert.Equal(JsonSerializer.Serialize(_dtoInstance1), JsonSerializer.Serialize(_dtoInstance2));
            Assert.True(time1 < 3 * time2);
        }

        private void DecisionTreeCheck()
        {
            _treeTrunk.Evaluate(_dtoInstance1);
        }

        private void PureConditionalCheck()
        {
            if (_dtoInstance2.Project.ItemsToDo == 0) return;
                _dtoInstance2.SetSendNotification(true);
            if (_dtoInstance2.Project.IsOnHold) return;
            if (_dtoInstance2.Project.Type != ProjectType.External) return;
            if (_dtoInstance2.Project.ItemsToDo <= 10) return;
            if (_dtoInstance2.Project.TimeToDeadline.Days < 7) return;
            if (_dtoInstance2.Project.BudgetRemaining >= _dtoInstance2.Project.ItemsToDo * 1000)
                _dtoInstance2.SetIsBudgetReviewed(true);
        }

        private static double RunInTimedLoop(int count, int times, Action action)
        {
            var timeRecordings = new List<double>(times);

            for (var i = 0; i < times; i++)
            {
                var start = GetCurrentCpuMilliseconds();

                for (var j = 0; j < count; j++)
                    action.Invoke();

                var stop = GetCurrentCpuMilliseconds();

                timeRecordings.Add(stop - start);
            }

            return NormalizedMean(timeRecordings);
        }

        private static double GetCurrentCpuMilliseconds() =>
            Process
                .GetCurrentProcess()
                .TotalProcessorTime
                .TotalMilliseconds;

        private static double NormalizedMean(ICollection<double> values)
        {
            var deviations = GetDeviations(values).ToArray();
            var meanDeviation = deviations.Sum(deviation => Math.Abs(deviation.Item2)) / values.Count;
            return deviations
                .Where(deviation => deviation.Item2 > 0 || Math.Abs(deviation.Item2) <= meanDeviation)
                .Average(t => t.Item1);
        }

        private static IEnumerable<Tuple<double, double>> GetDeviations(ICollection<double> values)
        {
            var average = values.Average();
            foreach (var value in values)
                yield return Tuple.Create(value, average - value);
        }
    }
}
