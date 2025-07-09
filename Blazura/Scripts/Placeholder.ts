class Placeholder implements IClassBringer
{
	private container: HTMLElement;

	initialize(container: HTMLElement)
	{
		this.container = container;

		const observer = new MutationObserver(this.callback.bind(this));

		observer.observe(this.container, { attributes: true, childList: true, subtree: true });
	}

	private callback()
	{
		const invalidElement = this.container.querySelector(".invalid");
		this.container.classList.toggle("error", !!invalidElement);
	}
}

ClassBringer.register(Placeholder);
