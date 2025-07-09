class AdaptiveImage implements IClassBringer {
    private container: HTMLImageElement;

    initialize(container: HTMLImageElement) {
        this.container = container;

        window.addEventListener("scroll", this.onScroll.bind(this));
        window.addEventListener("resize", this.onScroll.bind(this));
        this.onScroll();
    }

    private onScroll() {
        const bounds = this.container.getBoundingClientRect();

        if (bounds.bottom < 0) return;
        if (window.innerHeight - bounds.top < 0) return;


        if (this.getScaledImageHeight > window.innerHeight) {
            const procentage = bounds.bottom / (window.innerHeight + this.container.offsetHeight);
            this.container.style.objectPosition = `center ${((1 - procentage) * this.container.offsetHeight) - (procentage * this.getScaledImageHeight)}px`;
        }
        else {
            const procentage = bounds.top / (window.innerHeight - this.container.offsetHeight);
            this.container.style.objectPosition = `center ${procentage * 100}%`;
        }
    }

    private get getScaledImageHeight() {
        return this.container.naturalHeight * this.getImageScale;
    }

    private get getImageScale() {
        return this.container.offsetWidth / this.container.naturalWidth;
    }
}

ClassBringer.register(AdaptiveImage);
