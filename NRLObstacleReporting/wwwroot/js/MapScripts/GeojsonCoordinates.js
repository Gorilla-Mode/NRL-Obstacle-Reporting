/**
 * Creates a map and places marker on current location and sets geojson field to this.
 */
function GetGeoJsonCoordinates()
{
    var map = L.map('map').setView([58.14671, 7.9956], 12);

    L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png', {
        maxZoom: 19,
        minZoom: 4,
        attribution: '&copy; <a href="https://cache.kartverket.no/">© Kartverket</a>'
    }).addTo(map);

    let drawnItems = new L.FeatureGroup();
    map.addLayer(drawnItems);

    navigator.geolocation.getCurrentPosition(
        function(position)
        {
            let latitude = position.coords.latitude;
            let longitude = position.coords.longitude;

            L.marker([latitude, longitude]).addTo(drawnItems)
            let geojson = drawnItems.toGeoJSON();

            // Save JSON text to hidden field 
            document.getElementById('GeometryGeoJson').value = JSON.stringify(geojson);
        },
    );
}

