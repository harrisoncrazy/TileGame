using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public GameObject resourcePanel;
	public GameObject peoplePanel;

	//People Array
	public Text[] peopleTexts;

	//People Sprite Array
	public Image[] peopleHeads;

	// Use this for initialization
	void Start () {
		peopleTexts = new Text[32];
		peopleHeads = new Image[32];
		for (int i = 1; i <= 7; i++) {
			peopleTexts [i - 1] = GameObject.Find ("p" + i + "Text").GetComponent<Text> ();
			peopleHeads [i - 1] = GameObject.Find ("p" + i + "Head").GetComponent<Image> ();
		}

		for (int i = 0; i <= 7; i++) {
			if (GameManager.Instance.playerPeople[0] == "Tim") {
				peopleTexts [i].text = GameManager.Instance.playerPeople [i];
				peopleHeads [i].enabled = true;
			} else if (GameManager.Instance.playerPeople [i] == "null") {
				peopleTexts [i].text = " ";
				peopleHeads [i].enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleResourcePanel() {
		resourcePanel.SetActive (true);
		peoplePanel.SetActive (false);
	}

	public void TogglePeoplePanel() {
		resourcePanel.SetActive (false);
		peoplePanel.SetActive (true);
	}
}
