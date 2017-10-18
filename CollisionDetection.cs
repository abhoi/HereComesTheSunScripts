using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

	/*// Use this for initialization
	void Start () {
		
	}*/
	
	/*// Update is called once per frame
	void Update () {
		
	}*/

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Trigger entered!");
	}

	void OnMouseDown() {
		Debug.Log ("Mouse Down!");
	}

}
