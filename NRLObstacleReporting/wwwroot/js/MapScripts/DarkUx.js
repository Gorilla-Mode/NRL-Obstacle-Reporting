const toggle = document.getElementById("theme-toggle");
const circle = document.getElementById("toggle-circle");
const root = document.documentElement;

// Sett riktig posisjon på sirkelen ved load
if (root.classList.contains("dark")) {
    circle?.classList.add("translate-x-full");
}

// Når du trykker på toggle
toggle?.addEventListener("click", () => {
    const isDark = root.classList.toggle("dark");

    if (isDark) {
        circle?.classList.add("translate-x-full");
        localStorage.setItem("theme", "dark");
    } else {
        circle?.classList.remove("translate-x-full");
        localStorage.setItem("theme", "light");
    }
});
