/* global GEvent */

var AFFIRMED_ACTIVE_COLOR = "#0000FF";

var marker = null;
var googlemap = null;
var polyline = null;
var nextWaypointLine = null;
var precisionCircle = null;
var testCircle = null;
var attitudeSymbol = null;
var attitudeMarker = null;
var waypointPolyline = null;
var waypointCircles = [];
var waypointLengthTest;
var affirmedWaypointPolyline = null;
var affirmedWaypointCircles = [];

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

function updatePosition(latitude, longitude, accuracy,degHeading,gpsValid,nextWaypointId){
    var lat = parseFloat(latitude);
    var lng = parseFloat(longitude);
    var acc = parseFloat(accuracy);
    var hed = parseFloat(degHeading);

    var latlng = new google.maps.LatLng(lat, lng);
    if (attitudeMarker){
        if(googlemap.getCenter().equals(attitudeMarker.getPosition())){
                googlemap.panTo(latlng);
        }
    }
    
    drawLine(latlng);
    moveMarker(latlng,hed,gpsValid);
    drawPrecisionCircle(latlng, acc);
    
    setNextAffirmedWaypoint(parseInt(nextWaypointId));
    drawNextWaypointLine(latlng,parseInt(nextWaypointId));
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

function drawNextWaypointLine(latLng,nextWaypointId){
        if(!affirmedWaypointCircles || nextWaypointId > affirmedWaypointCircles.length || !affirmedWaypointCircles[nextWaypointId]){
            if(nextWaypointLine){
                nextWaypointLine.setMap(null);
                nextWaypointLine=null;
            }
            
            return;
        }
        
        
        if(!nextWaypointLine){
            var polyOptions = {
                strokeColor: "#808080",
                strokeOpacity:0.5,
                strokeWeight:1
            };
            nextWaypointLine = new google.maps.Polyline(polyOptions);
            nextWaypointLine.setMap(googlemap);
            
            
        }
        var path = nextWaypointLine.getPath();
        while(path.getLength() !== 0){
            path.removeAt(0);
        }
        
        
        path.push(latLng);
        path.push(affirmedWaypointCircles[nextWaypointId].getPosition());
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
    if(attitudeMarker){
        googlemap.panTo(attitudeMarker.getPosition());
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

function deleteAllWaypoints(){
        while(waypointCircles.length !== 0){
            deleteWaypoint(0);
        }
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
				strokeWeight:2,
                                draggable:false,
                                clickable:false
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

function addAffirmedWaypoint(lat,lng){
        var latLng = new google.maps.LatLng(lat,lng);
        var circleSymbol = {
                path:google.maps.SymbolPath.CIRCLE,
                scale:10,
                fillOpacity:0,
                strokeColor:AFFIRMED_ACTIVE_COLOR,
                strokeWeight:1,
        }
        
	var newCircle = new google.maps.Marker({
			map: googlemap,
                        position: latLng,
                        path:google.maps.SymbolPath.CIRCLE,
                        icon:circleSymbol,
			draggable:false,
                        clickable:false
			});
	affirmedWaypointCircles.push(newCircle);
        affirmedWaypointPolylineReconstruct();
}

function affirmedWaypointPolylineReconstruct(){
        if(affirmedWaypointPolyline != null){
            affirmedWaypointPolyline.setMap(null);
            affirmedWaypointPolyline = null;
        }
	var points = [];
		affirmedWaypointPolyline = new google.maps.Polyline({
				map:googlemap,
				path:points,
				strokeColor:AFFIRMED_ACTIVE_COLOR,
				strokeOpacity:0.3,
				strokeWeight:2,
                                draggable:false,
                                clickable:false
		});
	for(var i=0;i<affirmedWaypointCircles.length;i++){
		affirmedWaypointPolyline.getPath().push(affirmedWaypointCircles[i].getPosition());
	}
}

function deleteAllAffirmedWaypoints(){
    while(affirmedWaypointCircles.length !== 0){
        affirmedWaypointCircles[0].setMap(null);
        affirmedWaypointCircles.splice(0,1);
    }
    affirmedWaypointPolylineReconstruct();
}

function setNextAffirmedWaypoint(waypointId){
    if(!affirmedWaypointCircles){
        return;
    }
    for(var i=0;i<affirmedWaypointCircles.length;i++){
        if(i == waypointId){
            affirmedWaypointCircles[i].setOpacity(1.0);
        }else{
            affirmedWaypointCircles[i].setOpacity(0.4);
        }
    }
    
}

