const toggle = document.getElementById("theme-toggle");
const circle = document.getElementById("toggle-circle");
const body = document.body;

// Sjekk localStorage for tidligere valg
const savedTheme = localStorage.getItem("theme");
if (savedTheme === "dark") {
    body.classList.add("dark");
    circle.classList.add("translate-x-full");
} else {
    body.classList.remove("dark");
    circle.classList.remove("translate-x-full");
}

// Når du trykker på toggle
toggle?.addEventListener("click", () => {
    body.classList.toggle("dark");
    const isDark = body.classList.contains("dark");

    if (isDark) {
        circle.classList.add("translate-x-full");
        localStorage.setItem("theme", "dark");
    } else {
        circle.classList.remove("translate-x-full");
        localStorage.setItem("theme", "light");
    }
});
