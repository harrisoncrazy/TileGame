using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public GameObject resourcePanel;
	public GameObject peoplePanel;
	public GameObject needsPanel;

	public GameObject expeditionPanel;
	public GameObject expeditonPrefab;

	//People Array
	public Text[] peopleTexts;

	//People Sprite Array
	public Image[] peopleHeads;

	//Resource Texts
	public Text foodTextResourcePanel;
	public Text waterTextResourcePanel;

	//Expedition Stuff
	public bool arrangingExpedition = false;
	public bool finalizingExpediton = false;
	public Toggle expPerson1;
	public Toggle expPerson2;
	public Toggle expPerson3;
	public Toggle expPerson4;
	public Text expPerson1Text;
	public Text expPerson2Text;
	public Text expPerson3Text;
	public Text expPerson4Text;

	// Use this for initialization
	void Start () {
		peopleTexts = new Text[7];
		peopleHeads = new Image[7];
		for (int i = 1; i <= 7; i++) {
			peopleTexts [i - 1] = GameObject.Find ("p" + i + "Text").GetComponent<Text> ();
			peopleHeads [i - 1] = GameObject.Find ("p" + i + "Head").GetComponent<Image> ();
		}
		checkPeople();
		checkFood ();
		checkWater ();

		peoplePanel.SetActive (false);
		needsPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		checkFood ();
		checkWater ();
		checkPeople ();

		if (arrangingExpedition == true) {
			expPerson1Text.text = GameManager.Instance.playerPeople [0];
			expPerson2Text.text = GameManager.Instance.playerPeople [1];
			expPerson3Text.text = GameManager.Instance.playerPeople [2];
			expPerson4Text.text = GameManager.Instance.playerPeople [3];

			if (finalizingExpediton == true) {
				expeditionHandler expParty = ((GameObject)Instantiate (expeditonPrefab, GameObject.Find("homeBase").transform.position, GameObject.Find("homeBase").transform.rotation)).GetComponent<expeditionHandler> ();//instanciating the expidition
				if (expPerson1.isOn == true) {
					expParty.expeditionPeople[0] = GameManager.Instance.playerPeople [0];
					expParty.expeditionPeopleNum++;
					GameManager.Instance.playerPeople [0] = null;
				}
				if (expPerson2.isOn == true) {
					expParty.expeditionPeople[1] = GameManager.Instance.playerPeople [1];
					expParty.expeditionPeopleNum++;
					GameManager.Instance.playerPeople [1] = null;
				}
				if (expPerson3.isOn == true) {
					expParty.expeditionPeople[2] = GameManager.Instance.playerPeople [2];
					expParty.expeditionPeopleNum++;
					GameManager.Instance.playerPeople [2] = null;
				}
				if (expPerson4.isOn == true) {
					expParty.expeditionPeople[3] = GameManager.Instance.playerPeople [3];
					expParty.expeditionPeopleNum++;
					GameManager.Instance.playerPeople [3] = null;
				}
				finalizingExpediton = false;
				arrangingExpedition = false;
				expeditionPanel.SetActive (false);
			}
		}
	}

	void checkPeople() {
		for (int i = 0; i <= 6; i++) {
			peopleTexts [i].text = GameManager.Instance.playerPeople [i];
			if (GameManager.Instance.playerPeople [i] == null) {
				peopleHeads [i].enabled = false;
			}
		}
	}

	void checkFood() {
		int totalFoodVal = 0;
		for (int i = 0; i <= GameManager.Instance.storedFood.Length-1; i++) {
			totalFoodVal += GameManager.Instance.storedFood [i].foodVal;
		}
		foodTextResourcePanel.text = "" + totalFoodVal;
	}

	void checkWater() {
		int totalWaterVal = GameManager.Instance.waterStore;
		waterTextResourcePanel.text = "" +  totalWaterVal;
	}

	public void ToggleResourcePanel() {
		resourcePanel.SetActive (true);
		peoplePanel.SetActive (false);
		needsPanel.SetActive (false);
	}

	public void TogglePeoplePanel() {
		resourcePanel.SetActive (false);
		peoplePanel.SetActive (true);
		needsPanel.SetActive (false);
	}

	public void ToggleNeedsPanel() {
		resourcePanel.SetActive (false);
		peoplePanel.SetActive (false);
		needsPanel.SetActive (true);
	}

	public void NextTurn() {
		GameManager.Instance.NewTurn = true;
		expeditionHandler.Instance.NewTurn = true;
	}

	public void ArrangeExpedition() {
		if (arrangingExpedition == false) {
			arrangingExpedition = true;
			expeditionPanel.SetActive (true);
		} else if (arrangingExpedition == true) {
			arrangingExpedition = false;
			expeditionPanel.SetActive (false);
		}
	}

	public void FinalizeExpedition() {
		finalizingExpediton = true;
	}
}
