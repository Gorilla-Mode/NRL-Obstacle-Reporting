function GeolocateUser(zoom) 
{
    if (navigator.geolocation) 
    {
        navigator.geolocation.getCurrentPosition(
            function(position) 
            {
                let latitude = position.coords.latitude;
                let longitude = position.coords.longitude;

                map.setView([latitude, longitude], zoom);
            },
            function(error) {
                let mapContainer = document.getElementById('map');
                switch (error.code) {
                    case error.PERMISSION_DENIED:
                        mapContainer.innerHTML = 'Access to geolocation was denied.';
                        break;
                    case error.POSITION_UNAVAILABLE:
                        mapContainer.innerHTML = 'Location information is unavailable.';
                        break;
                    case error.TIMEOUT:
                        mapContainer.innerHTML = 'The request to get user location timed out.';
                        break;
                    default:
                        mapContainer.innerHTML = 'An error occurred while retrieving geolocation.';
                        break;
                }
            }
        );
    } else {
        document.getElementById('map').innerHTML = 'Geolocation is not supported by your browser.';
    }
}