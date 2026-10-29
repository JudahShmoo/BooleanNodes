namespace Nodes.BasicNodes;
public class AndNode : Node
{
	public AndNode() : base("And")
	{
		A = AddInput("A", false);
		B = AddInput("B", false);
		Output = AddOutput("Output", () => A.Value && B.Value);
	}
	public InputSocket<bool> A { get; }
	public InputSocket<bool> B { get; }
	public OutputSocket<bool> Output { get; }
}