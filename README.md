# DecisionTree

DecisionTree is a lightweight open source framework that helps you define decision trees via config and generates DOT graph definition.<br>

Available on [![NuGet](https://img.shields.io/nuget/v/DecisionTree.svg)](https://www.nuget.org/packages/DecisionTree/) [![NuGet](https://img.shields.io/npm/l/express.svg)](https://github.com/Smrecz/DecisionTree/blob/master/LICENSE)<br>
# Aim

DecisionTree is aimed to provide a skeleton to build your decision tree around and to facilitate logic understanding, requirements cross-check, knowledge sharing by generating visualization definition in DOT language.<br>

# DOT graph definition
DOT language is used to define graphs and is understood by many tools.<br>
(all of which have dependency on Graphviz: https://graphviz.org/)<br>

DecisionTree's ConvertToDotGraph() method output can be used to generate a graph visualisation.<br>
You can use on-line tools like for example: http://www.webgraphviz.com/<br>

# Examples
Check out [ProjectDecisionTree.cs](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/Tree/ProjectDecisionTree.cs) as an example of defined decision tree.<br>
Check out [DecisionTreeTest.cs](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/DecisionTreeTest.cs) for tree usage.<br>

Below is a graph from generated DOT definition:<br>

![ProjectDecisionTree Graph](https://github.com/Smrecz/DecisionTree/blob/master/ProjectDecisionTree.png)

# TODO
More documentation...
