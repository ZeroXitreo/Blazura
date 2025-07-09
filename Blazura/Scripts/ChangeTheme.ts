const enum Theme
{
	dark = "dark",
	light = "light"
}

const localStorageName = "theme";

function getTheme(): Theme | undefined
{
	const localDarkTheme = localStorage.getItem(localStorageName)
	if (localDarkTheme == Theme.dark)
	{
		return Theme.dark;
	}
	if (localDarkTheme == Theme.light)
	{
		return Theme.light;
	}
}

function changeTheme(isDarkTheme?: boolean)
{
	if (isDarkTheme != undefined)
	{
		if (isDarkTheme)
		{
			localStorage.setItem(localStorageName, Theme.dark);
		}
		else
		{
			localStorage.setItem(localStorageName, Theme.light);
		}
	}
	else
	{
		localStorage.removeItem(localStorageName);
	}
	applyTheme();
}

/**
 * @deprecated The method should not be used, please use applyTheme instead
 */
function triggerTheme() {
	applyTheme();
}

function applyTheme() {
	if (document.documentElement.dataset.themelock == "true") return;

	let theme = localStorage.getItem(localStorageName)

	if (theme === null && window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        theme = Theme.dark;
	}

	if (theme == Theme.dark && document.documentElement.dataset.theme != Theme.dark) {
		document.documentElement.dataset.theme = Theme.dark;
	}
	if (theme == Theme.light && document.documentElement.dataset.theme != undefined) {
		delete document.documentElement.dataset.theme;
	}
}

(window as any).getTheme = getTheme

applyTheme();

(new MutationObserver(applyTheme)).observe(document.documentElement, { attributes: true });
