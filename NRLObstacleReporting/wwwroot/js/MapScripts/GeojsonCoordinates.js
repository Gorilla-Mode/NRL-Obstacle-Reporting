/**
 * Creates a map and places marker on current location and sets geojson field to this.
 * @constructor
 */
function GetGeoJsonCoordinates()
{
    var map = L.map('map').setView([58.14671, 7.9956], 12);

    L.tileLayer('https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/{z}/{x}/{y}.png', {
        maxZoom: 19,
        minZoom: 4,
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
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

