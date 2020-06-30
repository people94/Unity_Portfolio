public var PositionMult:Vector2;
public var PositionDirection:Vector2;

private var position:Vector2;
private var positionTemp:Vector2;
private var floatingtext:FloatingText;


function Start () {
	var screenPos : Vector3 = Camera.main.WorldToScreenPoint(this.transform.position);
	
	positionTemp = new Vector2(screenPos.x,Screen.height - screenPos.y);
	position = positionTemp;
	
	if(this.gameObject.GetComponent(FloatingText)){
		floatingtext = this.gameObject.GetComponent(FloatingText);
		floatingtext.Position = position;
	}
}

function Update () {
	positionTemp += PositionDirection * Time.deltaTime;
	PositionDirection += PositionMult * Time.deltaTime;
	position = Vector3.Lerp(position,positionTemp,0.5f);
	
	if(floatingtext){
		floatingtext.Position = position;
	}
}