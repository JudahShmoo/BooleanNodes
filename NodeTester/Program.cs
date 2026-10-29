using Nodes;
using Nodes.BasicNodes;

NodeGraph graph = new();

AndNode and1 = new();
AndNode and2 = new();
NorNode nor = new();
XorNode xor = new();

graph.AddNode(and1);
graph.AddNode(and2);
graph.AddNode(nor);
graph.AddNode(xor);

graph.Connect(and1.Output, and2.A);
graph.Connect(nor.Output, and2.B);
graph.Connect(xor.Output, nor.A);

void LogValues()
{
	Console.WriteLine($"And1 - A: {and1.A.Value}, B: {and1.B.Value}, Output: {and1.Output.Value}");
	Console.WriteLine($"And2 - A: {and2.A.Value}, B: {and2.B.Value}, Output: {and2.Output.Value}");
	Console.WriteLine($"Nor - A: {nor.A.Value}, B: {nor.B.Value}, Output: {nor.Output.Value}");
	Console.WriteLine($"Xor - A: {xor.A.Value}, B: {xor.B.Value}, Output: {xor.Output.Value}");
	Console.WriteLine("--------------------------------------------------");
}

LogValues();
and1.A.DefaultValue = true;
LogValues();
and1.B.DefaultValue = true;
LogValues();
and2.A.DefaultValue = true;
LogValues();
and2.B.DefaultValue = true;
LogValues();
nor.A.DefaultValue = true;
LogValues();
nor.B.DefaultValue = true;
LogValues();
xor.A.DefaultValue = true;
LogValues();
xor.B.DefaultValue = true;
LogValues();