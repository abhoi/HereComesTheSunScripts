using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListControl : MonoBehaviour {
	[SerializeField]
	private GameObject buttonTemplate;
	public GameObject holderTemplate;
	/*[SerializeField]
	private int[] intArray;*/

	private List<GameObject> buttons;

	/*void Start() {

		buttons = new List<GameObject> ();

		if (buttons.Count > 0) {
			foreach (GameObject button in buttons) {
				Destroy (button.gameObject);
			}

			buttons.Clear ();
		}

		for (int i = 0; i < 20; i++) {
			GameObject button = Instantiate (buttonTemplate) as GameObject;
			button.SetActive (true);

			button.GetComponent<ButtonListButton> ().SetText ("Button: " + i);

			button.transform.SetParent (buttonTemplate.transform.parent, false);
		}
	}*/

	public GameObject GenerateButton() {
		GameObject button = Instantiate (buttonTemplate) as GameObject;
		button.SetActive (true);

		button.GetComponent<ButtonListButton> ().SetText ("Button");

		button.transform.SetParent (buttonTemplate.transform.parent, false);
		return button;
	}
	public void ButtonClicked(string myTextString) {
		Debug.Log (myTextString);
	}
}
