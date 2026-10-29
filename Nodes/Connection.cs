namespace Nodes;

public interface IConnection
{
	IOutputSocket Output { get; }
	IInputSocket Input { get; }
}
public class Connection<T> : IConnection
{
	public OutputSocket<T> Output { get; }
	public InputSocket<T> Input { get; }
	IOutputSocket IConnection.Output => Output;
	IInputSocket IConnection.Input => Input;

	public Connection(OutputSocket<T> output, InputSocket<T> input)
	{
		Output = output;
		Input = input;
		Output.NotifyConnection();
		Input.NotifyConnection();
	}

	~Connection()
	{
		Output.NotifyConnection();
		Input.NotifyConnection();
	}
}