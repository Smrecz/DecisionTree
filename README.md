# DecisionTree

DecisionTree is a lightweight open source framework that helps you define decision trees via config and generates DOT graph definition.<br>

Available on:<br>
<a href="https://www.nuget.org/packages/DecisionTree"><img alt="Nuget" src="https://buildstats.info/nuget/DecisionTree"></a> [![NuGet](https://img.shields.io/npm/l/express.svg)](https://github.com/Smrecz/DecisionTree/blob/master/LICENSE)<br>

# Aim

DecisionTree is aimed to provide a skeleton to build your decision tree around and to facilitate logic understanding, requirements cross-check and knowledge sharing by generating visualization definition in DOT language.<br>

# Graph definition in DOT
DOT language is used to define graphs and is understood by many tools.<br>
(all of which have dependency on Graphviz: https://graphviz.org/)<br>

DecisionTree's ConvertToDotGraph() method output can be used to generate a graph visualisation.<br>
You can use on-line tools like for example: https://dreampuf.github.io/GraphvizOnline/<br>

# Examples
Check out [ProjectDecisionTree.cs](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/Tree/ProjectDecisionTree.cs) as an example of defined decision tree.<br>
Check out [DecisionTreeTest.cs](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/DecisionTreeTest.cs) for tree usage.<br>

Below is a graph from generated DOT definition ([DOT](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/approvals/DecisionTreeTest.DecisionTree_Should_Define_Graph.approved.html)):<br>

![ProjectDecisionTree Graph](https://github.com/Smrecz/DecisionTree/blob/master/ProjectDecisionTree.png)

# CONTRIBUTION
If you would like to support my work by making a Donation (€10), you are welcome to do so:<br>
[![Donate](https://img.shields.io/badge/donate-PayPal-yellow.svg)](https://www.paypal.me/smrecz/10)

# TODO
<b>ALL SUGGESTIONS ARE WELCOME!</b>
<br>
<br>
* Implement Action nodes to just apply action without changing the path
* More documentation...
