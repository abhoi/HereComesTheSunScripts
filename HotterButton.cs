using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotterButton : MonoBehaviour {

	public Planets p;

	public void TaskOnClick() {
		var gameObjects = GameObject.FindGameObjectsWithTag ("2DButtons");
		foreach (GameObject g in gameObjects)
			Destroy (g);
		int numberOfSystems = 376;
		int planetSystemCounter;
		var systemOffset = new Vector3 (0, 3, 0);
		var oneOffset = new Vector3 (0, -70, 0);
		systemOffset += oneOffset;
		GameObject allCenter = new GameObject();
		allCenter.name = "all systems";
		for (planetSystemCounter = 306; planetSystemCounter < numberOfSystems; planetSystemCounter++) {
			p.generate2dSystem (p.starData, planetSystemCounter, p.planetData, systemOffset, allCenter);
		}
		// Debug.Log (planetSystemCounter);
		allCenter.transform.localScale = new Vector3 (0.1F, 0.1F, 0.1F);
	}
}
