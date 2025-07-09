using Blazura.Components;

namespace Blazura.Models;

public interface IModalContainer
{
    public ModalManager ModalManager { get; set; }
    public Modal ModalRef { get; set; }

    //public static abstract void Open(ModalManager? modalManager, IStringLocalizer<T> localizer);
}
