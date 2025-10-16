function GenerateMapItems_DataformStep2(map, choice) {

// Initialize the feature group that will hold the drawn shapes
    var drawnItems = new L.FeatureGroup();
    map.addLayer(drawnItems);

// Creates the drawing control on the left hand side
// The drawing control enables drawing of markers, polygons, and polylines


    if (choice == 0 || choice == 3 || choice == 4 || choice == 5) {
        var drawControl = new L.Control.Draw({
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
    } else {
        var drawControl = new L.Control.Draw({
            draw: {
                polygon: false,
                polyline: false,
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
    }

    var form = document.getElementById('DataformStep2');

// --- Capture location data ---
    function captureData() {

        // Convert all layers to one GeoJSON object 
        var geojson = drawnItems.toGeoJSON();

        // Save JSON text to hidden field 
        document.getElementById('GeometryGeoJson').value = JSON.stringify(geojson);

        // Save preview text to hidden field
        document.getElementById('CoordinatesPreview').value = JSON.stringify(geojson);
    }

// --- Delete location data ---
    function deleteData() {
        drawnItems.clearLayers();
        document.getElementById('GeometryGeoJson').value = "";
        document.getElementById('CoordinatesPreview').value = "";
    }

    function onCreated(e) {
        drawnItems.addLayer(e.layer);
        captureData();
    }

    function onEdited(e) {
        captureData();
    }

    function onDeleted(e) {
        deleteData();
    }

// Event handlers
    map.on(L.Draw.Event.CREATED, onCreated);
    map.on(L.Draw.Event.EDITED, onEdited);
    map.on(L.Draw.Event.DELETED, onDeleted);

// marker validation: ensure at least one marker exists
    form.addEventListener('nextpage', function (ev) {
        if (drawnItems.getLayers().length === 0) {
            ev.preventDefault();   // stops submission when the condition is true
            alert('Please add a marker on the map before submitting.');
        } else {
            captureData(); // ensure hidden field is updated
        }
    });
}