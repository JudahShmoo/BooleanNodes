namespace Nodes.BasicNodes;
public class NorNode : Node
{
	public NorNode() : base("Nor")
	{
		A = AddInput("A", false);
		B = AddInput("B", false);
		Output = AddOutput("Output", () => !(A.Value || B.Value));
	}
	public InputSocket<bool> A { get; }
	public InputSocket<bool> B { get; }
	public OutputSocket<bool> Output { get; }
}