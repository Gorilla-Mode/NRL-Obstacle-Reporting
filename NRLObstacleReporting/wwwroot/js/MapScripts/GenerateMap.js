/**
 * 
 * @returns a map to use
 * @
 */
function GenerateMap()
{
     var map = L.map('map').setView([58.14671, 7.9956], 13);

    L.tileLayer('https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);
    
    return map;
}