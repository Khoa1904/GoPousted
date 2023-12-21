var canvas = document.getElementById("signature");								//	싸인 canvas
var contCtx = canvas.getContext("2d");
var signaturePad = new SignaturePad(canvas);

window.ClearSign = () => {
	signaturePad.clear();
}

window.GetSign = () => {
	contCtx.drawImage(canvas, 0, 0);

	var isBlank = isCanvasBlank(canvas);
	if (isBlank) {
		return "NO";
	} else {
		var canvasData = canvas.toDataURL("image/png", 1.0);
		//document.getElementById("signImage").src = canvasData;
		return canvasData;
	}

	
}

function isCanvasBlank(canvas) {
	return !canvas.getContext('2d').getImageData(0, 0, canvas.width, canvas.height).data.some(channel => channel !== 0);
}