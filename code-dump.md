# ./Editor/DataClasses/Position.cs

```csharp
﻿namespace Editor.DataClasses;
public struct Position(float x, float y)
{
	public float X { get; set; } = x;
	public float Y { get; set; } = y;

	public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
	public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
	public static Position operator *(Position a, float scalar) => new(a.X * scalar, a.Y * scalar);
	public static Position operator /(Position a, float scalar) => new(a.X / scalar, a.Y / scalar);
	public static Position operator *(float scalar, Position a) => a * scalar;
	public static Position operator /(float scalar, Position a) => new(scalar / a.X, scalar / a.Y);
	public static bool operator ==(Position a, Position b) => a.X == b.X && a.Y == b.Y;
	public static bool operator !=(Position a, Position b) => !(a == b);

	public readonly override bool Equals(object? obj) => obj is Position position && this == position;
	public readonly override int GetHashCode() => HashCode.Combine(X, Y);

	public static Position Zero => new(0, 0);
	public static Position One => new(1, 1);
}
```

# ./Editor/Editor.csproj

```csharp
<Project Sdk="Microsoft.NET.Sdk.BlazorWebAssembly">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <OverrideHtmlAssetPlaceholders>true</OverrideHtmlAssetPlaceholders>
    <ServiceWorkerAssetsManifest>service-worker-assets.js</ServiceWorkerAssetsManifest>
  </PropertyGroup>

  <ItemGroup>
    <Content Remove="save.schema.json" />
  </ItemGroup>

  <ItemGroup>
    <None Include="save.schema.json" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.13" />
    <PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.DevServer" Version="10.0.13" PrivateAssets="all" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Nodes\Nodes.csproj" />
  </ItemGroup>

  <ItemGroup>
    <ServiceWorker Include="wwwroot\service-worker.js" PublishedContent="wwwroot\service-worker.published.js" />
  </ItemGroup>

  <ProjectExtensions><VisualStudio><UserProperties testsave_1json__JsonSchema="..\..\..\c#\BooleanNodes\Editor\save.schema.json" /></VisualStudio></ProjectExtensions>

</Project>

```

# ./Editor/Program.cs

```csharp
using Editor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();

```

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
using Nodes.BasicNodes;

NodeGraph graph = new();

AndNode and1 = new();
AndNode and2 = new();
NorNode nor = new();
XorNode xor = new();

graph.AddNode(and1);
graph.AddNode(and2);
graph.AddNode(nor);
graph.AddNode(xor);

graph.Connect(and1.Output, and2.A);
graph.Connect(nor.Output, and2.B);
graph.Connect(xor.Output, nor.A);

void LogValues()
{
	Console.WriteLine($"And1 - A: {and1.A.Value}, B: {and1.B.Value}, Output: {and1.Output.Value}");
	Console.WriteLine($"And2 - A: {and2.A.Value}, B: {and2.B.Value}, Output: {and2.Output.Value}");
	Console.WriteLine($"Nor - A: {nor.A.Value}, B: {nor.B.Value}, Output: {nor.Output.Value}");
	Console.WriteLine($"Xor - A: {xor.A.Value}, B: {xor.B.Value}, Output: {xor.Output.Value}");
	Console.WriteLine("--------------------------------------------------");
}

LogValues();
and1.A.DefaultValue = true;
LogValues();
and1.B.DefaultValue = true;
LogValues();
and2.A.DefaultValue = true;
LogValues();
and2.B.DefaultValue = true;
LogValues();
nor.A.DefaultValue = true;
LogValues();
nor.B.DefaultValue = true;
LogValues();
xor.A.DefaultValue = true;
LogValues();
xor.B.DefaultValue = true;
LogValues();
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
}
```

# ./Nodes/BasicNodes/NorNode.cs

```csharp
﻿namespace Nodes.BasicNodes;
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
```

# ./Nodes/BasicNodes/OrNode.cs

```csharp
﻿namespace Nodes.BasicNodes;
public class OrNode : Node
{
	public OrNode() : base("Or")
	{
		A = AddInput("A", false);
		B = AddInput("B", false);
		Output = AddOutput("Output", () => A.Value || B.Value);
	}
	public InputSocket<bool> A { get; }
	public InputSocket<bool> B { get; }
	public OutputSocket<bool> Output { get; }
}
```

# ./Nodes/BasicNodes/XorNode.cs

```csharp
﻿namespace Nodes.BasicNodes;
public class XorNode : Node
{
	public XorNode() : base("Xor")
	{
		A = AddInput("A", false);
		B = AddInput("B", false);
		Output = AddOutput("Output", () => A.Value ^ B.Value);
	}
	public InputSocket<bool> A { get; }
	public InputSocket<bool> B { get; }
	public OutputSocket<bool> Output { get; }
}
```

# ./Nodes/Connection.cs

```csharp
﻿namespace Nodes;

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
```

# ./Nodes/InputSocket.cs

```csharp
﻿namespace Nodes;

public interface IInputSocket : ISocket { }
public class InputSocket<T> : Socket<T>, IInputSocket
{
	private void InvokeValueChanged() => ValueChanged?.Invoke();
	public InputSocket(Node owner, string name) : base(owner, name)
	{
		OnConnection += InvokeValueChanged;
		OnConnection += () => Connection!.Output.ValueChanged += InvokeValueChanged;
		OnDisconnection += InvokeValueChanged;
		OnDisconnection += () => Connection!.Output.ValueChanged -= InvokeValueChanged;
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
```

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

	public event Action? OnInputChanged;

	protected InputSocket<T> AddInput<T>(string name, T defaultValue)
	{
		InputSocket<T> input = new(this, name)
		{ DefaultValue = defaultValue };
		input.ValueChanged += () => OnInputChanged?.Invoke();
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
```

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
```

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
		value = computeValue();
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
	private T value;
	public override T Value => value;

	public override event Action? ValueChanged;
}
```

# ./Nodes/Socket.cs

```csharp
﻿namespace Nodes;

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
```

