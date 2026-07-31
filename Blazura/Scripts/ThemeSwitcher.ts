class ThemeSwitcher implements IClassBringer
{
	private container: HTMLElement = undefined!;

	initialize(container: HTMLElement)
	{
		this.container = container;
		if (this.isDark())
		{
			this.turnDark();
		}
		else
		{
			this.turnLight();
		}
		this.container.addEventListener("click", this.mouseClick.bind(this));
	}

	private isDark()
	{
		return localStorage.getItem("theme") == "dark";
	}

	private turnDark()
	{
		localStorage.setItem("theme", "dark");
		this.container.classList.add("dark");
		document.documentElement.dataset.theme = "dark";
	}

	private turnLight()
	{
		localStorage.setItem("theme", "light");
		document.cookie = "dark=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
		delete document.documentElement.dataset.theme;
		this.container.classList.remove("dark");
	}

	private mouseClick(event: MouseEvent)
	{
		event.preventDefault();
		const cWidth = document.documentElement.clientWidth;
		const cHeight = document.documentElement.clientHeight;
		const diameter = Math.sqrt(cWidth ** 2 + cHeight ** 2);
		const calculatedWidth = diameter / cWidth;

		let ripple = document.createElement("div");
		ripple.classList.add("ThemeSwitcherRipple");
		ripple.setAttribute("style", `--extraSizing: ${calculatedWidth};`);
		ripple.style.left = event.clientX - document.body.offsetLeft + "px";
		ripple.style.top = event.clientY - document.body.offsetTop + "px";
		ripple.addEventListener("transitionend", this.transitionEnd.bind(this));
		ripple.addEventListener("animationend", this.animationEnd.bind(this));

		if (!this.isDark())
		{
			ripple.classList.add("dark");
		}

		document.body.appendChild(ripple);
	}

	private animationEnd(event: AnimationEvent)
	{
		console.log("ass");
		if (this.isDark())
		{
			this.turnLight();
		}
		else
		{
			this.turnDark();
		}
		this.removeRipples();
	}

	private removeRipples()
	{
		const ripples = document.body.getElementsByClassName("ThemeSwitcherRipple");
		for (let i = 0; i < ripples.length; i++)
		{
			ripples[i].classList.add("release");
		}
	}

	private transitionEnd(event: TransitionEvent)
	{
		(<HTMLElement>event.target).parentNode?.removeChild(<HTMLElement>event.target);
	}
}

ClassBringer.register(ThemeSwitcher);
