/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class', // du styrer dark mode med body.classList
  content: [
    './Pages/**/*.{html,cshtml}',
    './Views/**/*.{html,cshtml}',
    './wwwroot/**/*.html',
    './wwwroot/js/MapScripts/**/*.js'
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};
