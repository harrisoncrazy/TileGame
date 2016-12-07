using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	void Awake () {
		Instance = this;
	}

	public bool placingCity;

	public bool tileSelected;

	public Text tileInfo;

	//Sprites for tiles
	public Sprite cabin;
	public Sprite grass;
	public Sprite farm;
	public GameObject cityMainTile;

	//Tile adding Bools
	public bool cabinPlace = false;
	public bool woodChop = false;
	public bool farmPlace = false;

	//Tile types
	public GameObject HeavyForestTile;
	public GameObject LightForestTile;
	public GameObject GrassTile;
	public GameObject GrassRockTile;
	public GameObject GrassRock2Tile;

	//Map Size ints, and floats for generation
	public int mapSizeX = 11;
	public int mapSizeY = 11;
	private float iDiff = 0;
	private float jDiff = 0;

	//Resources!
	public float food = 4;
	public float wood = 0;
	public float manpower = 2;

	//Number of Resource Buildings
	public int cabinNumber = 0;
	public int farmNumber = 0;

	//Grabbing the text for updating resource count
	public Text foodText;
	public Text woodText;
	public Text manText;

	public float timer = 5;

	public List <List<TileSelect>> map = new List<List<TileSelect>>(); 

	// Use this for initialization
	void Start () {
		generateMap ();
		placingCity = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (placingCity == true) {

		} 
		else if (placingCity == false) {
			passiveIncome ();
			checkKey ();

			foodText.text = "Food: " + food;
			woodText.text = "Wood: " + wood;
			manText.text = "Manpower: " + manpower;
		}
	}

	void checkKey() {
		if (Input.GetKeyDown ("v")) {
			woodChop = false;
			farmPlace = false;
			if (food >= 4 && wood >= 5 && manpower >= 2) {
				if (cabinPlace == true) {
					cabinPlace = false;
					tileInfo.text = " ";
				} else if (cabinPlace == false) {
					cabinPlace = true;
					tileInfo.text = "Placing: Cabin! Costs: 4 Food 5 Wood 2 Manpower";
				}
			} else if (food < 4 || wood < 5 || manpower < 2 && cabinPlace == true) {
				cabinPlace = false;
				tileInfo.text = "Insufficent Resources!";
			} else {
				tileInfo.text = " ";
			}
		}
		if (Input.GetKeyDown ("e")) {
			cabinPlace = false;
			farmPlace = false;
			if (woodChop == false) {
				tileInfo.text = "Chopping down wood!";
				woodChop = true;
			} else if (woodChop == true) {
				tileInfo.text = " ";
				woodChop = false;
			}

		}
		if (Input.GetKeyDown ("f")) {
			cabinPlace = false;
			woodChop = false;
			if (manpower >= 2 && wood >= 5) {
				if (farmPlace == false) {
					tileInfo.text = "Placeing: Farm! Costs: 2 Manpower 5 Wood";
					farmPlace = true;
				} else if (farmPlace == true) {
					tileInfo.text = " ";
					farmPlace = false;
				}
			} else if (manpower < 2 || wood < 5 && farmPlace == true) {
				farmPlace = false;
				tileInfo.text = "Insufficent Resources!";
			} else {
				tileInfo.text = " ";
			}
		}
	}

	void generateMap() {
		map = new List<List<TileSelect>>(); //generatign the playing field, making a grid of tile prefabs, and storing their positiosn in a 2d list
		for (int i = 0; i < mapSizeX; i++) {
			List <TileSelect> row = new List<TileSelect>();
			for (int j = 0; j < mapSizeY; j++) {
				if (i == 0) {
					iDiff = 0.8f;
				}
				//detects wether or not its an edge tile or not and pushes a row in if it needs to be
				if (j % 2 == 0) {
					iDiff = i + (.2f * (i+1));
				} else if (i != 0) {
					iDiff = i + 0.6f + (.2f * (i+1)); //Offsetting the X
				}
				jDiff = j + (.03f * j); //Offsetting the Y
				int rand = Random.Range (1, 101);
				if (rand <= 45) {
					TileSelect tile = ((GameObject)Instantiate (HeavyForestTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<TileSelect> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Heavy Forest";
					tile.GetComponent<TileSelect> ().tileInfo = GameObject.Find ("InfoText").GetComponent<Text>();
					tile.resourceType = "Production";
					tile.resourceProductionAmount = 2;
					row.Add (tile);
				} else if (rand >= 45 && rand <= 70) {
					TileSelect tile = ((GameObject)Instantiate (LightForestTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<TileSelect> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Light Forest";
					tile.resourceType = "Food/Production";
					tile.resourceFoodAmount = 1;
					tile.resourceProductionAmount = 1;
					tile.GetComponent<TileSelect> ().tileInfo = GameObject.Find ("InfoText").GetComponent<Text>();
					row.Add (tile);
				} else if (rand >= 70 && rand <= 90 ) {
					TileSelect tile = ((GameObject)Instantiate (GrassTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<TileSelect> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Grassland";
					tile.resourceType = "Food";
					tile.resourceFoodAmount = 2;
					tile.GetComponent<TileSelect> ().tileInfo = GameObject.Find ("InfoText").GetComponent<Text>();
					row.Add (tile);
				} else if (rand >= 90 && rand <= 97) {
					TileSelect tile = ((GameObject)Instantiate (GrassRockTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<TileSelect> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Light Rocks";
					tile.resourceType = "Food/Production";
					tile.resourceFoodAmount = .5f;
					tile.resourceProductionAmount = .5f;
					tile.GetComponent<TileSelect> ().tileInfo = GameObject.Find ("InfoText").GetComponent<Text>();
					row.Add (tile);
				} else if (rand >= 97 && rand <= 100) {
					TileSelect tile = ((GameObject)Instantiate (GrassRock2Tile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<TileSelect> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Heavy Rocks";
					tile.resourceType = "Production";
					tile.resourceProductionAmount = 1;
					tile.GetComponent<TileSelect> ().tileInfo = GameObject.Find ("InfoText").GetComponent<Text>();
					row.Add (tile);
				}

			}

			map.Add(row);
		}


	}

	void passiveIncome () {
		if (timer >= 5) {
			if (manpower <= 100) {
				manpower = manpower + cabinNumber;
			}
			if (food <= 100) {
				food = food +  (farmNumber * .5f);
			}
			timer = 0;
		} else {
			timer += Time.deltaTime;
		}
	}
}

