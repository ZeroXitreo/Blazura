class LivingText implements IClassBringer {
	private container: HTMLDivElement;

	initialize(container: HTMLDivElement) {
		this.container = container;

		for (var i = this.container.childNodes.length - 1; i >= 0; i--) {
			const child = this.container.childNodes[i]
			if (child.nodeType == Node.TEXT_NODE) {
				for (var j = child.textContent.length - 1; j >= 0; j--) {
					const char = child.textContent[j];
					const elm = document.createElement("span");
					elm.textContent = char;
					this.container.insertBefore(elm, this.container.firstChild);
					i++;
					//element.innerHTML = element.textContent
					//	.split('')
					//	.map((char, i) => `<span style="--char-index: ${Math.random() * 10}">${char}</span>`)
					//	.join('');
				}
				this.container.removeChild(child);
			}
        }
	}
}

ClassBringer.register(LivingText);
