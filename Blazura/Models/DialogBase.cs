using Blazura.Components;
using Microsoft.AspNetCore.Components;

namespace Blazura.Models;

public abstract class DialogBase : ComponentBase, IDialog
{
    [CascadingParameter]
    private DialogManager? Manager { get; set; }

    [Parameter]
    public required RenderFragment ChildContent { get; set; }

    public bool IsOpen { get; set; }

    public void Dispose()
    {
        Manager?.Unregister(this);
        GC.SuppressFinalize(this);
    }

    public async Task Open()
    {
        IsOpen = true;
        if (Manager is not null)
        {
            await Manager.TriggerRender();
        }
        Manager?.RunBackdropCheck();
    }

    public async Task Close()
    {
        IsOpen = false;
        if (Manager is not null)
        {
            await Manager.TriggerRender();
        }
        Manager?.RunBackdropCheck();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Manager?.Register(this);
        }

        if (Manager is not null)
        {
            await Manager.TriggerRender();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}
