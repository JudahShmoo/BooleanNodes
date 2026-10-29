# ./NodeTester/NodeTester.csproj
```csharp
﻿<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\Nodes\Nodes.csproj" />
  </ItemGroup>

</Project>
```

# ./NodeTester/Program.cs
```csharp
﻿using Nodes;

NodeGraph graph = new();
```

# ./Nodes/BasicNodes/AndNode.cs
```csharp
﻿namespace Nodes.BasicNodes;
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
}```

# ./Nodes/Connection.cs
```csharp
﻿namespace Nodes;

public interface IConnection
{
	IOutputSocket Output { get; }
	IInputSocket Input { get; }
}
public class Connection<T>(OutputSocket<T> output, InputSocket<T> input) : IConnection
{
	public OutputSocket<T> Output { get; } = output;
	public InputSocket<T> Input { get; } = input;
	IOutputSocket IConnection.Output => Output;
	IInputSocket IConnection.Input => Input;
}```

# ./Nodes/InputSocket.cs
```csharp
﻿namespace Nodes;

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
}```

# ./Nodes/Node.cs
```csharp
﻿namespace Nodes;
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
}```

# ./Nodes/NodeGraph.cs
```csharp
﻿namespace Nodes;
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
		connections.RemoveAll(c => c.Input.Owner == node || c.Output.Owner == node);
	}

	public Connection<T> Connect<T>(OutputSocket<T> output, InputSocket<T> input)
	{
		if (GetConnection(input) is not null) throw new InvalidOperationException("Input socket is already connected.");
		Connection<T> connection = new(output, input);
		connections.Add(connection);
		OnConnection?.Invoke(connection);
		return connection;
	}
	public void Disconnect(IConnection connection)
	{
		connections.Remove(connection);
		OnDisconnection?.Invoke(connection);
	}
	public IConnection? GetConnection(ISocket socket) => connections.FirstOrDefault(c => c.Input == socket || c.Output == socket);
	public Connection<T>? GetConnection<T>(Socket<T> socket) => GetConnection(socket);
	public IEnumerable<IConnection> GetConnections(IOutputSocket output) => connections.Where(c => c.Output == output);
}```

# ./Nodes/Nodes.csproj
```csharp
﻿<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

# ./Nodes/OutputSocket.cs
```csharp
﻿namespace Nodes;

public interface IOutputSocket : ISocket { }
public class OutputSocket<T> : Socket<T>, IOutputSocket
{
	public OutputSocket(Node owner, string name, Func<T> computeValue) : base(owner, name)
	{
		Owner.OnInputChanged += () =>
		{
			T newValue = computeValue();
			if (!EqualityComparer<T>.Default.Equals(value, newValue))
			{
				value = newValue;
				ValueChanged?.Invoke();
			}
		};
	}
	private T value = default!;
	public override T Value => value;

	public override event Action? ValueChanged;
}```

# ./Nodes/Socket.cs
```csharp
﻿namespace Nodes;

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
}```

