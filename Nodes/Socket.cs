namespace Nodes;

public interface ISocket
{
	Node Owner { get; }
	string Name { get; }
	abstract object? Value { get; }
	event Action? ValueChanged;
	NodeGraph? Graph { get; }
	IConnection? Connection => Graph?.GetConnection(this);
	Type GetType();
}
public abstract class Socket<T>(Node owner, string name) : ISocket
{
	public abstract event Action? ValueChanged;
	public NodeGraph? Graph => Owner.Graph;
	public Connection<T>? Connection => Graph?.GetConnection(this);
	public Node Owner { get; } = owner;
	public string Name { get; } = name;
	public abstract T Value { get; }
	object? ISocket.Value => Value;
	public event Action? OnConnection;
	public event Action? OnDisconnection;
	internal void NotifyConnection() => OnConnection?.Invoke();
	internal void NotifyDisconnection() => OnDisconnection?.Invoke();
	Type ISocket.GetType() => typeof(T);
}