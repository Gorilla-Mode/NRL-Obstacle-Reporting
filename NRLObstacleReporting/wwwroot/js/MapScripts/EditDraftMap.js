

//leaflet 
document.addEventListener('DOMContentLoaded', function() {


    var N100topo = L.tileLayer('https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png', {
        maxZoom: 18,
        minZoom: 4,
        attribution: '&copy; <a href="https://cache.kartverket.no/">Kartverket</a>'
    })

    var S100topo = L.tileLayer('https://geodata.npolar.no/arcgis/rest/services/Basisdata/NP_Basiskart_Svalbard_WMTS_3857/MapServer/WMTS/tile/1.0.0/Basisdata_NP_Basiskart_Svalbard_WMTS_3857/default/default028mm/{z}/{y}/{x}', {
        maxZoom: 15,
        minZoom: 4,
        attribution: 'Map data &copy; <a href="https://geodata.npolar.no/">Norsk Polarinstitutt</a>'
    })

    var J100topo = L.tileLayer('https://geodata.npolar.no/arcgis/rest/services/Basisdata/NP_Basiskart_JanMayen_WMTS_3857/MapServer/WMTS/tile/1.0.0/Basisdata_NP_Basiskart_JanMayen_WMTS_3857/default/default028mm/{z}/{y}/{x}', {
        maxZoom: 16,
        minZoom: 4,
        attribution: 'Map data &copy; <a href="https://geodata.npolar.no/">Norsk Polarinstitutt</a>'
    })

    let baseMaps = {
        "S100 Topographic": S100topo,
        "J100 Topographic": J100topo,
        "N100 Topographic": N100topo
    };

    var map = L.map('map', {
        layers: [N100topo],
    }).setView([58.14671, 7.9956], 17);

    L.control.layers(baseMaps).addTo(map);

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
    var geojsonLayer = L.geoJSON(geojson, 
{
            style: function ()
            {
                return{color: 'purple', weight: 6, opacity: 1}
            }
        });
    
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
        polyline: { shapeOptions: { color: 'purple', weight: 6, opacity: 1 } },
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
