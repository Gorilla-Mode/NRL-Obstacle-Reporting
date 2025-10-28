/**
 * Generates a leaflet layer and adds it to a map variable which is centered on a location
 * @returns a leaflet map variable
 * @
 */
function GenerateMap()
{
     var map = L.map('map').setView([58.14671, 7.9956], 17);

    L.tileLayer('https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);
    map.addControl(
        L.control.locate({
            locateOptions: {
                enableHighAccuracy: true
            }
        })
    );
    return map;
}