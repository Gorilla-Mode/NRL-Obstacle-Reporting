/**
 * Generates a leaflet layer and adds it to a map variable which is centered on a location
 * @returns a leaflet map variable
 * @
 */
function GenerateMap(zoom)
{
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
    }).setView([58.14671, 7.9956], zoom);

    L.control.layers(baseMaps).addTo(map);
    
    return map;
}

/**
 * Generates a leaflet layer and adds it to a map variable which is centered on a location. uses high accuracy geolocation
 * @param zoom the zoom level of the map
 * @returns a leaflet map variable
 */
function GenerateMapGeolocate(zoom)
{
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
    }).setView([58.14671, 7.9956], zoom);
    
    map.addControl(
        L.control.locate({
            locateOptions: {
                enableHighAccuracy: true
            }
        })
    );

    L.control.layers(baseMaps).addTo(map);
    
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
    }).setView([58.14671, 7.9956], zoom);

    L.control.layers(baseMaps).addTo(map);

    var inx = 1;
    let geojsonLayer = L.geoJSON(Geojson, 
{
            onEachFeature: function (feature, layer) 
            {
                if (feature.properties.name === "gpsCoordinates")
                {
                    layer.bindPopup(`${inx}. ${feature.properties.name}`);
                }
                else
                {
                    layer.bindPopup(`${inx}. ${feature.geometry.type}`); 
                }
                
                inx++;
            }
        });
    
    geojsonLayer.addTo(map);
    map.fitBounds(geojsonLayer.getBounds());
    
    return map;
}