/**
 * Extracts coordinates from geojson, the coordinates are reversed from lnglat to latlng to be easy to use with leaflet
 * @see {@link https://datatracker.ietf.org/doc/html/rfc7946#section-3.1.1} for geoJson coordinate definition
 * @see {@link https://leafletjs.com/reference.html#latlng} for Leaflet coordinate definition
 * @param GeoJson Input geojson duh
 * @returns {[Array]} Array of the coordinates of the first feature
 */
function GeoJsonExtractCoordinates (GeoJson)
{
    //array is reversed due to leaflet latlng instead of lnglat used by geojson
    //GeoJson source: https://datatracker.ietf.org/doc/html/rfc7946#section-3.1.1
    //Leaflet source: https://leafletjs.com/reference.html#latlng
    return GeoJson.features[0].geometry.coordinates.reverse()
}