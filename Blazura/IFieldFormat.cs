namespace Blazura;

public interface IFieldFormat
{
    public Func<string, string> Format { get; }
}
