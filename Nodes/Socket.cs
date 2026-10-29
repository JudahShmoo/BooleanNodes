namespace Nodes;

public interface ISocket
{
	Node Owner { get; }
	string Name { get; }
	abstract object? Value { get; }
	event Action? ValueChanged;
}
public abstract class Socket<T>(Node owner, string name) : ISocket
{
	public abstract event Action? ValueChanged;
	public NodeGraph? Graph => Owner.Graph;
	public Connection<T>? Connection => Graph?.GetConnection(this);
	public event Action? OnConnection
	{
		add => Graph!.OnConnection += c => { if (c.Input == this || c.Output == this) value?.Invoke(); };
		remove => Graph!.OnConnection -= c => { if (c.Input == this || c.Output == this) value?.Invoke(); };
	}
	public event Action? OnDisconnection
	{
		add => Graph!.OnDisconnection += c => { if (c.Input == this || c.Output == this) value?.Invoke(); };
		remove => Graph!.OnDisconnection -= c => { if (c.Input == this || c.Output == this) value?.Invoke(); };
	}
	public Node Owner { get; } = owner;
	public string Name { get; } = name;
	public abstract T Value { get; }
	object? ISocket.Value => Value;
}