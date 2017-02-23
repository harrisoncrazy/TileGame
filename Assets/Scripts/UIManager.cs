using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager Instance;

	//home base ui elements
	public GameObject resourcePanel;
	public GameObject peoplePanel;
	public GameObject needsPanel;
	public GameObject expeditionPanelBase;

	//prefab for expedition
	public GameObject expeditonPrefab;

	//People Array
	public Text[] peopleTexts;

	//People Sprite Array
	public Image[] peopleHeads;

	//Resource Texts
	public Text foodTextResourcePanel;
	public Text waterTextResourcePanel;
	public Text woodTextResourcePanel;
	public Text stoneTextResroucePanel;

	//Expedition Stuff
	//Expedition Modes
	public bool arrangingExpedition = false;
	public bool finalizingExpediton = false;
	public bool expeditionEnabled = false;

	//People toggles and texts
	public Toggle expPerson1;
	public Toggle expPerson2;
	public Toggle expPerson3;
	public Toggle expPerson4;
	public Text expPerson1Text;
	public Text expPerson2Text;
	public Text expPerson3Text;
	public Text expPerson4Text;

	//Water stores for expedition handler
	public InputField expeditionWaterInput;
	private bool waterError = true;
	public GameObject waterErrorText;
	private int waterValToTake;

	//food stores for expedition handlers
	public Toggle[] foodToggles; //array of toggles for displaying food
	public Text[] foodToggleTexts;

	public Text expeditionFoodTextResourcePanel;
	public Text expeditionWaterTextResourcePanel;

	public Text expeditionPeoplePrint;

	//expedition panel stuff
	public GameObject expeditionPanel;
	public Button expMoveButton;
	public Button expEnterBaseButton;
	public Text expInfoText;
	//wood values
	public Button expChopButton;
	public bool genRandWoodTurns = false;
	//stone values
	public Button expMineButton;
	public bool genRandStoneTurns = false;
	//food values
	public Button expGatherButton;

	// Use this for initialization
	void Start () {
		Instance = this;
		//City Panel UI Stuff
		peopleTexts = new Text[7];
		peopleHeads = new Image[7];
		for (int i = 1; i <= 7; i++) {//finding heads and text values for the head array
			peopleTexts [i - 1] = GameObject.Find ("p" + i + "Text").GetComponent<Text> ();
			peopleHeads [i - 1] = GameObject.Find ("p" + i + "Head").GetComponent<Image> ();
		}

		checkPeopleBase();
		checkFoodBase();
		checkWaterBase();

		peoplePanel.SetActive (false);
		needsPanel.SetActive (false);


		//Expedition Panel UI stuff
		foodToggles = new Toggle[10];
		foodToggleTexts = new Text[10];
		for (int i = 1; i <= 10; i++) {//finding the food toggles for the expedition
			foodToggles [i - 1] = GameObject.Find ("expFood" + i).GetComponent<Toggle> ();
			foodToggleTexts [i - 1] = GameObject.Find ("expFoodText" + i).GetComponent<Text> ();
		}
		expeditionPanelBase.SetActive (false);
		expeditionPanel.SetActive (false);
		expChopButton.interactable = false;
		expMineButton.interactable = false;
		expGatherButton.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		checkFoodBase ();
		checkWaterBase ();
		checkPeopleBase ();
		checkResourcesBase ();

		if (expeditionEnabled == true) {
			checkExpeditionButtons ();
		}

		if (arrangingExpedition == true) {
			if (waterError == true) {//setting error text to pop up
				waterErrorText.SetActive (true);
			} else if (waterError == false) {
				waterErrorText.SetActive (false);
			}

			//Setting the people of the expedition panel
			expPerson1.gameObject.SetActive (true);
			expPerson2.gameObject.SetActive (true);
			expPerson3.gameObject.SetActive (true);
			expPerson4.gameObject.SetActive (true);

			if (GameManager.Instance.playerPeople [0] == "") {
				expPerson1.gameObject.SetActive (false);
			} 
			if (GameManager.Instance.playerPeople [1] == "") {
				expPerson2.gameObject.SetActive (false);
			} 
			if (GameManager.Instance.playerPeople [2] == "") {
				expPerson3.gameObject.SetActive (false);
			}
			if (GameManager.Instance.playerPeople [3] == "") {
				expPerson4.gameObject.SetActive (false);
			} 

			expPerson1Text.text = GameManager.Instance.playerPeople [0];
			expPerson2Text.text = GameManager.Instance.playerPeople [1];
			expPerson3Text.text = GameManager.Instance.playerPeople [2];
			expPerson4Text.text = GameManager.Instance.playerPeople [3];

			//Setting the food of the expedition panel
			foodToggles[0].gameObject.SetActive(true);
			foodToggles[1].gameObject.SetActive(true);
			foodToggles[2].gameObject.SetActive(true);
			foodToggles[3].gameObject.SetActive(true);
			foodToggles[4].gameObject.SetActive(true);
			foodToggles[5].gameObject.SetActive(true);
			foodToggles[6].gameObject.SetActive(true);
			foodToggles[7].gameObject.SetActive(true);
			foodToggles[8].gameObject.SetActive(true);
			foodToggles[9].gameObject.SetActive(true);

			if (GameManager.Instance.storedFood [0].foodType == null) {
				foodToggles [0].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [1].foodType == null) {
				foodToggles [1].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [2].foodType == null) {
				foodToggles [2].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [3].foodType == null) {
				foodToggles [3].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [4].foodType == null) {
				foodToggles [4].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [5].foodType == null) {
				foodToggles [5].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [6].foodType == null) {
				foodToggles [6].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [7].foodType == null) {
				foodToggles [7].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [8].foodType == null) {
				foodToggles [8].gameObject.SetActive (false);
			}
			if (GameManager.Instance.storedFood [9].foodType == null) {
				foodToggles [9].gameObject.SetActive (false);
			}

			foodToggleTexts [0].text = GameManager.Instance.storedFood [0].foodType + ", Value: " + GameManager.Instance.storedFood [0].foodVal;
			foodToggleTexts [1].text = GameManager.Instance.storedFood [1].foodType + ", Value: " + GameManager.Instance.storedFood [1].foodVal;
			foodToggleTexts [2].text = GameManager.Instance.storedFood [2].foodType + ", Value: " + GameManager.Instance.storedFood [2].foodVal;
			foodToggleTexts [3].text = GameManager.Instance.storedFood [3].foodType + ", Value: " + GameManager.Instance.storedFood [3].foodVal;
			foodToggleTexts [4].text = GameManager.Instance.storedFood [4].foodType + ", Value: " + GameManager.Instance.storedFood [4].foodVal;
			foodToggleTexts [5].text = GameManager.Instance.storedFood [5].foodType + ", Value: " + GameManager.Instance.storedFood [5].foodVal;
			foodToggleTexts [6].text = GameManager.Instance.storedFood [6].foodType + ", Value: " + GameManager.Instance.storedFood [6].foodVal;
			foodToggleTexts [7].text = GameManager.Instance.storedFood [7].foodType + ", Value: " + GameManager.Instance.storedFood [7].foodVal;
			foodToggleTexts [8].text = GameManager.Instance.storedFood [8].foodType + ", Value: " + GameManager.Instance.storedFood [8].foodVal;
			foodToggleTexts [9].text = GameManager.Instance.storedFood [9].foodType + ", Value: " + GameManager.Instance.storedFood [9].foodVal;

			if (finalizingExpediton == true) {//finalizing expedition, moving selected food and other things from the base to the expedition
				expeditionHandler expParty = ((GameObject)Instantiate (expeditonPrefab, GameObject.Find("homeBase").transform.position, GameObject.Find("homeBase").transform.rotation)).GetComponent<expeditionHandler> ();//instanciating the expidition
				if (expPerson1.isOn == true) {//if the person toggle is turned on, move them to the expedition
					expParty.expeditionPeople [0] = GameManager.Instance.playerPeople [0];
					expParty.expeditionPeopleNum++;
					GameManager.Instance.playerPeople [0] = "";
				}
				if (expPerson2.isOn == true) {
					expParty.expeditionPeople[1] = GameManager.Instance.playerPeople [1];
					expParty.expeditionPeopleNum++;
					GameManager.Instance.playerPeople [1] = "";
				}
				if (expPerson3.isOn == true) {
					expParty.expeditionPeople[2] = GameManager.Instance.playerPeople [2];
					expParty.expeditionPeopleNum++;
					GameManager.Instance.playerPeople [2] = "";
				}
				if (expPerson4.isOn == true) {
					expParty.expeditionPeople[3] = GameManager.Instance.playerPeople [3];
					expParty.expeditionPeopleNum++;
					GameManager.Instance.playerPeople [3] = "";
				}

				if (foodToggles [0].isOn == true) {//if the food toggle is on, move it to the expedition
					expParty.storedFood [0].foodType = GameManager.Instance.storedFood [0].foodType;
					expParty.storedFood [0].foodVal = GameManager.Instance.storedFood [0].foodVal;
					GameManager.Instance.storedFood [0].foodType = null;
					GameManager.Instance.storedFood [0].foodVal = 0;
				}
				if (foodToggles [1].isOn == true) {
					expParty.storedFood [1].foodType = GameManager.Instance.storedFood [1].foodType;
					expParty.storedFood [1].foodVal = GameManager.Instance.storedFood [1].foodVal;
					GameManager.Instance.storedFood [1].foodType = null;
					GameManager.Instance.storedFood [1].foodVal = 0;
				}
				if (foodToggles [2].isOn == true) {
					expParty.storedFood [2].foodType = GameManager.Instance.storedFood [2].foodType;
					expParty.storedFood [2].foodVal = GameManager.Instance.storedFood [2].foodVal;
					GameManager.Instance.storedFood [2].foodType = null;
					GameManager.Instance.storedFood [2].foodVal = 0;
				}
				if (foodToggles [3].isOn == true) {
					expParty.storedFood [3].foodType = GameManager.Instance.storedFood [3].foodType;
					expParty.storedFood [3].foodVal = GameManager.Instance.storedFood [3].foodVal;
					GameManager.Instance.storedFood [3].foodType = null;
					GameManager.Instance.storedFood [3].foodVal = 0;
				}
				if (foodToggles [4].isOn == true) {
					expParty.storedFood [4].foodType = GameManager.Instance.storedFood [4].foodType;
					expParty.storedFood [4].foodVal = GameManager.Instance.storedFood [4].foodVal;
					GameManager.Instance.storedFood [4].foodType = null;
					GameManager.Instance.storedFood [4].foodVal = 0;
				}
				if (foodToggles [5].isOn == true) {
					expParty.storedFood [5].foodType = GameManager.Instance.storedFood [5].foodType;
					expParty.storedFood [5].foodVal = GameManager.Instance.storedFood [5].foodVal;
					GameManager.Instance.storedFood [5].foodType = null;
					GameManager.Instance.storedFood [5].foodVal = 0;
				}
				if (foodToggles [6].isOn == true) {
					expParty.storedFood [6].foodType = GameManager.Instance.storedFood [6].foodType;
					expParty.storedFood [6].foodVal = GameManager.Instance.storedFood [6].foodVal;
					GameManager.Instance.storedFood [6].foodType = null;
					GameManager.Instance.storedFood [6].foodVal = 0;
				}
				if (foodToggles [7].isOn == true) {
					expParty.storedFood [7].foodType = GameManager.Instance.storedFood [7].foodType;
					expParty.storedFood [7].foodVal = GameManager.Instance.storedFood [7].foodVal;
					GameManager.Instance.storedFood [7].foodType = null;
					GameManager.Instance.storedFood [7].foodVal = 0;
				}
				if (foodToggles [8].isOn == true) {
					expParty.storedFood [8].foodType = GameManager.Instance.storedFood [8].foodType;
					expParty.storedFood [8].foodVal = GameManager.Instance.storedFood [8].foodVal;
					GameManager.Instance.storedFood [8].foodType = null;
					GameManager.Instance.storedFood [8].foodVal = 0;
				}
				if (foodToggles [9].isOn == true) {
					expParty.storedFood [9].foodType = GameManager.Instance.storedFood [9].foodType;
					expParty.storedFood [9].foodVal = GameManager.Instance.storedFood [9].foodVal;
					GameManager.Instance.storedFood [9].foodType = null;
					GameManager.Instance.storedFood [9].foodVal = 0;
				}
				finalizingExpediton = false;

				GameManager.Instance.storedFood = trimArray (GameManager.Instance.storedFood);

				//Reseting input fields in the arranging expedition tab
				expeditionWaterInput.text = null;
				expPerson1.isOn = false;
				expPerson2.isOn = false;
				expPerson3.isOn = false;
				expPerson4.isOn = false;

				for (int i = 0; i < foodToggles.Length - 1; i++) {
					foodToggles [i].isOn = false;
				}

				expeditionPanelBase.SetActive (false);
				expParty.ExpeditionWaterStore = waterValToTake;
				GameManager.Instance.waterStore -= waterValToTake;
				GameManager.Instance.playerPeopleNum -= expParty.expeditionPeopleNum;
				expeditionEnabled = true;
				waterError = true;
				expInfoText.enabled = false;
				arrangingExpedition = false;
			}
		}
	}

	public void LockInput(InputField input) { //ending water input
		waterValToTake = int.Parse (expeditionWaterInput.text);
		Debug.Log (waterValToTake);
		if (waterValToTake > GameManager.Instance.waterStore) {
			waterError = true;
		}
		if (waterValToTake < GameManager.Instance.waterStore) {
			waterError = false;
		}
	}

	//Checking the base values
	public void checkPeopleBase() {
		for (int i = 0; i <= 6; i++) {
			peopleTexts [i].text = GameManager.Instance.playerPeople [i];
			if (GameManager.Instance.playerPeople [i] != "") {
				peopleHeads [i].enabled = true;
			} else {
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

	void checkResourcesBase() {
		int totalWoodVal = GameManager.Instance.woodStored;
		woodTextResourcePanel.text = "" + totalWoodVal;

		int totalStoneVal = GameManager.Instance.stoneStored;
		stoneTextResroucePanel.text = "" + totalStoneVal;
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
	public void checkPeopleExpedition() {
		string output = "";
		for (int i = 0; i <= 3; i++) {
			output += expeditionHandler.Instance.expeditionPeople [i] + "\n";
		}
		expeditionPeoplePrint.text = output;
	}

	public void checkFoodExpedition() {
		int totalExpFoodVal = 0;
		for (int i = 0; i <= expeditionHandler.Instance.storedFood.Length-1; i++) {
			//Debug.Log (i + ": " + expeditionHandler.Instance.storedFood [i].foodType + ", " + expeditionHandler.Instance.storedFood [i].foodVal);
			totalExpFoodVal += expeditionHandler.Instance.storedFood [i].foodVal;
		}
		expeditionFoodTextResourcePanel.text = "" + totalExpFoodVal;
	}

	public void checkWaterExpedition() {
		int totalWaterVal = expeditionHandler.Instance.ExpeditionWaterStore;
		expeditionWaterTextResourcePanel.text = "" +  totalWaterVal;
	}

	public void SetExpeditionMoveMode() { 
		expeditionHandler.Instance.isMovingMode = !expeditionHandler.Instance.isMovingMode;

		expInfoText.text = "In moving mode!";
		if (expeditionHandler.Instance.isMovingMode == true) {
			expInfoText.enabled = true;
		} else {
			expInfoText.enabled = false;
		}
	}

	public void SetExpeditionEnterBaseMode() {
		if (expeditionHandler.Instance.isAtHome) {
			expeditionHandler.Instance.isEnterBaseMode = true;
		}
	}

	public void SetExpeditionChopWoodMode() {//chopping down wood
		if (expeditionHandler.Instance.isAtTrees) {
			if (genRandWoodTurns == false) {//generating the random amount of turns it will take to chop down the tile
				if (expeditionHandler.Instance.currentTileType.Contains("Light")) {
					int rand = Random.Range (1, 3);
					expeditionHandler.Instance.treeChopTurns = rand;
					genRandWoodTurns = true;
				} else if (expeditionHandler.Instance.currentTileType.Contains("Heavy")) {
					int rand = Random.Range (2, 5);
					expeditionHandler.Instance.treeChopTurns = rand;
					genRandWoodTurns = true;
				}
			}

			expeditionHandler.Instance.isChoppingMode = !expeditionHandler.Instance.isChoppingMode;
			expInfoText.text = "Chopping down trees. \nFinished in " + expeditionHandler.Instance.treeChopTurns + " turns.";//setting exp panel text
			if (expeditionHandler.Instance.isChoppingMode == true) {
				expInfoText.enabled = true;
				genRandWoodTurns = false;
			} else {
				expInfoText.enabled = false;
				genRandWoodTurns = false;
			}
		}
	}

	public void SetExpeditionMineStoneMode() {//mining stone
		if (expeditionHandler.Instance.isAtStone) {
			if (genRandStoneTurns == false) {//generating the random amount of turns it will take to mine the tile
				if (expeditionHandler.Instance.currentTileType.Contains("Light")) {
					int rand = Random.Range (1, 3);
					expeditionHandler.Instance.stoneMineTurns = rand;
					genRandStoneTurns = true;
				} else if (expeditionHandler.Instance.currentTileType.Contains("Heavy")) {
					int rand = Random.Range (2, 5);
					expeditionHandler.Instance.stoneMineTurns = rand;
					genRandStoneTurns = true;
				}
			}

			expeditionHandler.Instance.isMiningMode = !expeditionHandler.Instance.isMiningMode;
			expInfoText.text = "Mining stone. \nFinished in " + expeditionHandler.Instance.stoneMineTurns + " turns.";//setting exp panel text
			if (expeditionHandler.Instance.isMiningMode == true) {
				expInfoText.enabled = true;
				genRandStoneTurns = false;
			} else {
				expInfoText.enabled = false;
				genRandStoneTurns = false;
			}
		}
	}

	public void SetExpeditionGatherFoodMode() {
		if (expeditionHandler.Instance.isAtFood) {
			expeditionHandler.Instance.isFoodMode = !expeditionHandler.Instance.isFoodMode;
			expeditionHandler.Instance.foodGatherTurns = 1;
			expInfoText.text = "Scavenging for Food. \nFinished in " + expeditionHandler.Instance.foodGatherTurns + " turns.";//setting exp panel text
			if (expeditionHandler.Instance.isFoodMode == true) {
				expInfoText.enabled = true;

			} else {
				expInfoText.enabled = false;

			}
		}
	}

	public void checkExpeditionButtons() {
		if (expeditionHandler.Instance.isChoppingMode == true) {
			expMoveButton.interactable = false;
			expMineButton.interactable = false;
			expGatherButton.interactable = false;
		} else {
			if (expeditionHandler.Instance.hasMoved == false) {
				expMoveButton.interactable = true;
			}
		}
		if (expeditionHandler.Instance.isMiningMode == true) {
			expMoveButton.interactable = false;
			expChopButton.interactable = false;
			expGatherButton.interactable = false;
		} else { 
			if (expeditionHandler.Instance.hasMoved == false) {
				expMoveButton.interactable = true;
			}
		}
		if (expeditionHandler.Instance.isFoodMode == true) {
			expMoveButton.interactable = false;
			expChopButton.interactable = false;
			expMineButton.interactable = false;
		} else { 
			if (expeditionHandler.Instance.hasMoved == false) {
				expMoveButton.interactable = true;
			}
		}

		if (expeditionHandler.Instance.isAtTrees == true) {
			expChopButton.interactable = true;
		} else {
			expChopButton.interactable = false;
		}
		if (expeditionHandler.Instance.isAtStone == true) {
			expMineButton.interactable = true;
		} else {
			expMineButton.interactable = false;
		}
		if (expeditionHandler.Instance.isAtFood == true) {
			expGatherButton.interactable = true;
		} else {
			expGatherButton.interactable = false;
		}
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

	GameManager.FoodData[] trimArray(GameManager.FoodData[] array) {//getting rid of the null elements in an array
		int j = 0;
		GameManager.FoodData[] tempArray = new GameManager.FoodData[32];//creating temp array
		for (int i = 0; i <= array.Length -1 ; i++) {//cycling through, and only transferring over the non null elements
			if (array [i].foodType == null) {
				continue;
			}
			tempArray [j] = array [i];
			j++;
		} 

		return tempArray;
	}
}
