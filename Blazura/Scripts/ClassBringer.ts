class ClassBringer
{
	private static instance: ClassBringer;

	private bringers: (new () => IClassBringer)[] = [];

	public static get Instance()
	{
		if (!ClassBringer.instance)
		{
			ClassBringer.instance = new ClassBringer();
		}

		return ClassBringer.instance;
	}

	private constructor()
	{
		//this.onContentLoaded()
		window.addEventListener('DOMContentLoaded', this.onContentLoaded.bind(this));
	}

	private onContentLoaded()
	{
		this.initiateElementTree(document.body);

		const observer = new MutationObserver(this.callback.bind(this));

		// https://developer.mozilla.org/en-US/docs/Web/API/MutationObserverInit
		const config = { attributes: true, childList: true, subtree: true };

		observer.observe(document.body, config);
	}

	private callback(mutationsList: MutationRecord[], observer: MutationObserver)
	{
		if (mutationsList)
		{
			for (var i = 0; i < mutationsList.length; i++)
			{
				let mutationRecord = mutationsList[i];
				let mutationTarget = mutationRecord.target;

				if (mutationTarget instanceof Element)
				{
					let element = mutationTarget;

					this.initiateElementTree(element);
				}
			}
		}
	}

	private initiateElementTree(element: Element)
	{
		for (var i = 0; i < this.bringers.length; i++)
		{
			let bringer = this.bringers[i];

			if (element.classList.contains((<any>bringer).name))
			{
				this.setBringer(element, bringer);
			}

			let elements = element.getElementsByClassName((<any>bringer).name);

			for (var j = 0; j < elements.length; j++)
			{
				let element = elements[j];

				this.setBringer(element, bringer);
			}
		}
	}

	private setBringer(element: Element, bringer: new () => IClassBringer)
	{
		if (!element[(<any>bringer).name])
		{
			element[(<any>bringer).name] = new bringer();
			(<IClassBringer>element[(<any>bringer).name]).initialize(element);
		}
	}

	public static register(bringer: new () => IClassBringer)
	{
		ClassBringer.Instance.bringers.push(bringer);
	}
}

ClassBringer.Instance;
