using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class expeditionHandler : MonoBehaviour {

	public static expeditionHandler Instance;

	private GameObject objToDelete;

	public GameObject expOutline;

	//the current tile the expedition is located at
	public GameObject expLocationTile;
	public string currentTileType;

	public Vector3 expLocation; //curent location

	public bool NewTurn;

	public bool isSelectedMode = false; //if expedition is selected
	public bool isMovingMode = false; //if expedition is in moving mode
	public bool hasMoved = false; //if the expedition has moved this turn
	public bool isAtHome = false; //if the expedtion is at the home tile

	public string[] expeditionPeople = new string[4];//names of people in expedition
	public int expeditionPeopleNum; //number of people in expediton

	public bool isMoving = false;//if the expedition is currently moving
	public Vector3 targetPos;//move destination

	public struct ExpeditionToolType { //storing different Tools and thier efficencies
		public bool isAxe;
		public bool isPick;
		public float efficency;
	}

	public struct ExpeditionFoodData { //storing different food types and vals associated
		public string foodType;
		public int foodVal;
	}
	private ExpeditionFoodData temp; //temp val for swapping food variables

	public ExpeditionFoodData[] storedFood = new ExpeditionFoodData[32];
	public ExpeditionToolType[] storedTools;

	public int storedWood;
	public int storedStone;

	public int ExpeditionWaterStore;

	//values for rentering base
	public bool reEnteringBase = false;
	public bool isEnterBaseMode = false;

	//values for gathering wood
	public bool isAtTrees = false;
	public bool isChoppingMode = false;
	public int treeChopTurns;

	//values for gathering stone
	public bool isAtStone = false;
	public bool isMiningMode = false;
	public int stoneMineTurns;

	//values for gaterhing food
	public bool isAtFood = false;
	public bool isFoodMode = false;
	public int foodGatherTurns = 1;

	//values for gathering water
	public bool isAtWater = false;
	public bool isWaterMode = false;
	public int waterGatherTurns = 1;

	// Use this for initialization
	void Start () {
		Instance = this;
		expLocation = baseHandler.Instance.baseLocation;
		storedTools = new ExpeditionToolType[expeditionPeopleNum];

		expLocationTile = GameObject.Find ("homeBase");
	}
	
	// Update is called once per frame
	void Update () {
		if (isSelectedMode == true) {
			if (isMovingMode == true) {
				if (isMoving == true) {
					transform.position = Vector3.MoveTowards (transform.position, targetPos, 4f * Time.deltaTime);
					if (transform.position == targetPos) {
						//GameObject ColliderGen = ((GameObject)Instantiate (colliderGen, transform.position, Quaternion.Euler (new Vector3 ())));
						isMoving = false;
						isMovingMode = false;
						//objToDelete = ColliderGen;
						//StartCoroutine ("destroyThing");
						hasMoved = true;
						UIManager.Instance.expMoveButton.interactable = false;
						UIManager.Instance.expInfoText.enabled = false;
					}
				}
			}
		}
	
		if (isAtHome == true) {//if in range of the base, allows the renter base button to be clicked
			UIManager.Instance.expEnterBaseButton.interactable = true;
		} else {
			UIManager.Instance.expEnterBaseButton.interactable = false;
		}

		//checking to see if thecurrent tile has forest
		if (expLocationTile.name == "baseTile(Clone)") {
			currentTileType = expLocationTile.GetComponent<tileHandler> ().tileType;
		} else if (expLocationTile.name == "homeBase") {
			currentTileType = "Home Base";
		}
		switch (currentTileType) {
		case "Home Base":
			isAtHome = true;
			isAtTrees = false;
			isAtStone = false;
			isAtFood = false;
			break;
		case "Grassland":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = true;
			break;
		case "Heavy Forest":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = true;
			break;
		case "Light Forest":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = true;
			break;
		case "Heavy Forest Snow":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = false;
			break;
		case "Light Forest Snow":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = false;
			break;
		case "Heavy Forest Stone":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = false;
			break;
		case "Light Forest Stone":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = false;
			break;
		case "Light Forest Dirt":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = false;
			break;
		case "Heavy Forest Dirt":
			isAtHome = false;
			isAtTrees = true;
			isAtStone = false;
			isAtFood = false;
			break;
		case "Light Rocks":
			isAtHome = false;
			isAtTrees = false;
			isAtStone = true;
			isAtFood = false;
			break;
		case "Heavy Rocks":
			isAtHome = false;
			isAtTrees = false;
			isAtStone = true;
			isAtFood = false;
			break;
		case "Light Rocks Sand":
			isAtHome = false;
			isAtTrees = false;
			isAtStone = true;
			isAtFood = false;
			break;
		case "Light Rocks Dirt":
			isAtHome = false;
			isAtTrees = false;
			isAtStone = true;
			isAtFood = false;
			break;
		case "Light Rocks Snow":
			isAtHome = false;
			isAtTrees = false;
			isAtStone = true;
			isAtFood = false;
			break;
		case "Heavy Rocks Snow":
			isAtHome = false;
			isAtTrees = false;
			isAtStone = true;
			isAtFood = false;
			break;
		default:
			isAtTrees = false;
			isAtHome = false;
			isAtStone = false;
			isAtFood = false;
			break;
		}

		//setting if the expedition is at water
		if (currentTileType != "Home Base") {
			if (expLocationTile.GetComponent<tileHandler> ().isNearWater == true) {
				isAtWater = true;
			} else {
				isAtWater = false;
			}
		}

		if (isEnterBaseMode == true) {//entering the base, offloading food and people values
			for (int i = 0; i < storedFood.Length - 1; i++) {
				for (int j = 0; j < GameManager.Instance.storedFood.Length - 1; j++) {
					if (GameManager.Instance.storedFood [j].foodType == null) {
						GameManager.Instance.storedFood [j].foodType = storedFood [i].foodType;
						GameManager.Instance.storedFood [j].foodVal = storedFood [i].foodVal;
						storedFood[i].foodType = null;
						storedFood [i].foodVal = 0;
						j = 64;
					}
				}
			}
			GameManager.Instance.waterStore += ExpeditionWaterStore;
			GameManager.Instance.woodStored += storedWood;
			GameManager.Instance.stoneStored += storedStone;
		
			for (int i = 0; i <= 3; i++) {//putting the people back into the base
				if (expeditionPeople [i] != "") {
					GameManager.Instance.playerPeople [i] = expeditionPeople [i];
					GameManager.Instance.playerPeopleNum++;
				}
			}
			//disabling references to the expedtition and deleting
			UIManager.Instance.expeditionEnabled = false;
			UIManager.Instance.expeditionPanel.SetActive (false);

			baseHandler.Instance.cityUIScreen.SetActive (true);//toggling on the base ui panel
			baseHandler.Instance.toggleCityUI = true;

			Destroy (this.gameObject);
		}

		if (NewTurn == true) { //moving to the next turn
			int neededFood = expeditionPeopleNum * 2; //getting amount of needed food
			while (neededFood > 0) {
				if (storedFood [0].foodVal > neededFood) { //if the current food index foodval is greater than the need
					if (storedFood [0].foodType == "Bread") {// if it is bread it gets swaped to half bread
						storedFood [0].foodType = "1/2 Bread";
						storedFood [0].foodVal = 2;
						neededFood -= 2;
					} else if (storedFood [0].foodType == "Boar Meat") {//if it is bread it gets swaped to half boar meat
						storedFood [0].foodType = "1/2 Boar Meat";
						storedFood [0].foodVal = 2;
						neededFood -= 2;
					}
				} else if (storedFood [0].foodVal <= neededFood) {
					int tempVal;
					for (int i = 0; i <= storedFood.Length-1; i++) {//if the amount of food at the current index is less than or equal to needed food, find the last food item and move it ot the top of the array
						if (storedFood [i].foodType != null) {
							temp = storedFood [i];
							tempVal = i;
						} if (storedFood [i].foodType == null) {
							i = storedFood.Length;
						}
					}
					storedFood [0] = temp;
					storedFood [tempVal].foodType = null;
					storedFood [tempVal].foodVal = 0;
					neededFood -= 2;
				}
			}

			if (isChoppingMode == true) {
				treeChopTurns--;
				UIManager.Instance.expInfoText.text = "Chopping down trees. \nFinished in " + treeChopTurns + " turns.";//setting exp panel text
				if (treeChopTurns <= 0) {//if the tree tile is chopped all the way down
					if (currentTileType.Contains("Light")) {
						int random = Random.Range (25, 60);
						storedWood += random;
						UIManager.Instance.genRandWoodTurns = false;
						UIManager.Instance.expInfoText.enabled = false;
						isChoppingMode = false;

						if (currentTileType.Contains ("Snow")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Snow";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassSnowTile;
						} else if (currentTileType.Contains ("Stone")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Stone";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultStone;
						} else if (currentTileType.Contains ("Dirt")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Dirt";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultDirt;
						} else {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Grassland";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassTile;
						}
					} else if (currentTileType.Contains("Heavy")) {
						int random = Random.Range (50, 100);
						storedWood += random;
						UIManager.Instance.genRandWoodTurns = false;
						UIManager.Instance.expInfoText.enabled = false;
						isChoppingMode = false;

						if (currentTileType.Contains ("Snow")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Snow";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassSnowTile;
						} else if (currentTileType.Contains ("Stone")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Stone";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultStone;
						} else if (currentTileType.Contains ("Dirt")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Dirt";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultDirt;
						} else {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Grassland";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassTile;
						}
					}
				}
			}

			if (isMiningMode== true) {
				stoneMineTurns--;
				UIManager.Instance.expInfoText.text = "Mining stone. \nFinished in " + stoneMineTurns + " turns.";//setting exp panel text
				if (stoneMineTurns <= 0) {//if the tree tile is chopped all the way down
					if (currentTileType.Contains("Light")) {
						int random = Random.Range (15, 30);
						storedStone += random;
						UIManager.Instance.genRandStoneTurns = false;
						UIManager.Instance.expInfoText.enabled = false;
						isMiningMode = false;

						if (currentTileType.Contains ("Snow")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Snow";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassSnowTile;
						} else if (currentTileType.Contains ("Stone")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Stone";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultStone;
						} else if (currentTileType.Contains ("Dirt")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Dirt";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultDirt;
						} else if (currentTileType.Contains ("Sand")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Sand";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultSand;
						} else {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Grassland";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassTile;
						}
					} else if (currentTileType.Contains("Heavy")) {
						int random = Random.Range (25, 50);
						storedStone += random;
						UIManager.Instance.genRandStoneTurns = false;
						UIManager.Instance.expInfoText.enabled = false;
						isMiningMode = false;

						if (currentTileType.Contains ("Snow")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Snow";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassSnowTile;
						} else if (currentTileType.Contains ("Stone")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Stone";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultStone;
						} else if (currentTileType.Contains ("Dirt")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Dirt";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultDirt;
						} else if (currentTileType.Contains ("Sand")) {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Sand";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultSand;
						} else {
							expLocationTile.GetComponent<tileHandler> ().tileType = "Grassland";
							expLocationTile.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassTile;
						}
					}
				}
			}

			if (isFoodMode == true) {
				foodGatherTurns--;
				UIManager.Instance.expInfoText.text = "Scavenging for Food. \nFinished in " + foodGatherTurns + " turns.";//setting exp panel text

				if (foodGatherTurns <= 0) {//selecting a random food to add to the expedition
					int random = Random.Range (0, 100);

					isFoodMode = false;
					if (random <= 25) {//finding small amount of berries (4 total food score, seperated too 2 items)
						int numToAdd = 2;
						for (int i = 0; i <= storedFood.Length - 1; i++) {
							if (numToAdd >= 0) {
								if (storedFood [i].foodVal == 0) {
									storedFood [i].foodType = "Berries";
									storedFood [i].foodVal = 2;
									numToAdd--;
								}
							}
						}
						UIManager.Instance.expInfoText.text = "Scavenging finished. \nFound a small amount of berries.";//setting exp panel text
					} else if (random >= 25 && random <= 50) {//finding large amount of berries (8 total food score, seperated to 4 items)
						int numToAdd = 4;
						for (int i = 0; i <= storedFood.Length - 1; i++) {
							if (numToAdd >= 0) {
								if (storedFood [i].foodVal == 0) {
									storedFood [i].foodType = "Berries";
									storedFood [i].foodVal = 2;
									numToAdd--;
								}
							}
						}
						UIManager.Instance.expInfoText.text = "Scavenging finished. \nFound a large amount of berries.";//setting exp panel text
					} else if (random >= 50 && random <= 75) {//finding small amount of small game (6 total food score, seperated to 2 items worth 2 and 4)
						int numToAdd = 2;
						for (int i = 0; i >= storedFood.Length - 1; i++) {
							if (numToAdd == 1) {
								if (storedFood [i].foodVal == 0) {
									storedFood [i].foodType = "Squirrel Meat";
									storedFood [i].foodVal = 2;
									numToAdd--;
								}
							} else if (numToAdd == 2) {
								if (storedFood [i].foodVal == 0) {
									storedFood [i].foodType = "Boar Meat";
									storedFood [i].foodVal = 4;
									numToAdd--;
								}
							}
						}
						UIManager.Instance.expInfoText.text = "Scavenging finished. \nFound and hunted some small game.";//setting exp panel text
					} else if (random >= 75 && random <= 100) {//finding larg amount of small game (12 total food score, seperated into 4 items, 2 worth 2, 2 worth 4)
						int numToAdd = 4;
						for (int i = 0; i <= storedFood.Length - 1; i++) {
							if (numToAdd == 1) {
								if (storedFood [i].foodVal == 0) {
									storedFood [i].foodType = "Squirrel Meat";
									storedFood [i].foodVal = 2;
									numToAdd--;
								}
							} else if (numToAdd == 2) {
								if (storedFood [i].foodVal == 0) {
									storedFood [i].foodType = "Squirrel Meat";
									storedFood [i].foodVal = 2;
									numToAdd--;
								}
							} else if (numToAdd == 3) {
								if (storedFood [i].foodVal == 0) {
									storedFood [i].foodType = "Boar Meat";
									storedFood [i].foodVal = 4;
									numToAdd--;
								}
							} else if (numToAdd == 4) {
								if (storedFood [i].foodVal == 0) {
									storedFood [i].foodType = "Boar Meat";
									storedFood [i].foodVal = 4;
									numToAdd--;
								}
							}
						}
						UIManager.Instance.expInfoText.text = "Scavenging finished. \nFound plentiful small game!";//setting exp panel text
					}
					isFoodMode = false;
				}
			}

			if (isWaterMode == true) {
				waterGatherTurns--;
				UIManager.Instance.expInfoText.text = "Gathering Water. \nFinished in " + waterGatherTurns + " turns.";//setting exp panel text

				if (waterGatherTurns <= 0) {
					int random = Random.Range (1, 4);

					switch (random) {
					case(1):
						ExpeditionWaterStore += 6;
						UIManager.Instance.expInfoText.text = "Gathering finished. \nFound a small amount of water.";//setting exp panel text
						break;
					case(2):
						ExpeditionWaterStore += 8;
						UIManager.Instance.expInfoText.text = "Gathering finished. \nFound a decent amount of water.";//setting exp panel text
						break;
					case(3):
						ExpeditionWaterStore += 12;
						UIManager.Instance.expInfoText.text = "Gathering finished. \nFound a large amount of water.";//setting exp panel text
						break;
					case(4):
						ExpeditionWaterStore += 16;
						UIManager.Instance.expInfoText.text = "Gathering finished. \nFound a huge amount of water!";//setting exp panel text
						break;
					}

					isWaterMode = false;
				}
			}

			ExpeditionWaterStore -= expeditionPeopleNum * 4; //subtracting needed water
			hasMoved = false;//allowing movement next turn
			UIManager.Instance.expMoveButton.interactable = true;
			UIManager.Instance.checkFoodExpedition ();//checking food and water for the expedition
			UIManager.Instance.checkWaterExpedition ();

			NewTurn = false; //ending the turn swap
		}
	}

	void OnMouseDown() {
		if (isMovingMode == false) {
			if (isSelectedMode == false) {//toggling on and off the expedition panel values
				baseHandler.Instance.toggleCityUI = false;
				isSelectedMode = true;
				expOutline.SetActive (true);
				UIManager.Instance.expeditionPanel.SetActive (true);
				baseHandler.Instance.cityUIScreen.SetActive (false);
				baseHandler.Instance.toggleCityUI = false;
				UIManager.Instance.checkFoodExpedition ();
				UIManager.Instance.checkWaterExpedition ();
				UIManager.Instance.checkPeopleExpedition ();
			} else if (isSelectedMode == true) {
				isSelectedMode = false;
				expOutline.SetActive (false);
				UIManager.Instance.expeditionPanel.SetActive (false);
				baseHandler.Instance.cityUIScreen.SetActive (false);
				UIManager.Instance.checkFoodExpedition ();
				UIManager.Instance.checkWaterExpedition ();
				UIManager.Instance.checkPeopleExpedition ();
			}
		}
	}

	IEnumerator destroyThing() {
		yield return new WaitForSeconds (1.0f);
		Destroy (objToDelete);
	}

	/*
	void OnTriggerStay2D (Collider2D col) {
		if (col.gameObject.tag == "HomeTile") {
			isAtHome = true;
		} 
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.gameObject.tag == "HomeTile") {
			isAtHome = false;
		}
	}*/
}
