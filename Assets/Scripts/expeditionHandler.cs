using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expeditionHandler : MonoBehaviour {

	public static expeditionHandler Instance;

	public GameObject expOutline;

	public bool NewTurn;

	public bool isSelectedMode = false;

	public string[] expeditionPeople;//names of people in expedition
	public int expeditionPeopleNum;
	private int foodStorageCapacity;


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

	public ExpeditionFoodData[] storedFood;
	public ExpeditionToolType[] storedTools;

	public int ExpeditionWaterStore;

	// Use this for initialization
	void Start () {
		Instance = this;
		foodStorageCapacity = expeditionPeopleNum * 10;
		storedFood = new ExpeditionFoodData[foodStorageCapacity];
		storedTools = new ExpeditionToolType[expeditionPeopleNum];
	}
	
	// Update is called once per frame
	void Update () {
		if (isSelectedMode == true) {
			
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

			NewTurn = false; //ending the turn swap
		}
	}

	void OnMouseDown() {
		if (isSelectedMode == false) {
			isSelectedMode = true;
			expOutline.SetActive (true);
		} else if (isSelectedMode == true) {
			isSelectedMode = false;
			expOutline.SetActive (false);
		}
	}
}
