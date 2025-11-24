/**
 * Generates a leaflet layer and adds it to a map variable which is centered on a location
 * @returns a leaflet map variable
 * @
 */
function GenerateMap(zoom)
{
     var map = L.map('map').setView([58.14671, 7.9956], zoom);

    L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png', {
        maxZoom: 19,
        minZoom: 4,
        attribution: '&copy; <a href="https://cache.kartverket.no/">© Kartverket</a>'
    }).addTo(map);
    
    return map;
}

/**
 * Generates a leaflet layer and adds it to a map variable which is centered on a location. uses high accuracy geolocation
 * @param zoom the zoom level of the map
 * @returns a leaflet map variable
 */
function GenerateMapGeolocate(zoom)
{
    var map = L.map('map').setView([58.14671, 7.9956], zoom);

    L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png', {
        maxZoom: 19,
        minZoom: 4,
        attribution: '&copy; <a href="https://cache.kartverket.no/">© Kartverket</a>'
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

/**
 * Generates a leaflet layer and adds it to a map variable. The view is bound by the inputted geoJson
 * @param zoom the zoom level on the map
 * @param Geojson Geojson to add to as a layer to the map
 * @returns A leaflet map, with geojson data from input
 */
function GenerateMapWithGeojson(zoom, Geojson)
{
    var map = L.map('map').setView([58.14671, 7.9956], zoom);

    L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png', {
        maxZoom: 19,
        minZoom: 4,
        attribution: '&copy; <a href="https://cache.kartverket.no/">© Kartverket</a>'
    }).addTo(map);

    let geojsonLayer = L.geoJSON(Geojson);
    
    geojsonLayer.addTo(map);
    map.setMaxBounds(geojsonLayer.getBounds())
    
    return map;
}