public var CustomSkin:GUISkin;// GUISkin
public var Text:String = "";// Text
public var LifeTime:float = 1;// Life time
public var FadeEnd:boolean = false;// Fade out at last 1 second before destroyed
public var TextColor:Color = Color.white;// Text color
public var Position3D:boolean = false; // enabled when you need the text along with world 3d position
public var Position:Vector2;// 2D Position

private var alpha:float = 1;
private var timeTemp:float = 0;



function Start () {
	timeTemp = Time.time;
	GameObject.Destroy(this.gameObject,LifeTime);
	if(Position3D){
		var screenPos : Vector3 = Camera.main.WorldToScreenPoint(this.transform.position);
		Position = new Vector2(screenPos.x,Screen.height - screenPos.y);
	}
}

function Update () {

	if(FadeEnd){
		if(Time.time >= ((timeTemp + LifeTime) - 1)){
			alpha = 1.0f - (Time.time - ((timeTemp + LifeTime) - 1));
		}
	}else{
		alpha = 1.0f - ((1.0f / LifeTime) * (Time.time - timeTemp));
	}
	
	if(Position3D){
		var screenPos : Vector3 = Camera.main.WorldToScreenPoint(this.transform.position);
		Position = new Vector2(screenPos.x,Screen.height - screenPos.y);
	}
	
}


function OnGUI(){
	GUI.color.a = alpha;
	if(CustomSkin){
		GUI.skin = CustomSkin;
	}
	
	var textsize : Vector2 = GUI.skin.label.CalcSize(GUIContent(Text));
	var rect : Rect = Rect(Position.x - (textsize.x/2), Position.y,textsize.x,textsize.y);

	GUI.skin.label.normal.textColor = TextColor;
	GUI.Label(rect,Text);

}


