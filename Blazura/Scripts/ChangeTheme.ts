const enum Theme {
    dark = "dark",
    light = "light"
}

const localStorageName = "theme";

function getTheme() {
    const storedTheme = localStorage.getItem(localStorageName)
    switch (storedTheme) {
        case Theme.dark:
            return Theme.dark;
        case Theme.light:
            return Theme.light;
        default:
    }
    return systemTheme();
}

function setTheme(theme?: Theme) {
    if (theme != undefined) {
        if (theme === Theme.dark) {
            localStorage.setItem(localStorageName, Theme.dark);
        }
        else {
            localStorage.setItem(localStorageName, Theme.light);
        }
    }
    else {
        localStorage.removeItem(localStorageName);
    }
    applyTheme();
}

function systemTheme() {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        return Theme.dark;
    }
    return Theme.light;
}

function applyTheme() {
    if (document.documentElement.dataset.themelock == "true") return;

    let theme = localStorage.getItem(localStorageName)

    if (theme === null) {
        theme = systemTheme();
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
