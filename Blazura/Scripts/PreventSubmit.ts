class PreventSubmit implements IClassBringer
{
	private container: HTMLElement;

	initialize(container: HTMLElement)
	{
		this.container = container;
		this.container.addEventListener("keydown", this.KeyDown.bind(this));
	}

	private KeyDown(event: KeyboardEvent)
	{
		if (event.code == "Enter" || event.key == "Enter" || event.code == "NumpadEnter")
		{
			event.preventDefault();
		}
	}
}

ClassBringer.register(PreventSubmit);
