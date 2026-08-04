class ThemeSwitcher implements IClassBringer {
    private container: HTMLElement = undefined!;

    initialize(container: HTMLElement) {
        this.container = container;
        this.container.addEventListener("click", this.mouseClick.bind(this));
    }

    private mouseClick(event: MouseEvent) {
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

        if (getTheme() === Theme.light) {
            ripple.classList.add("dark");
        }

        document.body.appendChild(ripple);
    }

    private animationEnd(event: AnimationEvent) {
        var setToTheme = getTheme() === Theme.dark ? Theme.light : Theme.dark;
        setTheme(setToTheme === systemTheme() ? undefined : setToTheme);
        this.removeRipples();
    }

    private removeRipples() {
        const ripples = document.body.getElementsByClassName("ThemeSwitcherRipple");
        for (let i = 0; i < ripples.length; i++) {
            ripples[i].classList.add("release");
        }
    }

    private transitionEnd(event: TransitionEvent) {
        (<HTMLElement>event.target).parentNode?.removeChild(<HTMLElement>event.target);
    }
}

ClassBringer.register(ThemeSwitcher);
