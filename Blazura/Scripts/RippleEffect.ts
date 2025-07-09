class RippleEffect implements IClassBringer
{
	private container: HTMLElement;

	initialize(container: HTMLElement)
	{
		this.container = container;
		this.container.addEventListener("mousedown", this.MouseDown.bind(this));
		this.container.addEventListener("mouseup", this.RemoveRipples.bind(this));
		this.container.addEventListener("mouseleave", this.RemoveRipples.bind(this));
		this.container.addEventListener("keydown", this.KeyDown.bind(this));
	}

	private KeyDown(event: KeyboardEvent)
	{
		if (event.code == "Enter" || event.key == "Enter" || event.code == "NumpadEnter")
		{
			this.MouseDown();
			this.RemoveRipples();
		}
	}

	private MouseDown(event?: MouseEvent)
	{
		this.container.focus();
		const cWidth = this.container.clientWidth;
		const cHeight = this.container.clientHeight;
		const diameter = Math.sqrt(cWidth ** 2 + cHeight ** 2);
		const calculatedWidth = diameter / cWidth;
		const bounds = this.container.getBoundingClientRect();

		let ripple = document.createElement("div");
		ripple.classList.add("ripple");
		ripple.setAttribute("style", `--extraSizing: ${calculatedWidth};`);

		let clientX = this.container.offsetWidth / 2;
		let clientY = this.container.offsetHeight / 2;
		if (event)
		{
			event.preventDefault();
			clientX = event.clientX - bounds.left;
			clientY = event.clientY - bounds.top;
		}
		ripple.style.left = clientX + "px";
		ripple.style.top = clientY + "px";

		ripple.addEventListener("transitionend", this.TransitionEnd.bind(this));
		this.container.appendChild(ripple);
	}

	private RemoveRipples()
	{
		requestAnimationFrame(() =>
		{
			requestAnimationFrame(() =>
			{
				const ripples = this.container.getElementsByClassName("ripple");
				for (let i = 0; i < ripples.length; i++)
				{
					ripples[i].classList.add("release");
				}
			});
		});
	}

	private TransitionEnd(event: TransitionEvent)
	{
		(<HTMLElement>event.target).parentNode?.removeChild(<HTMLElement>event.target);
	}
}

ClassBringer.register(RippleEffect);
