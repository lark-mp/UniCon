var marker = null;
var googlemap = null;
var polyline = null;
var precisionCircle = null;

function initialize(){
	var initOptions = {
	    zoom: 18,
	    scaleControl: true,
	    center: new google.maps.LatLng(35.707909,139.760745),
	    mapTypeId: google.maps.MapTypeId.ROADMAP
	};
	googlemap = new google.maps.Map(document.getElementById("map_canvas"), initOptions);
}

function updatePosition(latitude, longitude, accuracy){
    var lat = parseFloat(latitude);
    var lng = parseFloat(longitude);
    var acc = parseFloat(accuracy);

    var latlng = new google.maps.LatLng(lat, lng);
    if (marker){
		if(googlemap.getCenter().equals(marker.getPosition())){
			googlemap.panTo(latlng);
		}
	}

	drawLine(latlng);
	moveMarker(latlng);
	drawPrecisionCircle(latlng, acc);
}

function moveMarker(currentPos){
	if (!marker){
		marker = new google.maps.Marker({
		position: currentPos,
		map: googlemap
		});
		googlemap.panTo(currentPos);
	} else {
	    marker.setPosition(currentPos);
	}
}

function drawLine(currentPos) {
	if(!polyline){
		var polyOptions = {
			strokeColor: '#ff00ff',
			strokeOpacity: 0.7,
			strokeWeight: 3
		};
		polyline = new google.maps.Polyline(polyOptions);
		polyline.setMap(googlemap);
	}

	var path = polyline.getPath();
	path.push(currentPos);
}

function drawPrecisionCircle(currentPos, accuracy){
	if(!precisionCircle){
		precisionCircle =
			new google.maps.Circle({
				map: googlemap,
				center: currentPos,
				radius: accuracy, // íPà ÇÕÉÅÅ[ÉgÉã
				strokeColor: '#0088ff',
				strokeOpacity: 0.8,
				strokeWeight: 1,
				fillColor: '#0088ff',
				fillOpacity: 0.2
			});
	} else {
		precisionCircle.setCenter(currentPos);
	}
}

function currentPos(){
    googlemap.panTo(marker.getPosition());
}

function clearLine(){
	if(polyline != null){
		polyline.setMap(null);
		polyline = null;
	}
}
