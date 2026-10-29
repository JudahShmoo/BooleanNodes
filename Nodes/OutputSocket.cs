namespace Nodes;

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
}