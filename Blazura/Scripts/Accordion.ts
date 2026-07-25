var openAccordion = function (id: string) {
    var content = document.getElementById(id) as HTMLDivElement;
    content.animate([
        {
            height: content.offsetHeight + 'px', // Current height
        },
        {
            height: Accordion.calculateHeightWithoutAbsolutePosition(content) + 'px', // Open
        }
    ], Accordion.animationOptions);
    console.log("openAccordion", id, content);
}

var closeAccordion = function (id: string) {
    var content = document.getElementById(id) as HTMLDivElement;
    content.animate([
        {
            height: content.offsetHeight + 'px', // Current height
        },
        {
            height: 0 + 'px', // Closed
        }
    ], Accordion.animationOptions);
    console.log("closeAccordion", id, content);
}

class Accordion implements IClassBringer {
    static animationOptions: KeyframeAnimationOptions = {
        duration: 200,
        easing: "ease",
    }

    initialize(_element: HTMLElement): void {
    }

    static calculateHeightWithoutAbsolutePosition(container: HTMLElement) {
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
