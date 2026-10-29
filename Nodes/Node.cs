namespace Nodes;
public abstract class Node(string name)
{
	public string Name { get; } = name;
	public NodeGraph? Graph { get; internal set; }

	private readonly List<IInputSocket> inputs = [];
	private readonly List<IOutputSocket> outputs = [];

	public IReadOnlyList<IInputSocket> Inputs => inputs;
	public IReadOnlyList<IOutputSocket> Outputs => outputs;

	public event Action? OnInputChanged
	{
		add => inputs.ForEach(i => i.ValueChanged += value);
		remove => inputs.ForEach(i => i.ValueChanged -= value);
	}

	protected InputSocket<T> AddInput<T>(string name, T defaultValue)
	{
		InputSocket<T> input = new(this, name)
		{ DefaultValue = defaultValue };
		inputs.Add(input);
		return input;
	}
	protected OutputSocket<T> AddOutput<T>(string name, Func<T> computeValue)
	{
		OutputSocket<T> output = new(this, name, computeValue);
		outputs.Add(output);
		return output;
	}
}