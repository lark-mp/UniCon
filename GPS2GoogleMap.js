/* global GEvent */

var marker = null;
var googlemap = null;
var polyline = null;
var precisionCircle = null;
var testCircle = null;
var attitudeSymbol = null;
var attitudeMarker = null;
var waypointPolyline = null;
var waypointCircles = [];
var waypointLengthTest;

var tmp =0;

function initialize(){
	var initOptions = {
	    zoom: 18,
	    scaleControl: true,
	    center: new google.maps.LatLng(35.707909,139.760745),
	    mapTypeId: google.maps.MapTypeId.ROADMAP,
		disableDoubleClickZoom:true
	};
	var styles = [
		{
			featureType:"poi",
			stylers:[
				{visibility:"off"}
			]
		}
	];
	
	
	googlemap = new google.maps.Map(document.getElementById("map_canvas"), initOptions);
	googlemap.setOptions({styles: styles});
	
	google.maps.event.addListener(googlemap,'click',clickAction);
        
        
        var waypointLengthDiv = document.createElement('div');
        var waypointLength = new initWaypointLengthDiv(waypointLengthDiv);

        waypointLengthDiv.index = 1;
        googlemap.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(waypointLengthDiv);
}

function initWaypointLengthDiv(controlDiv){
    controlDiv.style.padding = '5px';

    // Set CSS for the control border.
    var controlUI = document.createElement('div');
    controlUI.style.backgroundColor = 'white';
    controlUI.style.opacity = 0.6;
    controlUI.style.borderStyle = 'none';
    controlUI.style.borderWidth = '1px';
    controlUI.style.cursor = 'pointer';
    controlUI.style.textAlign = 'left';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior.
    waypointLengthTest = document.createElement('div');
    waypointLengthTest.style.fontFamily = 'Arial,sans-serif';
    waypointLengthTest.style.fontSize = '12px';
    waypointLengthTest.style.paddingLeft = '4px';
    waypointLengthTest.style.paddingRight = '4px';
    waypointLengthTest.innerHTML = '<strong>0 [m]</strong>';
    controlUI.appendChild(waypointLengthTest);
}

function updatePosition(latitude, longitude, accuracy,degHeading,gpsValid){
    var lat = parseFloat(latitude);
    var lng = parseFloat(longitude);
    var acc = parseFloat(accuracy);
    var hed = parseFloat(degHeading);

    var latlng = new google.maps.LatLng(lat, lng);
//    if (marker){
//        if(googlemap.getCenter().equals(marker.getPosition())){
//                googlemap.panTo(latlng);
//        }
//    }
    
    drawLine(latlng);
    moveMarker(latlng,hed,gpsValid);
    drawPrecisionCircle(latlng, acc);
    
}

//function moveMarker(currentPos){
//	if (!marker){
//		marker = new google.maps.Marker({
//		position: currentPos,
//		map: googlemap
//		});
//		googlemap.panTo(currentPos);
//	} else {
//	    marker.setPosition(currentPos);
//	}
//}

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
    if(marker){
        googlemap.panTo(marker.getPosition());
    }
}

function clearLine(){
	if(polyline != null){
		polyline.setMap(null);
		polyline = null;
	}
}

function moveMarker(currentPos,degAttitude,gpsValid){
//	if(!attitudeMarker){	
	if(!attitudeMarker || !attitudeSymbol){
		attitudeSymbol = {
			path:google.maps.SymbolPath.FORWARD_CLOSED_ARROW,
			scale:5,
			rotation:degAttitude,
			fillColor:"#FF0000",
			fillOpacity:1,
			strokeColor:"#FF0000",
			strokeWeight:0,
		}
		
		attitudeMarker = new google.maps.Marker({
			map:googlemap,
			position:currentPos,
			icon:attitudeSymbol,
			clickable:false,
			crossOnDrag:false
		});
	}else{
		attitudeSymbol.rotation = degAttitude;
		attitudeMarker.setIcon(attitudeSymbol);
		attitudeMarker.setPosition(currentPos);
	}
        if(gpsValid == 1){
            attitudeSymbol.fillColor = "#FF0000"
            attitudeSymbol.strokeColor = "#FF0000"
        }else{
            attitudeSymbol.fillColor = "#8080FF"
            attitudeSymbol.strokeColor = "#8080FF"
            
        }
}

function clickAction(event){
	var clickPos = new google.maps.LatLng(event.latLng.lat(),event.latLng.lng());
	
	addWaypoint(clickPos);
}

function addWaypoint(latLng){
        var circleSymbol = {
                path:google.maps.SymbolPath.CIRCLE,
                scale:10,
                fillOpacity:0,
                strokeColor:"#FF0000",
                strokeWeight:1,
        }
        
	var newCircle = new google.maps.Marker({
			map: googlemap,
                        position: latLng,
                        path:google.maps.SymbolPath.CIRCLE,
                        icon:circleSymbol,
			draggable:true
			});
	waypointCircles.push(newCircle);
        
	google.maps.event.addListener(newCircle,'dragend',waypointCircleDragged);
	google.maps.event.addListener(newCircle,'dblclick',function(){
		for(var i=0;i<waypointCircles.length;i++){
			if(newCircle === waypointCircles[i]){
				deleteWaypoint(i);
			}
		}
	});
        
        waypointPolylineReconstruct();
}

function deleteWaypoint(i){
	waypointCircles[i].setMap(null);
	waypointCircles.splice(i,1);
	waypointPolylineReconstruct();
}

function waypointCircleDragged(event){
        waypointPolylineReconstruct();
}

function waypointPolylineReconstruct(){
        if(waypointPolyline != null){
            waypointPolyline.setMap(null);
            waypointPolyline = null;
        }
	var points = [];
		waypointPolyline = new google.maps.Polyline({
				map:googlemap,
				path:points,
				strokeColor:"#FF0000",
				strokeOpacity:1.0,
				strokeWeight:2
		});
	for(var i=0;i<waypointCircles.length;i++){
		waypointPolyline.getPath().push(waypointCircles[i].getPosition());
	}
        
        waypointLengthTest.innerHTML = '<strong>' + getWaypointLength().toFixed(2) +' [m]</strong>';
}

function getWaypointLength(){
        if(waypointCircles == null){
            return 0;
        }
        
	var totalDistance =0;
	for(var i=0;i<waypointCircles.length-1;i++){
		totalDistance += google.maps.geometry.spherical.computeDistanceBetween(waypointCircles[i].getPosition(), waypointCircles[i+1].getPosition());
	}
	return totalDistance;
}

function getWaypointString(){
        if(waypointCircles == null){
            return "";
        }
        
        var waypointString = "";
        for(var i=0;i<waypointCircles.length;i++){
            waypointString += waypointCircles[i].getPosition().toString();
        }
        return waypointString;
}