using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager Instance;

	public GameObject resourcePanel;
	public GameObject peoplePanel;
	public GameObject needsPanel;

	public GameObject expeditionPanelBase;
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
	public bool expeditionEnabled = false;
	public Toggle expPerson1;
	public Toggle expPerson2;
	public Toggle expPerson3;
	public Toggle expPerson4;
	public Text expPerson1Text;
	public Text expPerson2Text;
	public Text expPerson3Text;
	public Text expPerson4Text;
	public GameObject expeditionPanel;
	public InputField expeditionWaterInput;
	private bool waterError = false;
	public Text waterErrorText;
	public Text expeditionFoodTextResourcePanel;
	public Text expeditionWaterTextResourcePanel;

	// Use this for initialization
	void Start () {
		Instance = this;
		//City Panel UI Stuff
		peopleTexts = new Text[7];
		peopleHeads = new Image[7];
		for (int i = 1; i <= 7; i++) {
			peopleTexts [i - 1] = GameObject.Find ("p" + i + "Text").GetComponent<Text> ();
			peopleHeads [i - 1] = GameObject.Find ("p" + i + "Head").GetComponent<Image> ();
		}
		checkPeopleBase();
		checkFoodBase();
		checkWaterBase();

		peoplePanel.SetActive (false);
		needsPanel.SetActive (false);


		//Expedition Panel UI stuff
	}
	
	// Update is called once per frame
	void Update () {
		checkFoodBase ();
		checkWaterBase ();
		checkPeopleBase ();

		if (expeditionEnabled == true) {
			checkFoodExpedition ();
			checkWaterExpedition ();
			//checkPeopleExpedition ();
		}

		if (arrangingExpedition == true) {
			expPerson1.gameObject.SetActive (true);
			expPerson2.gameObject.SetActive (true);
			expPerson3.gameObject.SetActive (true);
			expPerson4.gameObject.SetActive (true);

			if (GameManager.Instance.playerPeople [0] == null) {
				expPerson1.gameObject.SetActive (false);
			} 
			if (GameManager.Instance.playerPeople [1] == null) {
				expPerson2.gameObject.SetActive (false);
			} 
			if (GameManager.Instance.playerPeople [2] == null) {
				expPerson3.gameObject.SetActive (false);
			}
			if (GameManager.Instance.playerPeople [3] == null) {
				expPerson4.gameObject.SetActive (false);
			} 


			expPerson1Text.text = GameManager.Instance.playerPeople [0];
			expPerson2Text.text = GameManager.Instance.playerPeople [1];
			expPerson3Text.text = GameManager.Instance.playerPeople [2];
			expPerson4Text.text = GameManager.Instance.playerPeople [3];
			int waterValToTake = int.Parse (expeditionWaterInput.text);
			if (waterValToTake < GameManager.Instance.waterStore) {
				waterError = true;
				waterErrorText.enabled = true;
			} else if (waterValToTake >= GameManager.Instance.waterStore) {
				waterError = false;
				waterErrorText.enabled = false;
			}

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
				expeditionPanelBase.SetActive (false);

				expParty.ExpeditionWaterStore = waterValToTake;
				GameManager.Instance.playerPeopleNum -= expParty.expeditionPeopleNum;
				expeditionEnabled = true;
			}
		}
	}

	//Checking the base values
	void checkPeopleBase() {
		for (int i = 0; i <= 6; i++) {
			peopleTexts [i].text = GameManager.Instance.playerPeople [i];
			if (GameManager.Instance.playerPeople [i] == null) {
				peopleHeads [i].enabled = false;
			}
		}
	}

	void checkFoodBase() {
		int totalFoodVal = 0;
		for (int i = 0; i <= GameManager.Instance.storedFood.Length-1; i++) {
			totalFoodVal += GameManager.Instance.storedFood [i].foodVal;
		}
		foodTextResourcePanel.text = "" + totalFoodVal;
	}

	void checkWaterBase() {
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


	//Checking the expedition party values
	/*void checkPeopleExpedition() {
		for (int i = 0; i <= 6; i++) {
			peopleTexts [i].text = expeditionHandler.Instance.expeditionPeople [i];
			if (expeditionHandler.Instance.expeditionPeople [i] == null) {
				peopleHeads [i].enabled = false;
			}
		}
	}*/

	void checkFoodExpedition() {
		int totalFoodVal = 0;
		for (int i = 0; i <= expeditionHandler.Instance.storedFood.Length-1; i++) {
			totalFoodVal += expeditionHandler.Instance.storedFood [i].foodVal;
		}
		expeditionFoodTextResourcePanel.text = "" + totalFoodVal;
	}

	void checkWaterExpedition() {
		int totalWaterVal = expeditionHandler.Instance.ExpeditionWaterStore;
		expeditionWaterTextResourcePanel.text = "" +  totalWaterVal;
	}


	public void NextTurn() {
		GameManager.Instance.NewTurn = true;
		expeditionHandler.Instance.NewTurn = true;
	}

	public void ArrangeExpedition() {
		if (arrangingExpedition == false) {
			arrangingExpedition = true;
			expeditionPanelBase.SetActive (true);
		} else if (arrangingExpedition == true) {
			arrangingExpedition = false;
			expeditionPanelBase.SetActive (false);
		}
	}

	public void FinalizeExpedition() {
		if (waterError != true) {
			finalizingExpediton = true;
		}
	}
}
