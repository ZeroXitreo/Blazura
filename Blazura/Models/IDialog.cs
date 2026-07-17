using Microsoft.AspNetCore.Components;

namespace Blazura.Models;

internal interface IDialog : IDisposable
{
    public RenderFragment ChildContent { get; set; }
    public bool IsOpen { get; set; }
    public void Open();
    public Task Close();
}
