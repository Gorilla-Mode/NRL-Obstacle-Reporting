/**
 * A function that customizes a leaflet map with drawn items and registers when the user draws on the map. Also verifies if the user has drawn on the map.
 * @param map the leaflet map variable you want to customize further
 * @param choice value of the obstacle-type you choose in DataformStem1
 * 
 */

function GenerateMapItems_DataformStep2(map, choice) {

// Initialize the feature group that will hold the drawn shapes
    let drawnItems = new L.FeatureGroup();
    map.addLayer(drawnItems);
    
    navigator.geolocation.getCurrentPosition(
        function(position)
        {
            let latitude = position.coords.latitude;
            let longitude = position.coords.longitude;

            L.marker([latitude, longitude]).addTo(drawnItems)
            captureData();
        },
    );

    if (choice === 0 || choice === 4 ) {
        let drawControl = new L.Control.Draw({
            draw: {
                polygon: false,
                polyline: true,
                marker: false,
                circle: false,  // Disable circle drawing
                rectangle: false,
                circlemarker: false
            },
            edit: {
                featureGroup: drawnItems
            }
        });
        map.addControl(drawControl);

        // start polyline drawing immediately (use handler, not control)
        new L.Draw.Polyline(map, drawControl.options.draw.polyline).enable();
    } 
    else if (choice === 1 || choice === 2 || choice === 3) {
        let drawControl = new L.Control.Draw({
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

        // start marker drawing immediately (use handler, not control)
        new L.Draw.Marker(map, drawControl.options.draw.marker).enable();
    }
    else {
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

        // choose which tool to start; here we start the marker by default
        new L.Draw.Marker(map, drawControl.options.draw.marker).enable();
    }

    let form = document.getElementById('DataformStep2');

// --- Capture location data ---
    function captureData() {

        // Convert all layers to one GeoJSON object 
        let geojson = drawnItems.toGeoJSON();

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

    function onEdited() {
        captureData();
    }

    function onDeleted() {
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