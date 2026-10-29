using Nodes;
using Nodes.BasicNodes;

NodeGraph graph = new();

AndNode node1 = new();
AndNode node2 = new();

graph.AddNode(node1);
graph.AddNode(node2);

graph.Connect(node1.Output, node2.A);

void LogValues()
{
	Console.WriteLine($"Node1 - A: {node1.A.Value}, B: {node1.B.Value}, Output: {node1.Output.Value}");
	Console.WriteLine($"Node2 - A: {node2.A.Value}, B: {node2.B.Value}, Output: {node2.Output.Value}");
	Console.WriteLine("--------------------------------------------------");
}

LogValues();
node1.A.DefaultValue = true;
LogValues();
node1.B.DefaultValue = true;
LogValues();
node2.B.DefaultValue = true;
LogValues();