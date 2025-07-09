namespace Blazura;

public interface IInputFormatter
{
    public Func<string, string> Format { get; }
}
