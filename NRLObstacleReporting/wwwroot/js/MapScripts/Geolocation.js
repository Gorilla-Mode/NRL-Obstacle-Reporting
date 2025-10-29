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
    } else {
        alert("Geolocation deninedGeolocation: Not supported by your browser");
    }
}