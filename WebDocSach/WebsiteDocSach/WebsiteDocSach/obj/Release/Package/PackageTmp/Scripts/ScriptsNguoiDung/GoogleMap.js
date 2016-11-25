﻿
function initMap() {
    var latlng = new google.maps.LatLng(10.764840, 106.687165); //Tọa độ cửa hàng
    var map = new google.maps.Map(document.getElementById('map'), {
        center: latlng,
        zoom: 16 //Độ phóng to của bản đồ
    });
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        title: "Vị trí của bạn" //Tên hiển thị khi đưa chuột vào địa điểm
    });
}

function timDuongDi() {
    var latlng = new google.maps.LatLng(10.764840, 106.687165); //Vị trí của cửa hàng
    var map = new google.maps.Map(document.getElementById('map'), {
        center: latlng,
        zoom: 16
    });
    var infoWindow = new google.maps.InfoWindow({ map: map });

    // Try HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            infoWindow.setPosition(pos);
            infoWindow.setContent('Vị trí của bạn');
            map.setCenter(pos);

            var directionsDisplay = new google.maps.DirectionsRenderer({
                map: map
            });
            var request = {
                destination: latlng,
                origin: pos,
                travelMode: google.maps.TravelMode.DRIVING
            };
            var directionsService = new google.maps.DirectionsService();
            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    // Display the route on the map.
                    directionsDisplay.setDirections(response);
                }
            });
        }, function () {
            handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, infoWindow, map.getCenter());
    }
}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
                          'Error: The Geolocation service failed.' :
                          'Error: Your browser doesn\'t support geolocation.');
} 
