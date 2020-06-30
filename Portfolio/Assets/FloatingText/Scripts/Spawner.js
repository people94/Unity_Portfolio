var TextFloat:GameObject[];
var index:int = 0;
function Start () {

}
function OnGUI(){
	GUI.Label(Rect(10,10,300,30),"Floating Text : "+TextFloat[index].name);
	if(GUI.Button(Rect(10,50,180,30),"Position 2D Text")){
		index = 0;
	}
	if(GUI.Button(Rect(10,85,180,30),"Position 3D Text")){
		index = 1;
	}
	if(GUI.Button(Rect(10,120,180,30),"3D Object attached")){
		index = 2;
	}
}
function Update () {
	
	if(Input.GetMouseButtonDown(0)){
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
    	var hit : RaycastHit;
    	if (Physics.Raycast (ray, hit, 100)) {
    		if(TextFloat[index]){
        		var floattext : GameObject = GameObject.Instantiate(TextFloat[index],hit.point + (Vector3.up),Quaternion.identity);
        		floattext.GetComponent(FloatingText).Text = "+"+Random.Range(5,100);
        	}
   	 	}
	}
	
}