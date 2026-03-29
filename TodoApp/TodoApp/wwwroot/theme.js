(function () {
    const storageKey = "todoapp-theme";

    function systemPrefersDark() {
        return !!(window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches);
    }

    function isDarkActive() {
        const explicit = document.documentElement.dataset.theme;
        if (explicit === "dark") return true;
        if (explicit === "light") return false;
        return systemPrefersDark();
    }

    function apply(theme) {
        if (theme === "dark" || theme === "light") {
            document.documentElement.dataset.theme = theme;
        } else {
            delete document.documentElement.dataset.theme;
        }
        updateButton();
    }

    function toggle() {
        const next = isDarkActive() ? "light" : "dark";
        try {
            localStorage.setItem(storageKey, next);
        } catch {
            // ignore
        }
        apply(next);
    }

    function updateButton() {
        const button = document.getElementById("theme-toggle");
        if (!button) return;
        const label = button.querySelector(".theme-toggle-text");

        const dark = isDarkActive();
        button.setAttribute("aria-pressed", dark ? "true" : "false");
        button.setAttribute("aria-label", dark ? "Switch to light mode" : "Switch to dark mode");
        button.title = dark ? "Switch to light mode" : "Switch to dark mode";

        if (label) {
            label.textContent = dark ? "Switch to light mode" : "Switch to dark mode";
        }
    }

    function init() {
        let storedTheme = null;
        try {
            storedTheme = localStorage.getItem(storageKey);
        } catch {
            // ignore
        }

        if (storedTheme === "light" || storedTheme === "dark") {
            apply(storedTheme);
        } else {
            updateButton();
        }

        if (window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").addEventListener) {
            window.matchMedia("(prefers-color-scheme: dark)").addEventListener("change", function () {
                let hasStored = false;
                try {
                    hasStored = !!localStorage.getItem(storageKey);
                } catch {
                    // ignore
                }
                if (!hasStored) updateButton();
            });
        }
    }

    window.todoTheme = { apply, toggle, updateButton };

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", init);
    } else {
        init();
    }
})();
