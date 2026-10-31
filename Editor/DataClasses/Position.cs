namespace Editor.DataClasses;
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