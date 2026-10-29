namespace Nodes;
public class NodeGraph
{
	private readonly List<Node> nodes = [];
	private readonly List<IConnection> connections = [];

	public IReadOnlyList<Node> Nodes => nodes;
	public IReadOnlyList<IConnection> Connections => connections;

	public event Action<IConnection>? OnConnection;
	public event Action<IConnection>? OnDisconnection;

	public void AddNode(Node node)
	{
		nodes.Add(node);
		node.Graph = this;
	}
	public void RemoveNode(Node node)
	{
		node.Graph = null;
		nodes.Remove(node);
		foreach (IConnection connection in connections.Where(c => c.Input.Owner == node || c.Output.Owner == node))
			Disconnect(connection);
	}

	public Connection<T> Connect<T>(OutputSocket<T> output, InputSocket<T> input)
	{
		if (GetConnection(input) is not null) throw new InvalidOperationException("Input socket is already connected.");
		Connection<T> connection = new(output, input);
		connections.Add(connection);
		OnConnection?.Invoke(connection);
		connection.NotifyConnection();
		return connection;
	}
	public void Disconnect(IConnection connection)
	{
		connections.Remove(connection);
		connection.NotifyDisconnection();
		OnDisconnection?.Invoke(connection);
	}
	public IConnection? GetConnection(ISocket socket) => connections.FirstOrDefault(c => c.Input == socket || c.Output == socket);
	public Connection<T>? GetConnection<T>(Socket<T> socket) => connections.Cast<Connection<T>>().FirstOrDefault(c => c.Input == socket || c.Output == socket);
	public IEnumerable<IConnection> GetConnections(IOutputSocket output) => connections.Where(c => c.Output == output);
}