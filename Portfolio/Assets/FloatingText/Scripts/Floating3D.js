public var PositionMult:Vector3;
public var PositionDirection:Vector3;


private var positionTemp:Vector3;
private var floatingtext:FloatingText;


function Start () {
	positionTemp = this.transform.position;	
}

function Update () {
	positionTemp += PositionDirection * Time.deltaTime;
	PositionDirection += PositionMult * Time.deltaTime;
	this.transform.position = Vector3.Lerp(this.transform.position,positionTemp,0.5f);

}