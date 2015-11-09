var nextObjectId = 0;
var shipObject={};
var enemyShipObject={};
var currentRadarObject = 0;// 0- none, 1 - enemyShip

$(document).ready(function (){
	$('#sendCode').click(function (){
		$.ajax({
			url: "/api/code",
			contentType: "application/x-www-form-urlencoded; charset=UTF-8",
			data: "Code="+$("#codeEditorWindow textarea").val().replace(/\+/g, "%2B"),
			type: "POST",
			success: function (data) { },
			error: function () { terminate(); }
		});
		return false;
	});

	$('#codeEditorWindow textarea').on('keydown', function (e) {
	    var keyCode = e.keyCode || e.which;

	    if (keyCode === 9) {
			e.preventDefault();
			var start = this.selectionStart;
			var end = this.selectionEnd;
			var val = this.value;
			var selected = val.substring(start, end);
			var re = /^/gm;

			this.value = val.substring(0, start) + selected.replace(re, '    ') + val.substring(end);
			this.selectionStart = end + 4;
			this.selectionEnd = end + 4;
	    }
	});

	$('.shipLayout').mousemove(function(event) {
		var x = Math.floor((event.pageX - $(this).offset().left) / 35);
		var y = Math.floor((event.pageY - $(this).offset().top) / 35);
		var so = $(this).parent().is('#ship') ? shipObject : enemyShipObject;

		if(y < so.layout.length && x < so.layout[0].length && so.layout[y][x].type != 0)
		{
			if($(this).parent().is('#ship')) $('#toolTip').addClass('friendly');
			else $('#toolTip').removeClass('friendly');
			$('#toolTip').html("["+y+"]["+x+"]<br>"+
				so.layout[y][x].health+"hp<br>"+
				so.layout[y][x].energy+" GJ<br>"+
				so.layout[y][x].stepsToReady);
			$('#toolTip').css({
				"left":event.pageX+"px",
				"top":event.pageY+"px"
			});

			$('#toolTip').show();
		}
		else $('#toolTip').hide();
	}).mouseleave(function(){
		$('#toolTip').hide();
	});

	$('#message input').keypress(function (e){
		if (e.which == 13) {
			$('#loginButton').click();
			return false;
		}
	});

	$('#loginButton').click(function (){
		$.ajax({
			url: "/api/login",
			contentType: "application/x-www-form-urlencoded; charset=UTF-8",
			data: "Login="+$("#message #login").val()+
				"&Password="+$("#message #password").val(),
			type: "POST",
			success: function (data){
				if(data.success)
				{
					startGame();
				}
				else
				{
					$('#message p').text(data.error);
				}
			},
			error: function () { terminate("Server is not responding, try again later"); }
		});
		return false;
	});

	$('#register #registerButton').click(function (){
		$.ajax({
			url: "/api/login/register",
			contentType: "application/x-www-form-urlencoded; charset=UTF-8",
			data: "Login="+$("#register #login").val()+
				"&Password="+$("#register #password").val()+
				"&PasswordRepeat="+$("#register #passwordRepeat").val(),
			type: "POST",
			success: function (data){
				if(data.success)
				{
					$('#message').show();
					$('#register').hide();
					$('#message p').text("You can now log in");
				}
				else
				{
					$('#register p').text(data.error);
				}
			},
			error: function () { terminate("Server is not responding, try again later"); }
		});
		return false;
	});

	$('#message #registerButton').click(function (){
		$('#message').hide();
		$('#register').show();

		return false;
	});

	$('#register #cancelButton').click(function (){
		$('#message').show();
		$('#register').hide();

		return false;
	});

	$('#consoleWindow input').keypress(function (e){
		if (e.which == 13) {
			$('#consoleWindow a').click();
			return false;
		}
	});

	$('#consoleWindow a').click(function (){
		$.ajax({
			url: "/api/code/console",
			contentType: "application/x-www-form-urlencoded; charset=UTF-8",
			data: "Code="+$("#consoleWindow input").val().replace(/\+/g, "%2B"),
			type: "POST",
			success: function (data){
				$("#consoleWindow input").val("");
			},
			error: function () { terminate(); }
		});
		return false;
	});

	$('#repairButton').click(function (){
		$.ajax({
			url: "/api/ship/repair",
			type: "POST",
			error: function () { terminate(); }
		});
		return false;
	});
});

function setUpTaskBar()
{
	$('#buttonCodeEditor').click(function (){
		toggleWindow('#codeEditorWindow');
		return false;
	});
	$('#buttonRadar').click(function (){
		toggleWindow('#radarWindow');
		return false;
	});
	$('#buttonConsole').click(function (){
		toggleWindow('#consoleWindow');
		return false;
	});
}

function toggleWindow(windowId)
{
	if($(windowId).is(':visible'))
		$(windowId).hide();
	else
	{
		if($('.window:visible:not(.right)').length == 0) $(windowId).removeClass('right').show();
		else if($('.window.right:visible').length == 0) $(windowId).addClass('right').show();
		else {
			$('.window.right').hide();
			$(windowId).addClass('right').show();
		}
	}
}

function startGame()
{
	$('#ship, #codeEditorWindow, #radarWindow').show();
	$('#message').hide();
	$('#register').remove();
	loadShip(true);
	loadShip(false);

	$.ajax({
		url: "/api/code",
		type: "GET",
		success: function (data){
			$("#codeEditorWindow textarea").text(data);
		},
		error: function () { terminate(); }
	});

	updatesInterval = setInterval(getUpdates, 1000);

	setUpTaskBar();
}

function terminate(errorMessage)
{
	errorMessage = errorMessage || "Connection lost, please, refresh the page";
	$('#ship, .window, #deadMessage').remove();
	$('#message').show();
	$('#message').text(errorMessage);
	clearInterval(updatesInterval);
}

function getUpdates()
{
	$.ajax({
		url: "/api/updates",
		type: "GET",
		success: function (data){
			for (var i = 0; i < data.Events.length; i++) {
			    switch (data.Events[i].type)
				{
					case "shoot":
						coordsFrom = [
                            shipObject.weapons[data.Events[i].data[2]].position[0],
                            shipObject.weapons[data.Events[i].data[2]].position[1]
						];
						coordsTo = [
							Number(data.Events[i].data[0])*35+17 + shipObject.layoutOffset[0],
							Number(data.Events[i].data[1])*35+17 + shipObject.layoutOffset[1]
						];

						shootRocket(coordsFrom, coordsTo, false);
						break;
					case "shootIncoming":
						coordsFrom = [
                            enemyShipObject.weapons[data.Events[i].data[2]].position[0],
                            enemyShipObject.weapons[data.Events[i].data[2]].position[1]
						];
						coordsTo = [
							Number(data.Events[i].data[0])*35+17 + enemyShipObject.layoutOffset[0],
							Number(data.Events[i].data[1])*35+17 + enemyShipObject.layoutOffset[1]
						];

						shootRocket(coordsFrom, coordsTo, true);
						break;
					case "jump":
						$('#jumpAnimation').animate({opacity: "1"}, 200, "easeInCubic", function () {
							$('#jumpAnimation').animate({opacity: "0"}, 300, "easeInCubic");
						});
						break;
				}
			}

			for (var i = 0; i < data.ShipStatus.length; i++) {
			    for (var j = 0; j < data.ShipStatus[i].length; j++) {
			 		shipObject.layout[i][j].health = data.ShipStatus[i][j].health;
			        shipObject.layout[i][j].energy = data.ShipStatus[i][j].energy;
			        shipObject.layout[i][j].stepsToReady = data.ShipStatus[i][j].stepsToReady;
			    }
			}

			if(currentRadarObject != data.RadarType)
				updateRadar(data.RadarType);
			else
			{
				switch(data.RadarType)
				{
					case 1:
						for (var i = 0; i < data.RadarData.length; i++) {
						    for (var j = 0; j < data.RadarData[i].length; j++) {
						 		enemyShipObject.layout[i][j].health = data.RadarData[i][j].health;
						        enemyShipObject.layout[i][j].energy = data.RadarData[i][j].energy;
						        enemyShipObject.layout[i][j].stepsToReady = data.RadarData[i][j].stepsToReady;
						    }
						}
						break;
				}
			}

			for (var i = 0; i < data.Log.length; i++) {
				var classes = "";
				/*if (data.Log[i].slice(0,1) === "E")
				{
					classes=" error";
					data.Log[i] = data.Log[i].slice(1);
				}*/
				$('#consoleWindow div').append('<p class="'+classes+'"">'+data.Log[i]+"</p>");
			}

			if(data.IsDead)
			{
				$('#deadMessage').show();
			}
			else
			{
				$('#deadMessage').hide();
			}
		},
		error: function () { terminate(); }
	});
}

function updateRadar(radarType)
{
	switch(radarType)
	{
		case 0:
			$('#enemyShip').fadeOut();
			$('#radarWindow h4').text("No data");
			break;
		case 1:
			loadShip(false);
			break;
	}

	currentRadarObject = radarType;
}

function loadShip(yourShip)
{
	$.ajax({
		url: "/api/ship/" + (yourShip ? "" : "enemyShip"),
		type: "GET",
		success: function (data){
			if(data == null) return;

			var so = yourShip ? shipObject : enemyShipObject;
			so.elementId = yourShip ? "ship" : "enemyShip";

			if(yourShip && data.isDead)
			{
				
			}

		    so.background = "/content/img/shipBackground.png";
			so.layoutImage = "/content/img/shipLayout.png";

			$("#"+so.elementId+" .shipBackground").attr("src", so.background);
			$("#"+so.elementId+" .shipBackground").one("load", function() {
				$("#"+so.elementId).css({
					"margin-left": -$("#"+so.elementId+" .shipBackground").width()/2+"px"
					,"margin-top": -$("#"+so.elementId+" .shipBackground").height()/2+"px"
				});
			});

			$("#"+so.elementId+" .shipLayout").attr("src", so.layoutImage);
			$("#"+so.elementId+" .shipLayout, #"+so.elementId+" .roomSystemsContainer").css({
				"top": data.layoutOffset[0]+"px"
				,"left": data.layoutOffset[1]+"px"
			});

			so.layout = data.cells;
			so.layoutOffset = [Number(data.layoutOffset[0]), Number(data.layoutOffset[1])];
			so.weapons = data.weapons;

			updateRoomSystems(yourShip);
			updateWeapons(yourShip);

			$("#"+so.elementId).fadeIn();

			if(!yourShip) $('#radarWindow h4').text(data.owner);
		},
		error: function () { terminate(); }
	})
}

function updateRoomSystems(yourShip)
{
	var so = yourShip ? shipObject : enemyShipObject;

	$('#'+so.elementId+' .roomSystemsContainer').empty();

	for (var i = 0; i < so.layout.length; i++) {
		for (var j = 0; j < so.layout[i].length; j++) {
			if(so.layout[i][j].type >= 2)
			{
				var systemName = "";
				switch(so.layout[i][j].type)
				{
					case 2: systemName="Weapon"; break;
					case 3: systemName="Engine"; break;
					case 4: systemName="Repair"; break;
				}
				$('#'+so.elementId+' .roomSystemsContainer').append(
					'<img src="/content/img/system'+systemName+'.png" id="system'+so.elementId+i+'-'+j+'">');
				$('#system'+so.elementId+i+'-'+j).css({
					"left":j*35+"px",
					"top":i*35+"px"}).one("load", function() {
						$(this).css({
							"margin-top": (35-$(this).height())/2+"px",
							"margin-left": (35-$(this).width())/2+"px"
						});
					});
			}
		}
	}
}

function updateWeapons(yourShip)
{
	var so = yourShip ? shipObject : enemyShipObject;

	$('#'+so.elementId+' .weaponContainer').empty();

	for (var i = 0; i < so.weapons.length; i++) {
		var coords = [Number(so.weapons[i].position[0]), Number(so.weapons[i].position[1])];

		$('#'+so.elementId+' .weaponContainer').append(
			'<img src="/content/img/weapon'+so.weapons[i].weaponType+'.png" id="weapon'+so.elementId+i+'">');

		$('#weapon'+so.elementId+i).css({
			"left":coords[0]+"px",
			"top":coords[1]+"px"
		});


		if(so.weapons[i].reversed)
			$('#weapon'+so.elementId+i).one("load", function() {
				$(this).css({
					"transform": "scaleX(-1)",
					"margin-left": -$(this).width()+"px"
				});
			});
	}
}

function shootRocket(fromCoords, toCoords, incoming)
{
	var missileId = nextObjectId++;

	$((incoming ? '#enemyShip' : '#ship') + ' .outcomingContainer')
		.prepend('<img id="obj' + missileId + '" src="/content/img/missile.png">');

	$("#obj"+missileId).css({
		"position":"absolute"
		,"left":fromCoords[0]+"px"
		,"top":fromCoords[1]+"px"
	}).animate({
		"top":"-100px"
	}, 1000, "easeInSine", function(){
		$("#obj"+missileId).remove();
		$((incoming ? '#ship' : '#enemyShip') + ' .incomingContainer').prepend('<img id="obj'+missileId+'" src="/content/img/missile.png">');

		var angle = Math.random()*Math.PI*2;

		var distance = 200;

		incomingFromCoords = [
			toCoords[0]+Math.sin(angle)*distance,
			toCoords[1]+Math.cos(angle)*distance
		];

		$("#obj"+missileId).css({
			"position":"absolute"
			,"transform":"rotate("+(-angle)+"rad)"
			,"left":incomingFromCoords[0]+"px"
			,"top":incomingFromCoords[1]+"px"
			,"opacity":-0.5
		}).animate({
			"left":toCoords[0]+"px"
			,"top":toCoords[1]+"px"
			,"opacity":1.0
		}, 500, "easeInSine", function(){
			$("#obj"+missileId).remove();

			 spawnExplosion(toCoords[0], toCoords[1], !incoming);
		});
	});
}

function spawnExplosion(x, y, incoming)
{
	var missileId = nextObjectId++;

	$((incoming ? '#enemyShip' : '#ship') + ' .incomingContainer')
		.prepend('<div id="obj' + missileId + '" class="explosion"></div>');

	$("#obj"+missileId).css({
		"position":"absolute"
		,"left":x+"px"
		,"top":y+"px"
		,"width":"0px"
		,"height":"0px"
		,"border-radius":"25px"
		,"opacity":"0.6"
	}).animate({
			"left":x-50+"px"
			,"top":y-50+"px"
			,"width":"100px"
			,"height":"100px"
			,"border-radius":"50px"
			,"opacity":"0.1"
		}, 500, "easeInSine", function(){
			$("#obj"+missileId).remove();
		});
}