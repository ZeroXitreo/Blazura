class Accordion implements IClassBringer {
    private element!: HTMLElement;
    private content!: HTMLElement;
    private button!: HTMLElement;

    initialize(element: HTMLElement): void {
        this.element = element;

        this.button = this.element.getElementsByClassName("title")[0] as HTMLDivElement;
        this.content = this.element.getElementsByClassName("content")[0] as HTMLDivElement;
        this.button.addEventListener("click", this.toggle.bind(this));
    }

    toggle() {
        console.log("toggle");
        const animationOptions: KeyframeAnimationOptions = {
            duration: 200,
            easing: "ease",
        }
        if (this.content.classList.contains("opsen")) {
            this.content.animate([
                {
                    height: this.content.offsetHeight + 'px', // Current height
                },
                {
                    height: 0 + 'px', // Closed
                }
            ], animationOptions);
        }
        else {
            this.content.animate([
                {
                    height: this.content.offsetHeight + 'px', // Current height
                },
                {
                    height: this.calculateHeightWithoutAbsolutePosition(this.content) + 'px', // Open
                }
            ], animationOptions);
            this.content.animate([
                {
                    overflow: "hidden",
                },
                {
                    overflow: "initial",
                }
            ], {
                ...animationOptions,
                delay: animationOptions.duration as number,
                duration: 0,
                fill: "backwards",
            });
        }

        this.content.classList.toggle("opsen");
        this.button.classList.toggle("opsen", this.content.classList.contains("opsen"));
    }

    calculateHeightWithoutAbsolutePosition(container: HTMLElement) {
        // Select all elements inside the container
        var elements = container.children;

        // Filter out elements with position: absolute
        var nonAbsoluteElements: Element[] = [];
        for (var i = 0; i < elements.length; i++) {
            var computedStyle = window.getComputedStyle(elements[i]);
            if (computedStyle.position !== 'absolute') {
                nonAbsoluteElements.push(elements[i]);
            }
        }

        // Calculate the total height of non-absolute elements
        var totalHeight = 0;
        for (var j = 0; j < nonAbsoluteElements.length; j++) {
            totalHeight += (nonAbsoluteElements[j] as HTMLElement).offsetHeight;
        }

        return totalHeight;
    }
}

ClassBringer.register(Accordion);
