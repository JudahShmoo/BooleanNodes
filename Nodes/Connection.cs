namespace Nodes;

public interface IConnection
{
	IOutputSocket Output { get; }
	IInputSocket Input { get; }
	void NotifyConnection();
	void NotifyDisconnection();
}
public class Connection<T>(OutputSocket<T> output, InputSocket<T> input) : IConnection
{
	public OutputSocket<T> Output { get; } = output;
	public InputSocket<T> Input { get; } = input;
	IOutputSocket IConnection.Output => Output;
	IInputSocket IConnection.Input => Input;
	public void NotifyConnection()
	{
		Output.NotifyConnection();
		Input.NotifyConnection();
	}
	public void NotifyDisconnection()
	{
		Output.NotifyDisconnection();
		Input.NotifyDisconnection();
	}
}