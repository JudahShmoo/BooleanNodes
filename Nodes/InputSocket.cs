namespace Nodes;

public interface IInputSocket : ISocket { }
public class InputSocket<T> : Socket<T>, IInputSocket
{
	public InputSocket(Node owner, string name) : base(owner, name)
	{
		OnConnection += () => ValueChanged?.Invoke();
		OnDisconnection += () => ValueChanged?.Invoke();
	}
	public required T DefaultValue { 
		get; 
		set
		{
			field = value;
			if (Connection is null)
				ValueChanged?.Invoke();
		}
	}
	public override T Value => Connection is null ? DefaultValue : Connection.Output.Value;

	public override event Action? ValueChanged;
}