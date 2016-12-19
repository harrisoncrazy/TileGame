using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public Transform selectedTile;

	public bool NewTurn; //NEW TURN BOOL, USED FOR STARTING A NEW TURN

	public int waterStore;

	public string[] playerPeople; //different people names
	public int playerPeopleNum = 1;

	public FoodData[] storedFood;
	public ToolType[] storedTools;

	public struct FoodData { //storing different food types and vals associated
		public string foodType;
		public int foodVal;
	}
	private FoodData temp; //temp val for swapping food variables

	public struct ToolType { //storing different Tools and thier efficencies
		public bool isAxe;
		public bool isPick;
		public float efficency;
	}


	// Use this for initialization
	void Start () {
		Instance = this;
		playerPeople = new string[7];
		playerPeople [0] = "Tim";
		storedFood = new FoodData[32];
		storedFood [0].foodType = "Bread"; //adding initial food and water stores
		storedFood [0].foodVal = 4;
		storedFood [1].foodType = "Bread";
		storedFood [1].foodVal = 4;
		storedFood [2].foodType = "Bread";
		storedFood [2].foodVal = 4;
		storedFood [3].foodType = "Bread"; //adding initial food and water stores
		storedFood [3].foodVal = 4;
		storedFood [4].foodType = "Bread";
		storedFood [4].foodVal = 4;
		storedFood [5].foodType = "Bread";
		storedFood [5].foodVal = 4;
		waterStore = 48;

		storedTools = new ToolType[32]; //adding initial tools
		storedTools [0].isAxe = true;
		storedTools [0].isPick = false;
		storedTools [0].efficency = 0.5f; //axe with efficency of .5

		storedTools [0].isAxe = false;
		storedTools [0].isPick = true;
		storedTools [0].efficency = 0.5f; //pick with efficency of .5
	}
	
	// Update is called once per frame
	void Update () {
		if (NewTurn == true) { //moving to the next turn
			int neededFood = playerPeopleNum * 2; //getting amount of needed food
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


			waterStore -= playerPeopleNum * 4; //subtracting needed water

			NewTurn = false; //ending the turn swap
		}
	}
}
