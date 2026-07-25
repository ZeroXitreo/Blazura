var openAccordion = function (id: string) {
    Accordion.openAccordion(id);
}

var closeAccordion = function (id: string) {
    Accordion.closeAccordion(id);
}

abstract class Accordion {
    static animationOptions: KeyframeAnimationOptions = {
        duration: 200,
        easing: "ease",
    }

    static openAccordion(id: string) {
        var content = document.getElementById(id) as HTMLDivElement;
        content.animate([
            {
                height: `${content.offsetHeight}px`, // Current height
            },
            {
                height: `${Accordion.calculateHeightWithoutAbsolutePosition(content)}px`, // Open
            }
        ], Accordion.animationOptions);
    }

    static closeAccordion(id: string) {
        var content = document.getElementById(id) as HTMLDivElement;
        content.animate([
            {
                height: `${content.offsetHeight}px`, // Current height
            },
            {
                height: '0px', // Closed
            }
        ], Accordion.animationOptions);
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
