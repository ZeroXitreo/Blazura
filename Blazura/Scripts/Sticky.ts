class Sticky implements IClassBringer {
    private container!: HTMLElement;
    private filler!: HTMLElement;
    private previousScrollHeight: number = 0;

    initialize(container: HTMLElement) {
        this.container = container.getElementsByClassName("content")[0] as HTMLElement;
        window.addEventListener("scroll", this.OnScroll.bind(this));

        this.filler = document.createElement("div");
        this.container.parentElement!.insertBefore(this.filler, this.container.parentElement!.firstChild);

        this.OnScroll();
    }

    private OnScroll() {
        let scrollHeight = window.scrollY;

        let topMargin = parseInt(window.getComputedStyle(this.container.parentElement!).marginTop, 10);
        let bottomMargin = parseInt(window.getComputedStyle(this.container.parentElement!).marginBottom, 10);

        let heightAdjust = (this.container.offsetTop - this.container.parentElement!.offsetTop) + "px";
        let topAdjust = (document.documentElement.clientHeight - this.container.offsetHeight - topMargin) + "px";
        let bottomAdjust = (document.documentElement.clientHeight - this.container.offsetHeight - bottomMargin) + "px";

        //console.log(document.documentElement.clientHeight - this.container.offsetHeight - topMargin - bottomMargin);
        if ((document.documentElement.clientHeight - this.container.offsetHeight - topMargin - bottomMargin) <= 0) {
            if (this.previousScrollHeight < scrollHeight) {
                // Down
                if (!this.previousScrollHeight || this.container.style.bottom) {
                    this.container.style.top = topAdjust;
                    this.container.style.bottom = null!;
                    this.filler.style.height = heightAdjust;
                }
            }
            else {
                // Up
                if (!this.previousScrollHeight || this.container.style.top) {
                    this.container.style.bottom = bottomAdjust;
                    this.container.style.top = null!;
                    this.filler.style.height = heightAdjust;
                }
            }
        }
        else {
            this.container.style.bottom = null!;
            this.container.style.top = topMargin + "px";
            this.filler.style.height = null!;
        }

        this.previousScrollHeight = scrollHeight;
    }
}

ClassBringer.register(Sticky);
