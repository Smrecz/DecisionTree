﻿using System;
using System.Collections.Generic;
using DecisionTree.DotTreeExtensions;
using DecisionTree.Tests.Dto;
using DecisionTree.Tests.Model;

namespace DecisionTree.Tests.TestData
{
    public class DecisionTreeTestData
    {
        public static IEnumerable<object[]> TreeTestDataList =>
            new List<object[]>
            {
                CreateTreeTestData("NoItemsToDo",
                    new ItProject
                    {
                        ItemsToDo = 0
                    }),
                CreateTreeTestData("IsOnHold",
                    new ItProject
                    {
                        ItemsToDo = 5,
                        IsOnHold = true
                    }),
                CreateTreeTestData("Internal",
                    new ItProject
                    {
                        ItemsToDo = 5,
                        Type = ProjectType.Internal
                    }),
                CreateTreeTestData("NullType",
                    new ItProject
                    {
                        ItemsToDo = 5,
                        Type = null
                    }),
                CreateTreeTestData("OneDayToDeadline",
                    new ItProject
                    {
                        ItemsToDo = 15,
                        Type = ProjectType.Financial,
                        TimeToDeadline = TimeSpan.FromDays(1)
                    }),
                CreateTreeTestData("LowBudget",
                    new ItProject
                    {
                        ItemsToDo = 5,
                        Type = ProjectType.Financial,
                        BudgetRemaining = 1000
                    }),
                CreateTreeTestData("ToMuchToDo",
                    new ItProject
                    {
                        ItemsToDo = 15,
                        Type = ProjectType.Financial,
                        TimeToDeadline = TimeSpan.FromDays(10),
                        BudgetRemaining = 1000
                    }),
                CreateTreeTestData("EverythingFine",
                    new ItProject
                    {
                        ItemsToDo = 15,
                        Type = ProjectType.Financial,
                        TimeToDeadline = TimeSpan.FromDays(10),
                        BudgetRemaining = 1000000
                    })
            };

        public static IEnumerable<object[]> GraphTestDataList =>
            new List<object[]>
            {
                GraphTestData("Default", false, false ),
                GraphTestData("Horizontal", true, false ),
                GraphTestData("UniquePaths", false, true ),
                GraphTestData("HorizontalUniquePaths", true, true )
            };

        public static string ExpectedExceptionMessageWithPath =>
            $"Decision evaluation failed (check inner exception for details).{Environment.NewLine}" +
            $"Full decision tree path:{Environment.NewLine}" +
            $"X-- 'RequestBudgetResult' (FAILED){Environment.NewLine}" +
            $"^-- 'BudgetDecision'{Environment.NewLine}" +
            $"^-- 'DeadlineDecision'{Environment.NewLine}" +
            $"^-- 'ToDoDecision'{Environment.NewLine}" +
            $"^-- 'ProjectTypeDecision'{Environment.NewLine}" +
            $"^-- 'IsOnHoldDecision'{Environment.NewLine}" +
            $"^-- 'False FinishedDecision - SendNotificationAction'{Environment.NewLine}" +
            $"^-- 'FinishedDecision'";

        private static object[] GraphTestData(string title, bool isHorizontal, bool hasUniquePaths) =>
            new object[] { title, new GraphOptions
                {
                    IsHorizontal = isHorizontal,
                    UseUniquePaths = hasUniquePaths
                }
            };

        private static object[] CreateTreeTestData(string title, ItProject project) =>
            new object[] { title, new ItProjectDecisionDto { Project = project } };
    }
}