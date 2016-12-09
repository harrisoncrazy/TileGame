using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class generationManager : MonoBehaviour {

	public static generationManager Instance;

	public GameObject DefaultTile;

	//Grass tile sprites
	public Sprite HeavyForestTile;
	public Sprite LightForestTile;
	public Sprite GrassTile;
	public Sprite HeavyRockTile;
	public Sprite LightRockTile;

	//Desert tile Sprites
	public Sprite defaultSand;
	public Sprite lightCactusSand1;
	public Sprite lightCactusSand2;
	public Sprite heavyCactusSand;
	public Sprite lightRocksSand1;
	public Sprite lightRocksSand2;
	public Sprite lightRocksSand3;

	//Dirt tile Sprites
	public Sprite defaultDirt;
	public Sprite lightForestDirt;
	public Sprite heavyForestDirt;
	public Sprite lightRocksDirt1;
	public Sprite lightRocksDirt2;

	//Snow tile Sprites
	public Sprite HeavyForestSnowTile;
	public Sprite LightForestSnowTile;
	public Sprite GrassSnowTile;
	public Sprite HeavyRockSnowTile;
	public Sprite LightRockSnowTile;

	//Stone tile Sprites
	public Sprite defaultStone;
	public Sprite LightForestStone;
	public Sprite HeavyForestStone;

	public Sprite oceanTile;

	public Sprite mountainTile;

	//river generation values
	private Vector2 point1;
	private Vector2 point2;

	private float generationTimerOne = 1.0f;
	private float generationTimerTwo = 1.0f;
	public bool genStepOneDone = false;
	public bool genStepTwoDone = false;
	private int sandRand;
	private int snowRand;
	private int oceanRand;
	private int mountainRand;
	private int riverRand;

	//Map Size ints, and floats for generation
	public int mapSizeX = 11;
	public int mapSizeY = 11;
	private float iDiff = 0;
	private float jDiff = 0;

	public List <List<tileHandler>> map = new List<List<tileHandler>>(); 

	//Perlin Noise Generation
	private float scale = 3f;


	// Use this for initialization
	void Start () {
		Instance = this;
		generateMap ();
		sandRand = Random.Range (2, 4);
		for (int i = 0; i <= sandRand; i++) {
			Debug.Log ("Sand seed planted.");
			generateSand ();
		}
		snowRand = Random.Range (1, 5);
		for (int i = 0; i <= snowRand; i++) {
			Debug.Log ("Snow seed planted.");
			generateSnow ();
		}
		oceanRand = Random.Range (3, 6);
		for (int i = 0; i <= oceanRand; i++) {
			Debug.Log ("Ocean seed planted.");
			generateOcean ();
		}
		mountainRand = Random.Range (8, 15);
		for (int i = 0; i <= mountainRand; i++) {
			Debug.Log ("Mountain seed planted.");
			generateMountain();
		}

		riverRand = Random.Range (8, 15);
		//for (int i = 0; i <= riverRand; i++) {
			Debug.Log ("River seed planted.");
			//generateRiver();
		//}
	}
	
	// Update is called once per frame
	void Update () {
		if (genStepOneDone != true) {
			generationTimerOne -= Time.deltaTime;//timer for step one
			if (generationTimerOne <= 0) {
				genStepOneDone = true;
				Debug.Log ("GENERATION STEP ONE DONE");
			}
		}

		if (genStepTwoDone != true) {
			if (genStepOneDone == true) { //timer for step 2
				generationTimerTwo -= Time.deltaTime;
				if (generationTimerTwo <= 0) {
					genStepTwoDone = true;
					Debug.Log ("GENERATION STEP TWO DONE");
				}
			}
		}

		if (genStepTwoDone == true) {
			for (int i = 0; i <= mapSizeX - 1; i++) {
				for (int j = 0; j <= mapSizeY - 1; j++) {
					if (map [i] [j].tileType == "Sand") {//Iterating thru the list of tiles, finding those marked as default sand and selecting a random tile for them to be
						if (map [i] [j].newSpriteSet == false) {
							int rand = Random.Range (1, 101);
							if (rand <= 65) {
								map [i] [j].newSpriteSet = true;
							} else if (rand >= 65 && rand <= 70) {
								int rand2 = Random.Range (1, 2);
								if (rand2 == 1) {
									map [i] [j].sr.sprite = lightCactusSand1;
									map [i] [j].newSpriteSet = true;
								} else if (rand2 == 2) {
									map [i] [j].sr.sprite = lightCactusSand2;
									map [i] [j].newSpriteSet = true;
								}
							} else if (rand >= 70 && rand <= 80) {
								int rand2 = Random.Range (1, 3);
								if (rand2 == 1) {
									map [i] [j].sr.sprite = lightRocksSand1;
									map [i] [j].newSpriteSet = true;
								} else if (rand2 == 2) {
									map [i] [j].sr.sprite = lightRocksSand2;
									map [i] [j].newSpriteSet = true;
								} else if (rand2 == 3) {
									map [i] [j].sr.sprite = lightRocksSand3;
									map [i] [j].newSpriteSet = true;
								}
							} else if (rand >= 80 && rand <= 100) {
								map [i] [j].sr.sprite = heavyCactusSand;
								map [i] [j].newSpriteSet = true;
							}
						}
					}
					if (map [i] [j].tileType == "Dirt") {//finding the dirt tiles and randomizing
						if (map [i] [j].newSpriteSet == false) {
							int rand = Random.Range (1, 101);
							if (rand <= 35) {
								map [i] [j].newSpriteSet = true;
							} else if (rand >= 35 && rand <= 70) {
								int rand2 = Random.Range (1, 2);
								if (rand2 == 1) {
									map [i] [j].sr.sprite = lightForestDirt;
									map [i] [j].newSpriteSet = true;
								}
							} else if (rand >= 70 && rand <= 80) {
								int rand2 = Random.Range (1, 3);
								if (rand2 == 1) {
									map [i] [j].sr.sprite = lightRocksDirt1;
									map [i] [j].newSpriteSet = true;
								}
							} else if (rand >= 80 && rand <= 100) {
								map [i] [j].sr.sprite = heavyForestDirt;
								map [i] [j].newSpriteSet = true;
							}
						}
					}

					if (map [i] [j].tileType == "Snow") {//finding the snow tiles and randomizing
						if (map [i] [j].newSpriteSet == false) {
							int rand = Random.Range (1, 101);
							if (rand <= 35) {
								map [i] [j].newSpriteSet = true;;
							} else if (rand >= 35 && rand <= 60) {
								map [i] [j].sr.sprite = LightForestSnowTile;
								map [i] [j].newSpriteSet = true;
							} else if (rand >= 60 && rand <= 90 ) {
								map [i] [j].sr.sprite = HeavyForestSnowTile;
								map [i] [j].newSpriteSet = true;
							} else if (rand >= 90 && rand <= 97) {
								map [i] [j].sr.sprite = LightRockSnowTile;
								map [i] [j].newSpriteSet = true;
							} else if (rand >= 97 && rand <= 100) {
								map [i] [j].sr.sprite = HeavyRockSnowTile;
								map [i] [j].newSpriteSet = true;
							}
						}
					}

					if (map [i] [j].tileType == "Stone") {//finding the snow tiles and randomizing
						if (map [i] [j].newSpriteSet == false) {
							int rand = Random.Range (1, 101);
							if (rand <= 45) {
								map [i] [j].newSpriteSet = true;;
							} else if (rand >= 45 && rand <= 75) {
								map [i] [j].sr.sprite = LightForestStone;
								map [i] [j].newSpriteSet = true;
							} else if (rand >= 75 && rand <= 100 ) {
								map [i] [j].sr.sprite = HeavyForestStone;
								map [i] [j].newSpriteSet = true;
							}
						}
					}
				}
			}
		}
	}

	void generateMap() { //Generating default map
		map = new List<List<tileHandler>>(); //generatign the playing field, making a grid of tile prefabs, and storing their positiosn in a 2d list
		for (int i = 0; i < mapSizeX; i++) {
			List <tileHandler> row = new List<tileHandler>();
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
				/*
				float seed = Random.Range (1, 10);
				float height = Mathf.PerlinNoise (seed + i/scale, seed + j/scale);
				 if (height >= .3 && height <= 0.4f) {
					tileHandler tile = ((GameObject)Instantiate (DefaultTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<tileHandler> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Ocean";
					tile.sr.sprite = oceanTile;
					row.Add (tile);
				} else {*/
				if (rand <= 35) {
					tileHandler tile = ((GameObject)Instantiate (DefaultTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<tileHandler> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Heavy Forest";
					tile.sr.sprite = HeavyForestTile;
					row.Add (tile);
				} else if (rand >= 35 && rand <= 60) {
					tileHandler tile = ((GameObject)Instantiate (DefaultTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<tileHandler> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Light Forest";
					tile.sr.sprite = LightForestTile;
					row.Add (tile);
				} else if (rand >= 60 && rand <= 90) {
					tileHandler tile = ((GameObject)Instantiate (DefaultTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<tileHandler> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.tileType = "Grassland";
					tile.sr.sprite = GrassTile;
					row.Add (tile);
				} else if (rand >= 90 && rand <= 97) {
					tileHandler tile = ((GameObject)Instantiate (DefaultTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<tileHandler> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.sr.sprite = HeavyForestTile;
					tile.tileType = "Light Rocks";
					tile.sr.sprite = LightRockTile;
					row.Add (tile);
				} else if (rand >= 97 && rand <= 100) {
					tileHandler tile = ((GameObject)Instantiate (DefaultTile, new Vector3 (iDiff, jDiff, 0), Quaternion.Euler (new Vector3 ()))).GetComponent<tileHandler> ();
					tile.gridPosition = new Vector2 (i, j);
					tile.sr.sprite = HeavyForestTile;
					tile.tileType = "Heavy Rocks";
					tile.sr.sprite = HeavyRockTile;
					row.Add (tile);
				}
			}
			map.Add(row);
		}
	}

	void generateSand() {
		int xPlacer = Random.Range (0, mapSizeX - 1);//Generating random place to put the sand seed
		int yPlacer = Random.Range (0, mapSizeY - 1);
		Debug.Log (xPlacer);
		Debug.Log (yPlacer);
		map [xPlacer] [yPlacer].sandSeed = true;//placing the sand
		map [xPlacer] [yPlacer].sr.sprite = defaultSand;
	}

	void generateSnow() {
		int xPlacer = Random.Range (0, mapSizeX - 1);//Generating random place to put the sand seed
		int yPlacer = Random.Range (0, mapSizeY - 1);
		Debug.Log (xPlacer);
		Debug.Log (yPlacer);
		map [xPlacer] [yPlacer].snowSeed = true;//placing the sand
		map [xPlacer] [yPlacer].sr.sprite = GrassSnowTile;
	}

	void generateOcean() {
		int xPlacer = Random.Range (0, mapSizeX - 1);//Generating random place to put the sand seed
		int yPlacer = Random.Range (0, mapSizeY - 1);
		Debug.Log (xPlacer);
		Debug.Log (yPlacer);
		map [xPlacer] [yPlacer].oceanSeed = true;//placing the sand
		map [xPlacer] [yPlacer].sr.sprite = oceanTile;
	}
		
	void generateMountain() {
		int xPlacer1 = Random.Range (0, mapSizeX);//Generating random place to put the sand seed
		int yPlacer1 = Random.Range (0, mapSizeY);
		map [xPlacer1] [yPlacer1].mountainSeed = true;//placing the sand
		map [xPlacer1] [yPlacer1].sr.sprite = mountainTile;
		map [xPlacer1] [yPlacer1].tileType = "Mountain";
	}

	void generateRiver() {
		int xPlacer1 = Random.Range (0, mapSizeX);//Generating random place to put the sand seed
		int yPlacer1 = Random.Range (0, mapSizeY);
		map [xPlacer1] [yPlacer1].riverSeed = true;//placing the sand
		map [xPlacer1] [yPlacer1].sr.sprite = oceanTile;
		map [xPlacer1] [yPlacer1].tileType = "River";
	}
}