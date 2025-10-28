/**
 * Generates a leaflet layer and adds it to a map variable which is centered on a location
 * @returns a leaflet map variable
 * @
 */
function GenerateMap(zoom)
{
     var map = L.map('map').setView([58.14671, 7.9956], zoom);

    L.tileLayer('https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/{z}/{x}/{y}.png', {
        maxZoom: 19,
        minZoom: 4,
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);
    
    return map;
}

function GenerateMapGeolocate(zoom)
{
    var map = L.map('map').setView([58.14671, 7.9956], zoom);

    L.tileLayer('https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/{z}/{x}/{y}.png', {
        maxZoom: 19,
        minZoom: 4,
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