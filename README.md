# DecisionTree

DecisionTree is a lightweight opensource framework that helps you define decision trees via config and generates DOT graph definition.

# DOT graph definition
DOT language is used to define graphs and is understood by many tools.
(all of which have dependency on Graphviz: https://graphviz.org/)

DecisionTree's ConvertToDotGraph() method output can be used to generate a graph visualisation.
You can use on-line tools like for example: http://www.webgraphviz.com/

# Examples
Check out [ProjectDecisionTree.cs](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/Tree/ProjectDecisionTree.cs) as an example of defined decision tree.
Check out [DecisionTreeTest.cs](https://github.com/Smrecz/DecisionTree/blob/master/DecisionTree.Tests/DecisionTreeTest.cs) for tree usage.

Below is a graph from generated DOT definition:

![ProjectDecisionTree Graph](https://github.com/Smrecz/DecisionTree/blob/master/ProjectDecisionTree.png)

# TODO
More documentation...
