/// Sample Code for CS 491 Virtual And Augmented Reality Course - Fall 2017
/// written by Andy Johnson, Debojit Kaushik, Amlaan Bhoi.
/// Uses data from https://github.com/OpenExoplanetCatalogue/open_exoplanet_catalogue. (Open Exoplanet Archive)
/// makes use of various textures from the celestia motherlode - http://www.celestiamotherlode.net/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

using System;
using System.IO;
using UnityEngine.UI;

public class Planets : MonoBehaviour {

	public class System {
		public string name { get; set; }
		public Star star { get; set; }
		public List<Planet> planets { get; set; }
		public Binary binary { get; set; }
		public string declination { get; set; }
		public float distance { get; set; }
		public string rightascension { get; set; }
	}

	public class Star {
		public string name { get; set; }
		public List<Planet> planets { get; set; }
		public float mass { get; set; }
		public float radius { get; set; }
		public float temperature { get; set; }
		public float age { get; set; }
		public float metallicity { get; set; }
		public string spectralType { get; set; }
		public List<float> magnitude { get; set; }
		public float luminosity { get; set; }
	}

	public class Planet {
		public string name { get; set; }
		public float semiMajorAxis { get; set; }
		public float separation { get; set; }
		public float eccentricity { get; set; }
		public float periastron { get; set; }
		public float longitude { get; set; }
		public float meananomaly { get; set; }
		public float ascendingNode { get; set; }
		public float inclination { get; set; }
		public float impactParameter { get; set; }
		public float period { get; set; }
		public float transitTime { get; set; }
		public float periastronTime { get; set; }
		public float maximumRVTime { get; set; }
		public float mass { get; set; }
		public float radius { get; set; }
		public float temperature { get; set; }
		public float age { get; set; }
		public string spectralType { get; set; }
		public List<float> magnitude { get; set; }
		public string discoveryMethod { get; set; }
		public bool isTransiting { get; set; }
		public string description { get; set; }
		public int discoveryYear { get; set; }
		public string lastUpdate { get; set; }
		public float spinOrbitalAlignment { get; set; }
		public float speed { get; set; }
		public float distanceFromStar { get; set; }
	}

	public class Binary {
		public string name { get; set; }
		public List<Planet> planets { get; set; }
		public List<Star> stars { get; set; }
		// public Binary binary { get; set; }
		public float semiMajorAxis { get; set; }
		public float separation { get; set; }
		public float positionAngle { get; set; }
		public float eccentricity { get; set; }
		public float periastron { get; set; }
		public float longitude { get; set; }
		public float meananomaly { get; set; }
		public float ascendingNode { get; set; }
		public float inclination { get; set; }
		public float period { get; set; }
		public float transitTime { get; set; }
		public float periastronTime { get; set; }
		public float maximumRVTime { get; set; }
		public List<float> magnitude { get; set; }
	}

	float panelHeight = 0.1F;
	float panelWidth = 30.0F;
	float panelDepth = 0.1F;

	float orbitWidth = 0.01F;
	float habWidth = 0.03F;

	float revolutionSpeed = 0.2F;

	float panelXScale = 2.0F;
	float orbitXScale = 2.0F;

	// custom
	public ButtonListControl buttonControl;
	// public int numberOf3Dplanets = 0;
	public List<string[]> starData = new List<string[]> ();
	public List<List<string[]>> planetData = new List<List<string[]>> ();
	public List<GameObject> current3dstars = new List<GameObject> ();
	// public List<List<GameObject>> current3dplanets = new List<List<GameObject>> ();
	public List<GameObject> current3dplanets = new List<GameObject>();
	// GameObject tempSystem = new GameObject();
	public int numberOf3Dsystems = 0;

	public GameObject threeDPosOne = new GameObject ();
	public GameObject threeDPosTwo = new GameObject ();
	public GameObject threeDPosThree = new GameObject ();
	public GameObject threeDPosFour = new GameObject ();

	public List<System> farthestSystems = new List<System> ();
	public List<System> nearestSystems = new List<System> ();
	// public List<string[]> nearestStarData = new List<string[]> ();
	// public List<List<string[]>> nearestPlanetData = new List<List<string[]>> ();
	public List<System> coolerSystems = new List<System> ();
	public List<System> hotterSystem = new List<System> ();

	// custom2
	// Speed and scale factor(multiplier)
	private float speedFactor = 1;
	private Vector3 scaleFactor = new Vector3 (1, 1, 1);

	//Global list of created planets their stars and orbits as GameObjects.	
	public List <GameObject> createdPlanets = new List<GameObject> ();	
	public List <GameObject> createdStars = new List<GameObject> ();	
	public List <GameObject> createdOrbits = new List<GameObject> ();	
	public List <string[]> earthSizedPlanets = new List<string[]> ();	

	//Get Set methods for speed and scale multipliers.
	public void setSpeedFactor (float factor){		
		this.speedFactor = factor;		
	}		
	public float getSpeedFactor (){		
		return this.speedFactor;		
	}		
	public void setScaleFactor (Vector3 factor){		
		this.scaleFactor = factor;		
	}		
	public Vector3 getScaleFactor (){		
		return this.scaleFactor;		
	}		

	//------------------------------------------------------------------------------------//

	GameObject drawOrbit(string orbitName, float orbitRadius, Color orbitColor, float myWidth, GameObject myOrbits){

		GameObject newOrbit;
		GameObject orbits;

	
		newOrbit = new GameObject (orbitName);
		newOrbit.AddComponent<Circle> ();
		newOrbit.AddComponent<LineRenderer> ();

		newOrbit.GetComponent<Circle> ().xradius = orbitRadius;
		newOrbit.GetComponent<Circle> ().yradius = orbitRadius;

		var line = newOrbit.GetComponent<LineRenderer> ();
		line.startWidth = myWidth;
		line.endWidth = myWidth;
		line.useWorldSpace = false;

		newOrbit.GetComponent<LineRenderer> ().material.color = orbitColor;

		orbits = myOrbits;
		newOrbit.transform.parent = orbits.transform;

		this.createdOrbits.Add (newOrbit);

		return newOrbit;
		}

	//------------------------------------------------------------------------------------//

	public void dealWithPlanets (List<string []> planets, GameObject thesePlanets, GameObject theseOrbits){
		GameObject newPlanetCenter;
		GameObject newPlanet;

		GameObject sunRelated;

		Material planetMaterial;

		// GameObject to hold temp
		// GameObject temp = new GameObject();

		int planetCounter;
		// List<GameObject> tempPlanetList = new List<GameObject> ();
		for (planetCounter = 0; planetCounter < planets.Count; planetCounter++) {

			float planetDistance = float.Parse (planets [planetCounter][0]) / 149600000.0F * 10.0F;
			float planetSize = float.Parse (planets [planetCounter][1]) * 2.0F / 10000.0F;

			float planetSpeed = -1.0F / float.Parse (planets [planetCounter][2]) * revolutionSpeed;

			string textureName = planets [planetCounter][3];
			string planetName = planets [planetCounter][4];


			newPlanetCenter = new GameObject (planetName + "Center");
			newPlanetCenter.AddComponent<rotate> ();

			newPlanet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			newPlanet.name = planetName;
			newPlanet.transform.position = new Vector3 (0, 0, planetDistance * orbitXScale);
			newPlanet.transform.localScale = new Vector3 (planetSize, planetSize, planetSize);
			newPlanet.transform.parent = newPlanetCenter.transform;

			newPlanetCenter.GetComponent<rotate> ().rotateSpeed = this.speedFactor * planetSpeed; 

			planetMaterial = new Material (Shader.Find ("Standard"));
			newPlanet.GetComponent<MeshRenderer> ().material = planetMaterial;
			planetMaterial.mainTexture = Resources.Load (textureName) as Texture;

			try {
				createdPlanets.Add(newPlanetCenter);
			} catch {
				Debug.Log ("While while adding planets to list of Created Planets.");
			}

			GameObject LR1 = drawOrbit (planetName + " orbit", planetDistance * orbitXScale, Color.yellow, orbitWidth, theseOrbits);

			GameObject temp = new GameObject ();
			temp.AddComponent<rotate> ();
			temp.GetComponent<rotate> ().rotateSpeed = planetSpeed;
			newPlanetCenter.transform.parent = temp.transform;
			newPlanet.transform.parent = temp.transform;
			LR1.transform.parent = temp.transform;
			// newPlanetCenter.GetComponent<rotate> ().rotateSpeed = planetSpeed;
			// newPlanetCenter.GetComponent<rotate> ().transform.parent = temp.transform;
			// Debug.Log ("Planet speed: " + newPlanetCenter.GetComponent<rotate>() + " " + planetSpeed);

			// temp.transform.parent = thesePlanets.transform;
			// temp.transform.localPosition = Vector3.zero;

			if (numberOf3Dsystems % 2 == 0)
				temp.transform.position = new Vector3 (0, 0, 0);
			else if (numberOf3Dsystems % 3 == 0)
				temp.transform.position = new Vector3 (0, -40, 0);

			current3dplanets.Add (temp);

			sunRelated = thesePlanets;
			newPlanetCenter.transform.parent = sunRelated.transform;
		}
	}

	//------------------------------------------------------------------------------------//

	void sideDealWithPlanets (List<String[]> planets, GameObject thisSide, GameObject theseOrbits){
		GameObject newPlanet;

		GameObject sunRelated;
	
		Material planetMaterial;

		// custom
		GameObject sidePlanetDiscoveryText;
		GameObject sidePlanetNameText;
		GameObject outOfScopeindication;
		Material indicationMaterial;

		int planetCounter;

		GameObject newObj = new GameObject ();

		for (planetCounter = 0; planetCounter < planets.Count; planetCounter++) {

			float planetDistance = float.Parse (planets [planetCounter][0])/ 149600000.0F * 10.0F;
			float planetSize = float.Parse (planets [planetCounter][1]) * 1.0F / 10000.0F;
			string textureName = planets [planetCounter][3];
			string planetName = planets [planetCounter][4];
		
			// limit the planets to the width of the side view
			if ((panelXScale * planetDistance) < panelWidth) {

				newPlanet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				newPlanet.name = planetName;
				newPlanet.transform.position = new Vector3 (-0.5F * panelWidth + planetDistance * panelXScale, 0, 0);

				// custom
				// Debug.Log ("Planet name: " + newPlanet.name + " x: " + newPlanet.transform.position.x);

				if (planetSize <= 4.79)
					newPlanet.transform.localScale = new Vector3 (planetSize, planetSize, 5.0F * panelDepth);
				else
					newPlanet.transform.localScale = new Vector3 (4.79f, 4.79f, 5.0F * panelDepth);

				planetMaterial = new Material (Shader.Find ("Standard"));
				newPlanet.GetComponent<MeshRenderer> ().material = planetMaterial;
				planetMaterial.mainTexture = Resources.Load (textureName) as Texture;

				// custom

				sidePlanetDiscoveryText = new GameObject ();
				sidePlanetDiscoveryText.name = "Side Planet Discovery ";
				sidePlanetDiscoveryText.transform.position = new Vector3 (-0.5F * panelWidth + planetDistance * panelXScale, 0.3f, 0);
				sidePlanetDiscoveryText.transform.localScale = new Vector3 (0.1F, 0.1F, 0.1F);
				var sidePlanetMesh = sidePlanetDiscoveryText.AddComponent<TextMesh> ();
				if (planets [planetCounter] [5] != null)
					sidePlanetMesh.text = "Discovery method: " + planets [planetCounter] [5];
				else
					sidePlanetMesh.text = "Discovery method: Unknown";
				sidePlanetMesh.fontSize = 50;
				sidePlanetDiscoveryText.transform.parent = newPlanet.transform;


				sidePlanetNameText = new GameObject ();
				sidePlanetNameText.name = "Side Planet Name";
				sidePlanetNameText.transform.position = new Vector3 (-0.5F * panelWidth + planetDistance * panelXScale, 0.8f, 0);

				sidePlanetNameText.transform.localScale = new Vector3 (0.1F, 0.1F, 0.1F);
				var sidePlanetMesh2 = sidePlanetNameText.AddComponent<TextMesh> ();
				if (planets [planetCounter] [4] != null)
					sidePlanetMesh2.text = "Planet Name: " + planets [planetCounter] [4];
				else
					sidePlanetMesh2.text = "Planet Name: Unknown";

				sidePlanetMesh2.fontSize = 50;
				sidePlanetNameText.transform.parent = newPlanet.transform;

				sunRelated = thisSide;
				newPlanet.transform.parent = sunRelated.transform;
				newPlanet.transform.parent = newObj.transform;
			} else {
				outOfScopeindication = GameObject.CreatePrimitive (PrimitiveType.Cube);
				outOfScopeindication.name = "Out of Scope Planet Indication";
				outOfScopeindication.transform.position = new Vector3 (panelWidth - 10.0f, 0, 0);
				outOfScopeindication.transform.localScale = new Vector3 (0.5F, 5F, 0.5F);

				indicationMaterial = new Material (Shader.Find ("Standard"));
				outOfScopeindication.GetComponent<MeshRenderer> ().material = indicationMaterial;
				indicationMaterial.mainTexture = Resources.Load (textureName) as Texture;
				outOfScopeindication.transform.parent = newObj.transform;
			}
		}
		newObj.transform.parent = thisSide.transform;
		newObj.transform.localPosition = Vector3.zero;
	}

	//------------------------------------------------------------------------------------//

	void sideDealWithStar (string [] star, GameObject thisSide, GameObject theseOrbits){
		GameObject newSidePanel;
		GameObject newSideSun;
		GameObject sideSunText;

		// custom
		GameObject sideSunDistanceText;
		GameObject sideSunTypeText;

		GameObject habZone;

		Material sideSunMaterial, habMaterial;

		newSidePanel = GameObject.CreatePrimitive (PrimitiveType.Cube);
		newSidePanel.name = "Side " + star[1] + " Panel";
		newSidePanel.transform.position = new Vector3 (0, 0, 0);
		newSidePanel.transform.localScale = new Vector3 (panelWidth, panelHeight, panelDepth);
		newSidePanel.transform.parent = thisSide.transform;

		newSideSun = GameObject.CreatePrimitive (PrimitiveType.Cube);
		newSideSun.name = "Side " + star[1] + " Star";
		newSideSun.transform.position = new Vector3 (-0.5F * panelWidth - 0.5F, 0, 0);
		newSideSun.transform.localScale = new Vector3 (1.0F, panelHeight*40.0F, 2.0F * panelDepth);
		newSideSun.transform.parent = thisSide.transform;

		sideSunMaterial = new Material (Shader.Find ("Unlit/Texture"));
		newSideSun.GetComponent<MeshRenderer> ().material = sideSunMaterial;
		sideSunMaterial.mainTexture = Resources.Load (star[2]) as Texture;

		sideSunText = new GameObject();
		sideSunText.name = "Side Star Name";
		sideSunText.transform.position = new Vector3 (-0.47F * panelWidth, 22.0F * panelHeight, 0);
		sideSunText.transform.localScale = new Vector3 (0.1F, 0.1F, 0.1F);
		var sunTextMesh = sideSunText.AddComponent<TextMesh>();
		sunTextMesh.text = star[1];
		sunTextMesh.fontSize = 150;
		sideSunText.transform.parent = thisSide.transform;

		float innerHab = float.Parse (star[4]) * 9.5F;
		float outerHab = float.Parse (star[4]) * 14F;


		// need to take panelXScale into account for the hab zone

		habZone = GameObject.CreatePrimitive (PrimitiveType.Cube);
		habZone.name = "Hab";
		habZone.transform.position = new Vector3 ((-0.5F * panelWidth) + ((innerHab+outerHab) * 0.5F * panelXScale), 0, 0);
		habZone.transform.localScale = new Vector3 ((outerHab - innerHab)* panelXScale, 40.0F * panelHeight, 2.0F * panelDepth);
		habZone.transform.parent = thisSide.transform;

		// custom
		sideSunDistanceText = new GameObject ();
		sideSunDistanceText.name = "Side Star Distance";
		sideSunDistanceText.transform.position = new Vector3 (-1.1F * panelWidth, 22.0F * panelHeight, 0);
		sideSunDistanceText.transform.localScale = new Vector3 (0.1F, 0.1F, 0.1F);
		var sunTextMesh2 = sideSunDistanceText.AddComponent<TextMesh>();
		if (star [6] != null)
			sunTextMesh2.text = star [6] + " lightyears";
		else
			sunTextMesh2.text = "Unknown lightyears";
		sunTextMesh2.fontSize = 40;
		sideSunDistanceText.transform.parent = thisSide.transform;

		sideSunTypeText = new GameObject ();
		sideSunTypeText.name = "Side Star Distance";
		sideSunTypeText.transform.position = new Vector3 (-1.1F * panelWidth, 10.0F * panelHeight, 0);
		// Debug.Log ("Panelwidth: " + panelWidth + " Panelheight: " + panelHeight);
		sideSunTypeText.transform.localScale = new Vector3 (0.5F, 0.5F, 0.1F);
		var sunTextMesh3 = sideSunTypeText.AddComponent<TextMesh>();
		if (star [3] != null)
			sunTextMesh3.text = star [3] + " stellar classification";
		else
			sunTextMesh3.text = "Unknown stellar classification";
		sunTextMesh2.fontSize = 100;
		sideSunTypeText.transform.parent = thisSide.transform;

		// custom
		GameObject newObj = new GameObject ();
		newSidePanel.transform.parent = newObj.transform;
		newSideSun.transform.parent = newObj.transform;
		sideSunText.transform.parent = newObj.transform;
		habZone.transform.parent = newObj.transform;
		sideSunDistanceText.transform.parent = newObj.transform;
		sideSunTypeText.transform.parent = newObj.transform;
		newObj.transform.parent = thisSide.transform;
		newObj.transform.localPosition = Vector3.zero;

		habMaterial = new Material (Shader.Find ("Standard"));
		habZone.GetComponent<MeshRenderer> ().material = habMaterial;
		habMaterial.mainTexture = Resources.Load ("habitable") as Texture;

	}

	//------------------------------------------------------------------------------------//

	public void dealWithStar (string [] star, GameObject thisStar, GameObject theseOrbits){

		GameObject newSun, upperSun;
		Material sunMaterial;

		Light glowingLight;

		GameObject sunRelated;
		GameObject sunSupport;
		GameObject sunText;

		// GameObject to hold temp
		// GameObject temp = new GameObject();

		float sunScale = float.Parse(star [0]) / 100000F;
		float centerSunSize = 0.25F;

		// set the habitable zone based on the star's luminosity
		float innerHab = float.Parse (star[4]) * 9.5F;
		float outerHab = float.Parse (star[4]) * 14F;


		newSun = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		newSun.AddComponent<rotate> ();
		newSun.name = star[1];
		newSun.transform.position = new Vector3 (0, 0, 0);
		newSun.transform.localScale = new Vector3 (centerSunSize, centerSunSize, centerSunSize);

		sunRelated = thisStar;

		newSun.GetComponent<rotate> ().rotateSpeed = 4F; 

		sunMaterial = new Material (Shader.Find ("Unlit/Texture"));
		newSun.GetComponent<MeshRenderer> ().material = sunMaterial;
		sunMaterial.mainTexture = Resources.Load (star[2]) as Texture;

		newSun.transform.parent = sunRelated.transform;

		// copy the sun and make a bigger version above

		upperSun = Instantiate (newSun);
		upperSun.name = star[1] + " upper";
		upperSun.transform.localScale = new Vector3 (sunScale,sunScale,sunScale);
		upperSun.transform.position = new Vector3 (0, 20, 0);

		upperSun.AddComponent<Light> ();
		glowingLight = upperSun.GetComponent<Light> ();
		glowingLight.name = "starLight";
		// glowingLight.color = Color;
		glowingLight.intensity = 3;
		glowingLight.transform.position = new Vector3 (0, 5, 0);

		upperSun.transform.parent = sunRelated.transform;

		this.createdStars.Add (upperSun);

		// draw the support between them
		sunSupport = GameObject.CreatePrimitive (PrimitiveType.Cube);
		sunSupport.transform.localScale = new Vector3 (0.1F, 10.0F, 0.1F);
		sunSupport.transform.position = new Vector3 (0, 5, 0);
		sunSupport.name = "Sun Support";

		sunSupport.transform.parent = sunRelated.transform;


		sunText = new GameObject();
		sunText.name = "Star Name";
		sunText.transform.position = new Vector3 (0, 5, 0);
		sunText.transform.localScale = new Vector3 (0.1F, 0.1F, 0.1F);
		var sunTextMesh = sunText.AddComponent<TextMesh>();
		sunTextMesh.text = star[1];
		sunTextMesh.fontSize = 200;

		sunText.transform.parent = sunRelated.transform;

		/*// custom
		GameObject newObj = new GameObject ();
		newSun.transform.parent = newObj.transform;
		upperSun.transform.parent = newObj.transform;
		sunSupport.transform.parent = newObj.transform;
		sunText.transform.parent = newObj.transform;
		newObj.transform.parent = sunRelated.transform;

		if (numberOf3Dplanets == 0)
			newObj.transform.localPosition = Vector3.zero;
		else if (numberOf3Dplanets == 1)
			newObj.transform.localPosition = new Vector3 (0, -70, 0);
		else if (numberOf3Dplanets == 2)
			newObj.transform.localPosition = new Vector3 (0, -140, 0);
		else if (numberOf3Dplanets == 3)
			newObj.transform.localPosition = new Vector3 (0, -210, 0);
		else if (numberOf3Dplanets == 4)
			newObj.transform.localPosition = new Vector3 (0, -280, 0);*/

		GameObject LR1 = drawOrbit ("Habitable Inner Ring", innerHab * orbitXScale, Color.green, habWidth, theseOrbits);
		GameObject LR2 = drawOrbit ("Habitable Outer Ring", outerHab * orbitXScale, Color.green, habWidth, theseOrbits);

		GameObject temp = new GameObject ();
		temp.AddComponent<rotate> ();
		temp.GetComponent<rotate> ().rotateSpeed = newSun.GetComponent<rotate>().rotateSpeed;
		newSun.transform.parent = temp.transform;
		upperSun.transform.parent = temp.transform;
		sunSupport.transform.parent = temp.transform;
		sunText.transform.parent = temp.transform;
		// sunText.GetComponent<rotate> ().rotateSpeed = 0.0f;
		LR1.transform.parent = temp.transform;
		LR2.transform.parent = temp.transform;
		glowingLight.transform.parent = temp.transform;

		// temp.transform.parent = thisStar.transform;
		// temp.transform.localPosition = Vector3.zero;

		if (numberOf3Dsystems % 2 == 0)
			temp.transform.position = new Vector3 (0, 0, 0);
		else if (numberOf3Dsystems % 3 == 0)
			temp.transform.position = new Vector3 (0, -40, 0);

		current3dstars.Add (temp);
		// current3dsystems.Add (temp);
	}

	//------------------------------------------------------------------------------------//

//	void dealWithSystem(string[] starInfo, List<string[]> unscaledPlanetInfo, List<String[]> planetInfo, Vector3 offset, GameObject allThings){

//		GameObject SolarCenter;
//		GameObject AllOrbits;
//		GameObject SunStuff;
//		GameObject Planets;
//
//		SolarCenter = new GameObject();
//		AllOrbits = new GameObject();
//		SunStuff = new GameObject();
//		Planets = new GameObject();
//
//		SolarCenter.name = "SolarCenter" + " " + starInfo[1];
//		AllOrbits.name = "All Orbits" + " " + starInfo[1];
//		SunStuff.name = "Sun Stuff" + " " + starInfo[1];
//		Planets.name = "Planets" + " " + starInfo[1];
//
//		SolarCenter.transform.parent = allThings.transform;
//
//		AllOrbits.transform.parent = SolarCenter.transform;
//		SunStuff.transform.parent = SolarCenter.transform;
//		Planets.transform.parent = SolarCenter.transform;
//
//		dealWithStar (starInfo, SunStuff, AllOrbits);
//		dealWithPlanets (planetInfo, Planets, AllOrbits);
//
//		// need to do this last
//		SolarCenter.transform.position = offset;


//		// add in second 'flat' representation
//		GameObject SolarSide;
//		SolarSide = new GameObject();
//		SolarSide.name = "Side View of" + starInfo[1];
//
//
//		sideDealWithStar (starInfo, SolarSide, AllOrbits);
//		sideDealWithPlanets (unscaledPlanetInfo, SolarSide, AllOrbits);
//
//		SolarSide.transform.position = new Vector3 (0, 8, 10.0F);
//		SolarSide.transform.position += (offset * 0.15F);

//	}

	//------------------------------------------------------------------------------------//


	//Helper Functions to scale systems into view.

	public float getMaxDistance(List<string[]> planetSet){
		//Code Here.
		float maxDistance = 0F;
		foreach (var item in planetSet){
			if (maxDistance < float.Parse(item [0]))
				maxDistance = float.Parse(item [0]);
		}
		return maxDistance;
	}

			//_____________________________//

	public float getMaxSize(List<string[]> planetSet){
		//Code Here.
		float maxSize = 0F;
		foreach (var item in planetSet){
			if (maxSize < float.Parse(item [1]))
				maxSize = float.Parse(item [1]);
		}
		return maxSize;
	}
		

	//Scaling function.
	public List<string[]> scaleDistance(List<string[]> planetSet, float solMaxDist){
		//Code here.
		float maxDist = getMaxDistance(planetSet);
		foreach(var item in planetSet){
			float distRatio = float.Parse(item [0])/maxDist;
			float speedRatio= float.Parse (item [2]) / maxDist;
			float scaledDist = distRatio * solMaxDist;
			float scaledSpeed = speedRatio * solMaxDist;
			item [0] = Convert.ToString(scaledDist);
			item [2] = Convert.ToString (scaledSpeed);
		}
		return planetSet;
	}


	//Scaling function.
	public List<string[]> scaleSize(List<string[]> planetSet, float solMaxSize){
		//Code here.
		float maxSize = getMaxSize(planetSet);
		foreach(var item in planetSet){
			float sizeRatio = float.Parse(item [1])/maxSize;
			float scaledSize = sizeRatio * solMaxSize;
			item [1] = Convert.ToString(scaledSize);
		}
		return planetSet;
	}
		



	//------------------------------------------------------------------------------------//

	//Wrapper function to generate 2D systems. 
	public void generate2dSystem(List<string[]> starInfo, int systemId , List<List<string[]>> unscaledPlanetInfo, Vector3 offset, GameObject allThings){
		//Code here.

		// add in second 'flat' representation
		GameObject SolarSide;
		GameObject AllOrbits;
		GameObject SolarCenter;

		SolarSide = new GameObject ();
		AllOrbits = new GameObject ();
		SolarCenter = new GameObject ();

		SolarSide.name = "Side View of" + starInfo [1];
		SolarCenter.name = "SolarCenter" + " " + starInfo [1];
		AllOrbits.name = "All Orbits" + " " + starInfo [1];
		AllOrbits.transform.parent = SolarCenter.transform;

		// sideDealWithStar (starInfo[systemId], SolarSide, AllOrbits);
		// sideDealWithPlanets (unscaledPlanetInfo[systemId], SolarSide, AllOrbits);

		SolarSide.transform.position = new Vector3 (0, 8, 10.0F);
		SolarSide.transform.position += (offset * 0.15F);

		GameObject temp = buttonControl.GenerateButton ();
		temp.GetComponent<ButtonListButton> ().GetInfo (starInfo[systemId], systemId, unscaledPlanetInfo[systemId], offset, allThings);

		sideDealWithStar (starInfo[systemId], temp, AllOrbits);
		sideDealWithPlanets (unscaledPlanetInfo[systemId], temp, AllOrbits);

	}

	//Wrapper function to generate 3D systems.
	public void generate3dSystem(List<string[]> starInfo, int systemId , List<List<string[]>> planetInfo, Vector3 offset, GameObject allThings){
		GameObject SolarCenter;
		GameObject AllOrbits;
		GameObject SunStuff;
		GameObject Planets;

		SolarCenter = new GameObject();
		AllOrbits = new GameObject();
		SunStuff = new GameObject();
		Planets = new GameObject();

		SolarCenter.name = "SolarCenter" + " " + starInfo[1];
		AllOrbits.name = "All Orbits" + " " + starInfo[1];
		SunStuff.name = "Sun Stuff" + " " + starInfo[1];
		Planets.name = "Planets" + " " + starInfo[1];

		//'4503000000' is the distance of Uranus. and 69911 is the Size of Jupiter. This is to scale
		//down the systems WRT to the Solar System. You can scale either one.

//		List<string[]> systemPlanets = scaleSize(planetInfo[systemId], 69911);
		//		systemPlanets = scaleSize (systemPlanets, 4503000000);

		SolarCenter.transform.parent = allThings.transform;

		AllOrbits.transform.parent = SolarCenter.transform;
		SunStuff.transform.parent = SolarCenter.transform;
		Planets.transform.parent = SolarCenter.transform;

		// List<string[]> t = scaleSize (starInfo, 69911);
		// t = scaleDistance (t, 4503000000);

		dealWithStar (starInfo[systemId], SunStuff, AllOrbits);
		dealWithPlanets (planetInfo[systemId], Planets, AllOrbits);

		// need to do this last
		SolarCenter.transform.position = offset;
	}
	

	//------------------------------------------------------------------------------------//


	void Start () {

//		string[] sol = new string[5] { "695500", "Our Sun", "sol", "G2V" , "1.0"};
//
//		string[,] solPlan = new string[8, 5] {
//			{   "57910000",  "2440",    "0.24", "mercury", "mercury" },
//			{  "108200000",  "6052",    "0.62", "venus",   "venus" },
//			{  "149600000",  "6371",    "1.00", "earthmap", "earth" },
//			{  "227900000",  "3400",    "1.88", "mars",     "mars" },
//			{  "778500000", "69911",   "11.86", "jupiter", "jupiter" },
//			{ "1433000000", "58232",   "29.46", "saturn",   "saturn" },
//			{ "2877000000", "25362",   "84.01", "neptune", "uranus" },
//			{ "4503000000", "24622",  "164.80", "uranus", "neptune" }
//		};

//		List<string[]> solPlanets = new List<string[]> ();
//		foreach (var item in solPlan)
//			Console.WriteLine (item.GetType ());
//
//
//		string[] TauCeti = new string[5] { "556400", "Tau Ceti", "gstar", "G8.5V" , "0.52"};
//
//		List<string[]>TauCetiPlanets = new List<string[]> () {
//			{ "15707776",  "9009",   "0.04", "venus",   "b" },
//			{ "29171585", "11217",   "0.09", "venus", "c" },
//			{ "55949604", "12088",   "0.26", "mercury",  "d" },
//			{ "82578024", "13211",   "0.46", "mercury", "e" },
//			{"201957126", "16454",   "1.75", "uranus",  "f" }
//		};
//
//
//		string[] Gliese581 = new string[5] { "201750", "Gliese 581", "mstar", "M3V" , "0.013"};
//
//		List<string[]> Gliese581Planets = new List<string[]> () {
//			{ "4188740",  "8919",   "0.009", "venus",   "e" },
//			{ "6133513", "30554",   "0.014", "jupiter",   "b" },
//			{"10920645", "20147",   "0.18", "neptune",  "c" },
//			{"201957126", "16454",   "1.75", "uranus",  "f" },
//			{  "778500000", "69911",   "11.86", "jupiter", "jupiter" }
//		};

		XmlDocument doc = new XmlDocument();
		doc.Load(@"Assets/systems_final.xml");

		XmlNodeList l = doc.SelectNodes ("/systems/system");

		// Instantiate System list
		List<System> systemList = new List<System> ();
		// Loop through systems
		foreach (XmlNode node in l) {

			// Add system data
			System system = new System ();

			// Add system name
			system.name = node.SelectSingleNode ("child::name").InnerText;
			if (node.SelectSingleNode ("child::declination") != null)
				system.declination = node.SelectSingleNode ("child::declination").InnerText;
			else
				system.declination = null;

			// Add system rightascension
			if (node.SelectSingleNode ("child::rightascension") != null)
				system.rightascension = node.SelectSingleNode ("child::rightascension").InnerText;
			else
				system.rightascension = null;

			// Add system distance
			if (node.SelectSingleNode ("child::distance") != null)
				system.distance = float.Parse (node.SelectSingleNode ("child::distance").InnerText);
			else
				system.distance = 0.0f;

			// Add star data
			// Binary star condition
			if (node.SelectSingleNode ("child::binary") != null) {
				//					Console.WriteLine (system.name + ": Binary Star");
			} else if (node.SelectSingleNode ("child::star") != null) {
				// Unary star condition
				// Instantiate new Star object
				Star sTemp = new Star ();
				// Select XMLNode star
				XmlNode sTempNode = node.SelectSingleNode ("child::star");

				// Add star name
				if (sTempNode.SelectSingleNode ("child::name") != null)
					sTemp.name = sTempNode.SelectSingleNode ("child::name").InnerText;
				else
					sTemp.name = system.name;
				// Console.WriteLine ("Current star: " + sTemp.name);

				// Add star radius
				if (sTempNode.SelectSingleNode ("child::radius") != null)
					sTemp.radius = float.Parse (sTempNode.SelectSingleNode ("child::radius").InnerText);
				else
					sTemp.radius = 1.0f;

				// Add star spectral type
				if (sTempNode.SelectSingleNode ("child::spectraltype") != null)
					sTemp.spectralType = sTempNode.SelectSingleNode ("child::spectraltype").InnerText;
				else
					sTemp.spectralType = null;

				// Add star temperature
				if (sTempNode.SelectSingleNode ("child::temperature") != null)
					sTemp.temperature = float.Parse (sTempNode.SelectSingleNode ("child::temperature").InnerText);
				else
					sTemp.temperature = 0.0f;

				// Add star age
				if (sTempNode.SelectSingleNode ("child::age") != null) {
					if (sTempNode.SelectSingleNode ("child::age").Attributes ["upperlimit"] != null)
						sTemp.age = float.Parse (sTempNode.SelectSingleNode ("child::age").Attributes ["upperlimit"].InnerText);
					else if (sTempNode.SelectSingleNode ("child::age").Attributes ["lowerlimit"] != null)
						sTemp.radius = float.Parse (sTempNode.SelectSingleNode ("child::age").Attributes ["lowerlimit"].InnerText);
					else
						sTemp.age = float.Parse(sTempNode.SelectSingleNode ("child::age").InnerText);
				} else
					sTemp.age = 0.0f;

				// Add star metallicity
				if (sTempNode.SelectSingleNode ("child::metallicity") != null) {
					if (sTempNode.SelectSingleNode ("child::metallicity").Attributes ["upperlimit"] != null)
						sTemp.metallicity = float.Parse (sTempNode.SelectSingleNode ("child::metallicity").Attributes ["upperlimit"].InnerText);
					else
						sTemp.metallicity = float.Parse(sTempNode.SelectSingleNode ("child::metallicity").InnerText);
				} else
					sTemp.metallicity = 0.0f;

				// Add star mass
				if (sTempNode.SelectSingleNode ("child::mass") != null) {
					if (sTempNode.SelectSingleNode ("child::mass").Attributes ["upperlimit"] != null)
						sTemp.mass = float.Parse (sTempNode.SelectSingleNode ("child::mass").Attributes ["upperlimit"].InnerText);
					else
						sTemp.mass = float.Parse(sTempNode.SelectSingleNode ("child::mass").InnerText);
				} else
					sTemp.mass = 0.0f;



				// Calculate star luminosity
				if (sTemp.radius != 0.0f && sTemp.temperature != 0.0f) {
					float luminosityTemp = (float)Math.PI * 4.0f * (5.670373e-8f);
					float rSquared = (float)Math.Pow (sTemp.radius * 695700000.0f, 2);
					float tQuad = (float)Math.Pow (sTemp.temperature, 4);
					luminosityTemp = luminosityTemp * rSquared * tQuad;
					sTemp.luminosity = (float)(luminosityTemp / 3.828e26);
				} else
					sTemp.luminosity = 1.0f;

				// Convert solar radius to KM
				sTemp.radius *= 695700.0f;

				// Convert solar mass to KG
				sTemp.mass *= (float) 1.99e30;

				// Add planet data
				// Instantiate Planet list of current Star
				sTemp.planets = new List<Planet> ();
				// Select XMLNodeList planet
				XmlNodeList pTempList = sTempNode.SelectNodes ("child::planet");

				// Loop through available planets .+
				foreach (XmlNode pTempNode in pTempList) {

					Planet pTemp = new Planet ();

					// Add planet name
					if (pTempNode.SelectSingleNode ("child::name") != null)
						pTemp.name = pTempNode.SelectSingleNode ("child::name").InnerText;
					else
						pTemp.name = system.name;

					// Add planet eccentricity
					if (pTempNode.SelectSingleNode ("child::eccentricity") != null) {
						if (pTempNode.SelectSingleNode ("child::eccentricity").Attributes ["upperlimit"] != null)
							pTemp.eccentricity = float.Parse (pTempNode.SelectSingleNode ("child::eccentricity").Attributes ["upperlimit"].InnerText);
						else
							pTemp.eccentricity = float.Parse(pTempNode.SelectSingleNode ("child::eccentricity").InnerText);
					} else
						pTemp.eccentricity = 0.0f;

					// Add planet mass
					if (pTempNode.SelectSingleNode ("child::mass") != null) {
						if (pTempNode.SelectSingleNode ("child::mass").Attributes ["upperlimit"] != null)
							pTemp.mass = float.Parse (pTempNode.SelectSingleNode ("child::mass").Attributes ["upperlimit"].InnerText);
						else
							pTemp.mass = float.Parse(pTempNode.SelectSingleNode ("child::mass").InnerText);
					} else
						pTemp.mass = 0.0f;

					// Add planet radius
					if (pTempNode.SelectSingleNode ("child::radius") != null) {
						if (pTempNode.SelectSingleNode ("child::radius").Attributes ["upperlimit"] != null)
							pTemp.radius = float.Parse (pTempNode.SelectSingleNode ("child::radius").Attributes ["upperlimit"].InnerText);
						else if (pTempNode.SelectSingleNode ("child::radius").Attributes ["lowerlimit"] != null)
							pTemp.radius = float.Parse (pTempNode.SelectSingleNode ("child::radius").Attributes ["lowerlimit"].InnerText);
						else
							pTemp.radius = float.Parse(pTempNode.SelectSingleNode ("child::radius").InnerText);
					} else
						pTemp.radius = 0.0f;

					// Add planet radius to mass correlation

					if (pTemp.radius != 0.0f && pTemp.mass == 0.0f) {
						float earthRadius = 0.091f;
						float earthMass = 0.003f;
						float oldRange, oldMax, oldMin, newMin, newMax, newRange;
						if ((pTemp.radius/earthRadius) < (1.25 * earthRadius)) {
							oldMin = 0.0f;
							oldMax = 1.25f;
							newMin = 0.0f;
							newMax = 2.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.mass = (Math.Abs(((((pTemp.radius/earthRadius) - oldMin) * newRange) / oldRange) + newMin) * earthMass);
						} else if (((pTemp.radius/earthRadius) >= (1.25f * earthRadius)) && ((pTemp.radius/earthRadius) < (2.0f * earthRadius))) {
							oldMin = 1.25f;
							oldMax = 2.0f;
							newMin = 2.0f;
							newMax = 5.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.mass = (Math.Abs(((((pTemp.radius/earthRadius) - oldMin) * newRange) / oldRange) + newMin) * earthMass);
						} else if (((pTemp.radius/earthRadius) >= (2.0f * earthRadius)) && ((pTemp.radius/earthRadius) < (3.0f * earthRadius))) {
							oldMin = 2.0f;
							oldMax = 3.0f;
							newMin = 5.0f;
							newMax = 10.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.mass = (Math.Abs(((((pTemp.radius/earthRadius) - oldMin) * newRange) / oldRange) + newMin) * earthMass);
						} else if (((pTemp.radius/earthRadius) >= (3.0f * earthRadius)) && ((pTemp.radius/earthRadius) < (6.0f * earthRadius))) {
							oldMin = 3.0f;
							oldMax = 6.0f;
							newMin = 10.0f;
							newMax = 30.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.mass = (Math.Abs(((((pTemp.radius/earthRadius) - oldMin) * newRange) / oldRange) + newMin) * earthMass);
						} else if (((pTemp.radius/earthRadius) >= (6.0f * earthRadius)) && ((pTemp.radius/earthRadius) < (15.0f * earthRadius))) {
							oldMin = 6.0f;
							oldMax = 15.0f;
							newMin = 30.0f;
							newMax = 300.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.mass = (Math.Abs(((((pTemp.radius/earthRadius) - oldMin) * newRange) / oldRange) + newMin) * earthMass);
						} else if ((pTemp.radius/earthRadius) >= (15.0f * earthRadius)) {
							oldMin = 15.0f;
							oldMax = 100.0f;
							newMin = 300.0f;
							newMax = 3000.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.mass = (Math.Abs(((((pTemp.radius/earthRadius) - oldMin) * newRange) / oldRange) + newMin) * earthMass);
						}
					}

					// Add planet mass to radius correlation
					if (pTemp.radius == 0.0f && pTemp.mass != 0.0f) {
						float earthRadius = 0.091f;
						float earthMass = 0.003f;
						float oldRange, oldMax, oldMin, newMin, newMax, newRange;
						if ((pTemp.mass/earthMass) < (2 * earthMass)) {
							oldMin = 0.0f;
							oldMax = 2.0f;
							newMin = 0.0f;
							newMax = 1.25f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.radius = (Math.Abs(((((pTemp.mass/earthMass) - oldMin) * newRange) / oldRange) + newMin) * earthRadius);
						} else if (((pTemp.mass/earthMass) >= (2 * earthMass)) && ((pTemp.mass/earthMass) < (5 * earthMass))) {
							oldMin = 2.0f;
							oldMax = 5.0f;
							newMin = 1.25f;
							newMax = 2.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.radius = (Math.Abs(((((pTemp.mass/earthMass) - oldMin) * newRange) / oldRange) + newMin) * earthRadius);
						} else if (((pTemp.mass/earthMass) >= (5 * earthMass)) && ((pTemp.mass/earthMass) < (10 * earthMass))) {
							oldMin = 5.0f;
							oldMax = 10.0f;
							newMin = 2.0f;
							newMax = 3.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.radius = (Math.Abs(((((pTemp.mass/earthMass) - oldMin) * newRange) / oldRange) + newMin) * earthRadius);
						} else if (((pTemp.mass/earthMass) >= (10 * earthMass)) && ((pTemp.mass/earthMass) < (30 * earthMass))) {
							oldMin = 10.0f;
							oldMax = 30.0f;
							newMin = 3.0f;
							newMax = 6.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.radius = (Math.Abs(((((pTemp.mass/earthMass) - oldMin) * newRange) / oldRange) + newMin) * earthRadius);
						} else if (((pTemp.mass/earthMass) >= (30 * earthMass)) && ((pTemp.mass/earthMass) < (300 * earthMass))) {
							oldMin = 30.0f;
							oldMax = 300.0f;
							newMin = 6.0f;
							newMax = 15.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.radius = (Math.Abs(((((pTemp.mass/earthMass) - oldMin) * newRange) / oldRange) + newMin) * earthRadius);
						} else if ((pTemp.mass/earthMass) >= (15 * earthMass)) {
							oldMin = 300.0f;
							oldMax = 3000.0f;
							newMin = 15.0f;
							newMax = 100.0f;
							oldRange = (oldMax - oldMin);
							newRange = (newMax - newMin);
							pTemp.radius = (Math.Abs(((((pTemp.mass/earthMass) - oldMin) * newRange) / oldRange) + newMin) * earthRadius);
						}
					}

					// Change radius ratio to km
					pTemp.radius *= (float) 69911;

					// Change mass ratio to kg
					pTemp.mass *= (float) 1.898e27;

					// Add planet temperature
					if (pTempNode.SelectSingleNode ("child::temperature") != null) {
						if (pTempNode.SelectSingleNode ("child::temperature").Attributes ["upperlimit"] != null)
							pTemp.temperature = float.Parse (pTempNode.SelectSingleNode ("child::temperature").Attributes ["upperlimit"].InnerText);
						else
							pTemp.temperature = float.Parse(pTempNode.SelectSingleNode ("child::temperature").InnerText);
					} else
						pTemp.temperature = 0.0f;

					// Add planet age
					if (pTempNode.SelectSingleNode ("child::age") != null) {
						if (pTempNode.SelectSingleNode ("child::age").Attributes ["upperlimit"] != null)
							pTemp.age = float.Parse (pTempNode.SelectSingleNode ("child::age").Attributes ["upperlimit"].InnerText);
						else
							pTemp.age = float.Parse(pTempNode.SelectSingleNode ("child::age").InnerText);
					} else
						pTemp.age = 0.0f;

					// Add planet period
					if (pTempNode.SelectSingleNode ("child::period") != null) {
						if (pTempNode.SelectSingleNode ("child::period").Attributes ["upperlimit"] != null)
							pTemp.period = float.Parse (pTempNode.SelectSingleNode ("child::period").Attributes ["upperlimit"].InnerText);
						else
							pTemp.period = float.Parse(pTempNode.SelectSingleNode ("child::period").InnerText);
					} else
						pTemp.period = 365.0f;

					// Add planet semimajoraxis
					if (pTempNode.SelectSingleNode ("child::semimajoraxis") != null) {
						if (pTempNode.SelectSingleNode ("child::semimajoraxis").Attributes ["upperlimit"] != null)
							pTemp.semiMajorAxis = float.Parse (pTempNode.SelectSingleNode ("child::semimajoraxis").Attributes ["upperlimit"].InnerText);
						else
							pTemp.semiMajorAxis = float.Parse(pTempNode.SelectSingleNode ("child::semimajoraxis").InnerText);
					}

					// Add planet transittime
					if (pTempNode.SelectSingleNode ("child::transittime") != null) {
						if (pTempNode.SelectSingleNode ("child::transittime").Attributes ["upperlimit"] != null)
							pTemp.transitTime = float.Parse (pTempNode.SelectSingleNode ("child::transittime").Attributes ["upperlimit"].InnerText);
						else
							pTemp.transitTime = float.Parse(pTempNode.SelectSingleNode ("child::transittime").InnerText);
					} else
						pTemp.transitTime = 0.0f;

					// Add planet discovery method
					if (pTempNode.SelectSingleNode ("child::discoverymethod") != null)
						pTemp.discoveryMethod = pTempNode.SelectSingleNode ("child::discoverymethod").InnerText;
					else
						pTemp.discoveryMethod = null;

					// Add planet distance from star
					if (pTemp.semiMajorAxis != 0.0f)
						pTemp.distanceFromStar = (pTemp.semiMajorAxis * 149600000.0f);
					else
						pTemp.distanceFromStar = sTemp.radius / 2;

					// Add planet speed
					if (pTemp.period != 0.0f)
						pTemp.speed = (pTemp.period / 365.0f);
					else
						pTemp.speed = 1.0f;

					//system.star.planets.Add(pTemp);
					sTemp.planets.Add (pTemp);
				}
				system.star = sTemp;
			} else {
				// No star condition
				Console.WriteLine (system.name + ": No Star");
			}
			systemList.Add (system);
		}
			
		//Parsing/Generating Values for systems.
		foreach (System s in systemList) {
			if (s.star != null) {
				/*
				 * For all systems, maxTemperature = 11361K, minTemperature = 540K.
				 * We define six temperature ranges:
				 * 	range 1: 540 - 2086,
				 * 	range 2 : 2087 - 3632,
				 * 	range 3 : 3633 - 5178,
				 * 	range 4 : 5179 - 6724,
				 * 	range 5 : 6725 - 8270,
				 * 	range 6 : 8271 - 9816,
				 * 	range 7 : >9816
				*/
				//Fill temperature data holes.

				// custom
				/*using (StreamReader sr = new StreamReader("farthest.txt")) {
					while (sr.Peek () >= 0) {
						if (sr.ReadLine () == s.name) {
							farthestSystems.Add (s);
						}
					}
				}

				using (StreamReader sr = new StreamReader ("nearest.txt")) {
					while (sr.Peek () >= 0) {
						if (sr.ReadLine () == s.name) {
							nearestSystems.Add (s);
						}
					}
				}*/

				float temperature = 0.0F;
				string texture = "sol"; //Deafult Teexture is our sun's.

				if (float.Parse (s.star.temperature.ToString ()) == 0.0F)
					temperature = 5778;
				else
					temperature = float.Parse(s.star.temperature.ToString ());

				if (temperature<2086)
					texture = "scale_1";
				else if (temperature > 2086 && temperature <= 3632)
					texture = "scale_2";
				else if (temperature > 3632 && temperature <= 5178)
					texture = "scale_3";
				else if (temperature > 5178 && temperature <= 6724)
					texture = "sol";
				else if (temperature > 6724 && temperature <= 8270)
					texture = "scale_5";
				else if (temperature > 8270 && temperature <= 9816)
					texture = "scale_6";
				else if (temperature > 9816)
					texture = "scale_7";

				// Get string[] for current star.
				string[] star = new string[7] { s.star.radius.ToString(), s.star.name, texture, s.star.spectralType, s.star.luminosity.ToString() , Convert.ToString(temperature), (s.distance * 3.26156f).ToString()};

				starData.Add (star);

				List<string[]> thisStarPlanets = new List<string[]>();

				// Loop through planets in current system
				foreach (Planet p in s.star.planets) {
					// Fill temperature data holes

					float radius = p.radius / 69911;
					string planetTexture = "earthmap";

					if (radius<=0.301)
						planetTexture = "mercury";
					else if (radius > 0.301 && radius <= 0.542)
						planetTexture = "mars";
					else if (radius > 0.542 && radius <= 0.783)
						planetTexture = "venus";
					else if (radius > 0.783 && radius <= 1.024)
						planetTexture = "earthmap";
					else if (radius > 1.024 && radius <= 1.265)
						planetTexture = "neptune";
					else if (radius > 1.265 && radius <= 1.506)
						planetTexture = "uranus";
					else if (radius > 1.506 && radius <=1.747)
						planetTexture = "saturn";
					else if (radius > 1.747)
						planetTexture = "jupiter";

					// Get string[] for current planet
					string[] eachPlanet = new string[6] { p.distanceFromStar.ToString(), p.radius.ToString(), p.speed.ToString(), planetTexture, p.name, p.discoveryMethod };

					thisStarPlanets.Add (eachPlanet);

					if (float.Parse (eachPlanet [1]) > 0.783 && float.Parse (eachPlanet [1]) <= 1.024)
						earthSizedPlanets.Add (eachPlanet);

//					// Keep track of planet data
//					planetCount++;
//					if (p.radius != 0.0f)
//						radiusCount++;
//					if (p.mass != 0.0f)
//						massCount++;
//					if (p.semiMajorAxis != 0.0f)
//						semimajoraxisCount++;
//					if (p.period != 0.0f)
//						periodCount++;

					// Display planet data
					//						Console.WriteLine ("\t\t" + "Planet Name: " + p.name);
					//						Console.WriteLine ("\t\t" + "Planet Mass: " + p.mass);
					//						Console.WriteLine ("\t\t" + "Planet Radius: " + p.radius);
					//						Console.WriteLine ("\t\t" + "Planet SemiMajorAxis: " + p.semiMajorAxis);
					//						Console.WriteLine ("\t\t" + "Planet Distance From Star: " + p.distanceFromStar);
					//						Console.WriteLine ("\t\t" + "Planet Speed: " + p.speed);
				}
				planetData.Add(thisStarPlanets);
			}
			else {
				Console.WriteLine ("Binary system or no star system");
			}
		}
			
		GameObject allCenter = new GameObject();
		allCenter.name = "all systems";

		var systemOffset = new Vector3 (0, 3, 0);
		var oneOffset = new Vector3 (0, -70, 0);

//		List<string[]> starData = new List<string[]> ();
//		List<List<string[]>> planetData = new List<List<string[]>> ();

//		dealWithSystem (sol, solPlanets, systemOffset, allCenter);

		systemOffset += oneOffset;

		//Counter for keeping a track of which planets to plot against a certain star.
		int systemPlanetCounter = 0;

		foreach (var sys in starData){
			systemOffset += oneOffset;
			// generate3dSystem (starData, systemPlanetCounter , planetData , systemOffset, allCenter);
			generate2dSystem (starData, systemPlanetCounter, planetData, systemOffset, allCenter);
			// Debug.Log ("Count: " + systemPlanetCounter + " + " + sys[1]);
			systemPlanetCounter += 1;
		}

		/*foreach (System s in farthestSystems) {
			systemOffset += oneOffset;
			generate2dSystem (starData, systemPlanetCounter, planetData, systemOffset, allCenter);
			systemPlanetCounter += 1;
		}*/

		Debug.Log (systemPlanetCounter);

//		generate3dSystem (starData, 20 , planetData , systemOffset, allCenter);
//		generate2dSystem (starData, 20, planetData, systemOffset, allCenter);
//
//		systemOffset += oneOffset;
//
//		generate3dSystem (starData, 300 , planetData , systemOffset, allCenter);
//		generate2dSystem (starData, 300, planetData, systemOffset, allCenter);



//		dealWithSystem (TauCeti, TauCetiPlanets, systemOffset, allCenter);
//		systemOffset += oneOffset;
//		dealWithSystem (Gliese581, Gliese581Planets, systemOffset, allCenter);
//		systemOffset += oneOffset;
//		dealWithSystem (testStar, testPlanet, systemOffset, allCenter);
//		systemOffset += oneOffset;
		allCenter.transform.localScale = new Vector3 (0.1F, 0.1F, 0.1F);
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}