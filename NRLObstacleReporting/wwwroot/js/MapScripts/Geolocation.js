/**
 * Uses the browser geolocation api to get gps coordinates of the user to set the map veiew. Requires that 
 * geolocation is enabled by the client to work.
 * @param zoom Sets the zoom level on the map
 */
function GeolocateUser(zoom) 
{
    if (!navigator.geolocation)
    {
        alert("Geolocation: Not supported by your browser");
        
    }
    else
    {
        navigator.geolocation.getCurrentPosition(
            function(position)
            {
                let latitude = position.coords.latitude;
                let longitude = position.coords.longitude;

                map.setView([latitude, longitude], zoom);
            },
            function(error) 
            {
                let mapContainer = document.getElementById('map');
                switch (error.code) 
                {
                    case error.PERMISSION_DENIED:
                        alert("Geolocation: Permission denied");
                        break;
                    case error.POSITION_UNAVAILABLE:
                        alert("Geolocation: Position unavailable");
                        break;
                    case error.TIMEOUT:
                        alert("Geolocation: Timedout");
                        break;
                    default:
                        mapContainer.innerHTML = 'Geolocation: error occured';
                        break;
                }
            }
        );
    }
}