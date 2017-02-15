using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class expeditionHandler : MonoBehaviour {

	public static expeditionHandler Instance;

	public GameObject colliderGen;//collider prefab for sight
	private GameObject objToDelete;

	public GameObject expOutline;

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

	public ExpeditionFoodData[] storedFood = new ExpeditionFoodData[9];
	public ExpeditionToolType[] storedTools;

	public int ExpeditionWaterStore;

	//values for rentering base
	public bool reEnteringBase = false;
	public bool isEnterBaseMode = false;

	// Use this for initialization
	void Start () {
		Instance = this;
		expLocation = baseHandler.Instance.baseLocation;
		storedTools = new ExpeditionToolType[expeditionPeopleNum];
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
					}
				}
			}
		}
	
		if (isAtHome == true) {
			UIManager.Instance.expEnterBaseButton.interactable = true;
		} else {
			if (expLocation != baseHandler.Instance.baseLocation) {
				UIManager.Instance.expEnterBaseButton.interactable = false;
			}
		}

		if (isEnterBaseMode == true) {
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
		
			for (int i = 0; i <= 3; i++) {
				if (expeditionPeople [i] != "") {
					GameManager.Instance.playerPeople [i] = expeditionPeople [i];
					GameManager.Instance.playerPeopleNum++;
				}
			}

			UIManager.Instance.expeditionEnabled = false;
			UIManager.Instance.expeditionPanel.SetActive (false);
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


			ExpeditionWaterStore -= expeditionPeopleNum * 4; //subtracting needed water
			hasMoved = false;
			UIManager.Instance.expMoveButton.interactable = true;
			UIManager.Instance.checkFoodExpedition ();
			UIManager.Instance.checkWaterExpedition ();

			NewTurn = false; //ending the turn swap
		}
	}

	void OnMouseDown() {
		if (isMovingMode == false) {
			if (isSelectedMode == false) {
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

	void OnTriggerStay2D (Collider2D col) {
		if (col.gameObject.tag == "HomeTile") {
			isAtHome = true;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.gameObject.tag == "HomeTile") {
			isAtHome = false;
		}
	}
}
