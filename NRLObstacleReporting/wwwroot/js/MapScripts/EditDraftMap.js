

//leaflet 
document.addEventListener('DOMContentLoaded', function() {
var map = L.map('map').setView([58.14671, 7.9956], 17);

//adjustment of map color
L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="https://cache.kartverket.no/">© Kartverket</a>'
}).addTo(map); //adds map object so its actually being shown

var drawnItems = new L.FeatureGroup();
map.addLayer(drawnItems);
function captureData() {

    // Convert all layers to one GeoJSON object 
    let geojson = drawnItems.toGeoJSON();

    // Save JSON text to hidden field 
    document.getElementById('GeometryGeoJson').value = JSON.stringify(geojson);

    // Save preview text to hidden field
    document.getElementById('CoordinatesPreview').value = JSON.stringify(geojson);
}
function deleteData() {
    drawnItems.clearLayers();
    document.getElementById('GeometryGeoJson').value = "";
    document.getElementById('CoordinatesPreview').value = "";
}

function onCreated(e) {
    drawnItems.addLayer(e.layer);
    captureData();
}

function onEdited() {
    captureData();
}

function onDeleted() {
    deleteData();
}

//Loads saved GeoJSON data, adds it to the map, fits the view, and updates the data fields
var geojsonText = document.getElementById("GeometryGeoJson")?.value;
var geojson = geojsonText ? JSON.parse(geojsonText) : null;
//collects data from geometryGeojson, makes it a valid javascript object
if (geojson) {
    var geojsonLayer = L.geoJSON(geojson);
    geojsonLayer.eachLayer(function(layer) {
        drawnItems.addLayer(layer); //legg alle eksisterende lag i drawnItems
    });
    map.fitBounds(geojsonLayer.getBounds());
    captureData();
}
//adds a toolbar
let drawControl = new L.Control.Draw({
    draw: {
        polygon: false,
        polyline: true,
        marker: true,
        circle: false,  // Disable circle drawing
        rectangle: false,
        circlemarker: false
    },
    edit: {
        featureGroup: drawnItems
    }
});
map.addControl(drawControl);


// Event handlers
map.on(L.Draw.Event.CREATED, onCreated);
map.on(L.Draw.Event.EDITED, onEdited);
map.on(L.Draw.Event.DELETED, onDeleted); 

const detailsElement = document.querySelector('details');
if (detailsElement) {
    detailsElement.addEventListener('toggle', function() {
        if (this.open) {
            // Small delay to ensure DOM has updated before resizing map
            setTimeout(function() {
                map.invalidateSize(); // Recalculates map container size
            }, 100);
        }
    });
}
});
