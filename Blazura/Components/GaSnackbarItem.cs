using Blazura.Models;

namespace Blazura.Components;
public class GaSnackbarItem
{
    public string Text { get; set; } = default!;

    public SnackbarType Type { get; set; } = SnackbarType.Default;

    public bool Dismissed { get; set; }
}
