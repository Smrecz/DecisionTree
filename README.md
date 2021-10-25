# DecisionTree

DecisionTree is a lightweight open source framework that helps you define decision trees via configuration and generates DOT graph definition.<br>

Available on:<br>
<a href="https://www.nuget.org/packages/DecisionTree"><img alt="Nuget" src="https://buildstats.info/nuget/DecisionTree"></a> [![NuGet](https://img.shields.io/npm/l/express.svg)](https://github.com/Smrecz/DecisionTree/blob/master/LICENSE)<br>

# Aim

DecisionTree is aimed to provide a skeleton to build your decision tree around and to facilitate logic understanding, requirements cross-check and knowledge sharing by generating visualization definition in DOT language.<br>

# Graph definition in DOT
DOT language is used to define graphs and is understood by many tools - all of which have dependency on Graphviz: https://graphviz.org/<br>

DecisionTree's `ConvertToDotGraph<T>()` method output can be used to generate a graph visualisation.<br>
You can use on-line tools like for example: https://dreampuf.github.io/GraphvizOnline/<br>

# Examples
Check out [ProjectDecisionTree.cs](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/Tree/ProjectDecisionTree.cs) as an example of defined decision tree.<br>
Check out [DecisionTreeTest.cs](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/DecisionTreeTest.cs) for tree usage.<br>

Below is an example of graph from generated DOT definition ([DOT](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/approvals/DecisionTreeTest.DecisionTree_Should_Define_Graph.approved.html)):<br>

![ProjectDecisionTree Graph](https://github.com/Smrecz/DecisionTree/blob/master/ProjectDecisionTree.png)

# TODO
<b>ALL SUGGESTIONS ARE WELCOME!</b>
