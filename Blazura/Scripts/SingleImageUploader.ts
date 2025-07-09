class SingleImageUploader implements IClassBringer
{
	private container: HTMLInputElement;
	private image: HTMLImageElement;

	initialize(container: HTMLInputElement)
	{
		this.container = container;
		this.container.addEventListener("change", this.onChange.bind(this));

		// Create img
		this.image = <HTMLImageElement>this.container.nextElementSibling;
	}

	private onChange()
	{
		if (this.container.files[0])
		{
			this.image.src = URL.createObjectURL(this.container.files[0]);
		}
		else
		{
			URL.revokeObjectURL(this.image.src);
			this.image.src = null;
		}
	}
}

ClassBringer.register(SingleImageUploader);
