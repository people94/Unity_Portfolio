#pragma strict
var arrowSpawn : Transform;
var projectile : Transform;
var reload = false;
var timer : float;
function Start () {
	animation.Play("Draw");
	animation["Draw"].speed = 0.25;
	animation["Draw"].wrapMode = WrapMode.Once;
}

function Update () {
	if(animation["Draw"].time > 0.5) {
		animation.Play("Shoot");
		animation["Shoot"].speed = 2;
		animation["Shoot"].wrapMode = WrapMode.Once;
	}
	if(animation["Shoot"].time > 0.1) {
		arrowSpawn.GetComponent(MeshRenderer).enabled = false;
		if(!GameObject.Find("Arrow(Clone)")) {
			var arrow = Instantiate(projectile, arrowSpawn.transform.position, transform.rotation);
			arrow.transform.rigidbody.AddForce(transform.forward * 2000);
			Destroy(arrow.gameObject, 0.5);
		}
		reload = true;
	}
	if(reload == true) {
		timer += 1 * Time.deltaTime;
		if(timer >= 2) {
			reload = false;
			timer = 0;
			animation.Play("Draw");
			animation["Draw"].speed = 0.25;
			animation["Draw"].wrapMode = WrapMode.Once;
			arrowSpawn.GetComponent(MeshRenderer).enabled = true;
		}
	}
}