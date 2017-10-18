using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR.InteractionSystem;

public class sliderControl : MonoBehaviour {

	Planets planetScript;

	// Use this for initialization
	/*void Start () {
		GameObject sceneGen = GameObject.Find("Scene Generator");
		this.planetScript = sceneGen.GetComponent<Planets>();
	}*/

	// Update is called once per frame
	void LateUpdate () {
		GameObject sceneGen = GameObject.Find("Scene Generator");
		this.planetScript = sceneGen.GetComponent<Planets>();
	}

	//
	public void scaleSpeed(float val){
		//Speed up or slow down planets.
		/*foreach(var item in planetScript.createdPlanets){
			if(val < planetScript.getSpeedFactor()){
				item.GetComponent<rotate> ().rotateSpeed *= val;
				item.GetComponent<rotate> ().rotateSpeed /= planetScript.getSpeedFactor();
			}
			else if (val > planetScript.getSpeedFactor()) {
//				Debug.Log ("Lessen.");
				item.GetComponent<rotate> ().rotateSpeed /= planetScript.getSpeedFactor();
				item.GetComponent<rotate> ().rotateSpeed *= val;
			}
		}
		//Speed up slow down stars.
		foreach(var star in planetScript.createdStars){
			if(val < planetScript.getSpeedFactor()){
				try{
					star.GetComponent <rotate> ().rotateSpeed *= val;
					star.GetComponent <rotate> ().rotateSpeed /= planetScript.getSpeedFactor();
				} catch {
					Debug.Log ("Lessen");
				}
			}
			else if (val > planetScript.getSpeedFactor()) {
				star.GetComponent<rotate> ().rotateSpeed /= planetScript.getSpeedFactor();
				star.GetComponent<rotate> ().rotateSpeed *= val;
			}
		}
		planetScript.setSpeedFactor (val);*/
		foreach (var item in planetScript.current3dplanets) {
			if (item != null) {
				if (val < planetScript.getSpeedFactor()){
					item.GetComponent<rotate> ().rotateSpeed *= val;
					item.GetComponent<rotate> ().rotateSpeed /= planetScript.getSpeedFactor();
				} else if (val > planetScript.getSpeedFactor()) {
					//				Debug.Log ("Lessen.");
					item.GetComponent<rotate> ().rotateSpeed /= planetScript.getSpeedFactor();
					item.GetComponent<rotate> ().rotateSpeed *= val;
				}
			}
		}
		foreach(var star in planetScript.current3dstars){
			if (star != null) {
				if(val < planetScript.getSpeedFactor()){
					try{
						star.GetComponent <rotate> ().rotateSpeed *= val;
						star.GetComponent <rotate> ().rotateSpeed /= planetScript.getSpeedFactor();
					} catch {
						Debug.Log ("Lessen");
					}
				} else if (val > planetScript.getSpeedFactor()) {
					star.GetComponent<rotate> ().rotateSpeed /= planetScript.getSpeedFactor();
					star.GetComponent<rotate> ().rotateSpeed *= val;
				}
			}
		}
		planetScript.setSpeedFactor (val);
	}


	//Scale Size of planets.
	public void scaleSize (float val) {

		Vector3 newScale = new Vector3 (val,1,val);

		/*//Scale Orbits.
		foreach (var item in planetScript.createdOrbits){
			item.GetComponent<Circle> ().transform.localScale = newScale;
		}*/
		/*foreach (var item in planetScript.current3dplanets) {
			// item.GetComponent<Circle> ().transform.localScale = newScale;
			// item.transform.GetChild(3).transform.localScale = newScale;
			item.transform.Find("LR1");
		}*/

		/*//Scale Stars.
		foreach (var item in planetScript.createdStars){
			Debug.Log (planetScript.getScaleFactor()[0]*val);
			if(val < planetScript.getScaleFactor()[0]){
				Vector3 currentScale = item.transform.localScale;
				currentScale = currentScale * val;
				item.transform.localScale = currentScale;

				currentScale = item.transform.localScale;
				currentScale = currentScale / planetScript.getScaleFactor()[0];
				item.transform.localScale = currentScale;
			}
			else if (val > planetScript.getScaleFactor()[0]) {

				Vector3 currentScale = item.transform.localScale;
				currentScale = currentScale / planetScript.getScaleFactor()[0];
				item.transform.localScale = currentScale;

				currentScale = item.transform.localScale;
				currentScale = currentScale * val;
				item.transform.localScale = currentScale;
			}
		}*/

		foreach (var item in planetScript.current3dstars) {
			// Debug.Log (planetScript.getScaleFactor()[0]*val);
		if (item != null) {
			if(val < planetScript.getScaleFactor()[0]){
				Vector3 currentScale = item.transform.localScale;
				currentScale = currentScale * val;
				item.transform.localScale = currentScale;

				currentScale = item.transform.localScale;
				currentScale = currentScale / planetScript.getScaleFactor()[0];
				item.transform.localScale = currentScale;
			}
			else if (val > planetScript.getScaleFactor()[0]) {

				Vector3 currentScale = item.transform.localScale;
				currentScale = currentScale / planetScript.getScaleFactor()[0];
				item.transform.localScale = currentScale;

				currentScale = item.transform.localScale;
				currentScale = currentScale * val;
				item.transform.localScale = currentScale;
			}
		}
		}

		/*//Scale Planets.
		foreach (var item in planetScript.createdPlanets){
			Debug.Log (planetScript.getScaleFactor()[0]*val);
			if(val < planetScript.getScaleFactor()[0]){
				Vector3 currentScale = item.transform.localScale;
				currentScale = currentScale * val;
				item.transform.localScale = currentScale;

				currentScale = item.transform.localScale;
				currentScale = currentScale / planetScript.getScaleFactor()[0];
				item.transform.localScale = currentScale;
			}
			else if (val > planetScript.getScaleFactor()[0]) {
				Vector3 currentScale = item.transform.localScale;
				currentScale = currentScale / planetScript.getScaleFactor()[0];
				item.transform.localScale = currentScale;

				currentScale = item.transform.localScale;
				currentScale = currentScale * val;
				item.transform.localScale = currentScale;
			}
		}*/
		foreach (var item in planetScript.current3dplanets) {
			Debug.Log (planetScript.getScaleFactor()[0]*val);
			if (item != null) {
			if(val < planetScript.getScaleFactor()[0]){
				Vector3 currentScale = item.transform.localScale;
				currentScale = currentScale * val;
				item.transform.localScale = currentScale;

				currentScale = item.transform.localScale;
				currentScale = currentScale / planetScript.getScaleFactor()[0];
				item.transform.localScale = currentScale;
			}
			else if (val > planetScript.getScaleFactor()[0]) {
				Vector3 currentScale = item.transform.localScale;
				currentScale = currentScale / planetScript.getScaleFactor()[0];
				item.transform.localScale = currentScale;

				currentScale = item.transform.localScale;
				currentScale = currentScale * val;
				item.transform.localScale = currentScale;
			}
		}

		}
		planetScript.setScaleFactor (newScale);
	}
}