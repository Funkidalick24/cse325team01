(function () {
    const storageKey = "todoapp-theme";

    function getThemePreference() {
        try {
            const theme = localStorage.getItem(storageKey);
            return theme === "light" || theme === "dark" ? theme : "system";
        } catch {
            return "system";
        }
    }

    function setThemePreference(theme) {
        const normalized = theme === "light" || theme === "dark" ? theme : "system";
        try {
            if (normalized === "system") {
                localStorage.removeItem(storageKey);
            } else {
                localStorage.setItem(storageKey, normalized);
            }
        } catch {
            // ignore
        }

        if (window.todoTheme && typeof window.todoTheme.apply === "function") {
            window.todoTheme.apply(normalized === "system" ? null : normalized);
        }
    }

    function downloadFile(fileName, contentType, content) {
        const blob = new Blob([content], { type: contentType });
        const url = URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        link.remove();
        URL.revokeObjectURL(url);
    }

    window.taskflowSettings = {
        getThemePreference,
        setThemePreference,
        downloadFile
    };
})();
