#pragma strict

var zoom : int = 20;
var normal : int = 60;
var smooth : float = 5;

var imageBino : GUITexture;
private var isZoomed = false;

function Start () 
{
imageBino.enabled = false;
}

function Update () 
{
	if(Input.GetMouseButtonDown(1))
	{
		isZoomed = !isZoomed;
	}
	
	if(isZoomed == true)
	{
		GetComponent.<Camera>().fieldOfView = Mathf.Lerp(GetComponent.<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
		imageBino.enabled = true;
	}
	
	else
	{
		GetComponent.<Camera>().fieldOfView = Mathf.Lerp(GetComponent.<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
		imageBino.enabled = false;
	}
}
