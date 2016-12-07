using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileSelect : MonoBehaviour {

	public GameObject cityMainTile = GameManager.Instance.cityMainTile;
	public GameObject tileOutlineSprite;

	public bool enabled = false;

	static private Transform trSelect = null;
	private bool selected = false;

	public Text tileInfo;

	public string tileType;

	public Vector2 gridPosition;

	public float chopDownRate;

	SpriteRenderer sr;

	public bool chopped = false;

	//city stuffs
	public bool ownedByCity = false;
	public bool changed = false;
	public bool lightForest = false;
	public bool heavyForest = false;

	//tile resources
	public float resourceFoodAmount;
	public float resourceProductionAmount;
	public string resourceType;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		if (tileType == "Light Forest") {//setting a random number of clicks for chopping down forest
			chopDownRate = Random.Range (1, 3);
			lightForest = true;
		} else if (tileType == "Heavy Forest") {//setting a random number of clicks for chopping down forest
			chopDownRate = Random.Range (2, 7);
			heavyForest = true;
		}
	}

	void Update () {
		if (selected && transform != trSelect) { //If the currently selected tile and the transform do not equal the new selection, deselect this tile
			selected = false;
			tileOutlineSprite.SetActive (false);
			enabled = false;
			GameManager.Instance.tileSelected = false;
			cityTile.Instance.enabled = false;
			cityTile.Instance.selected = false;
			cityTile.Instance.tileOutlineSprite.SetActive (false);
		}
		if (GameManager.Instance.placingCity == false) {
			if (cityTile.Instance.enabled == true) {
				tileOutlineSprite.SetActive (false);
			}
		}

		if (ownedByCity == true && changed == false) {
			tileType = tileType + "\nOwned by City";
			changed = true;
		}
	}

	public void OnMouseOver() {
		
	}

	public void OnMouseExit() {
		
	}

	public void OnMouseDown() {
		if (GameManager.Instance.cabinPlace == false && GameManager.Instance.woodChop == false && GameManager.Instance.farmPlace == false && GameManager.Instance.placingCity == false) {//default tile selection
			if (selected && transform == trSelect) {
				selected = false;
				trSelect = null;
				tileOutlineSprite.SetActive (false);
				enabled = false;
				GameManager.Instance.tileSelected = false;
				this.tileInfo.text = " ";
			} else {
				selected = true;
				trSelect = transform;
				tileOutlineSprite.SetActive (true);
				enabled = true;
				GameManager.Instance.tileSelected = true;
				this.tileInfo.text = tileType;
			}
		
		} else if (GameManager.Instance.placingCity == true) {//placing a city tile selection
			GameObject newCity = Instantiate (cityMainTile);
			newCity.transform.position = this.transform.position;
			Destroy (this.gameObject);
			GameManager.Instance.placingCity = false;

		} else if (GameManager.Instance.cabinPlace == true) {//placing a cabin selection
			if (ownedByCity = true) {
				tileType = "Cabin\nOwned by City";
			} else {
				tileType = "Cabin";
			}
			sr.sprite = GameManager.Instance.cabin;
			GameManager.Instance.food--;
			GameManager.Instance.wood--;
			GameManager.Instance.manpower--;
			GameManager.Instance.cabinPlace = false;
			GameManager.Instance.cabinNumber++;

		} else if (GameManager.Instance.woodChop == true) { //chopping wood selection
			if (chopDownRate > 0) {
				if (lightForest == true || heavyForest == true) {
					chopDownRate--;
					this.tileInfo.text = "Click " + (chopDownRate + 1) + " more times to gather this material";
				} else {
					tileInfo.text = "This is not a valid tile to chop";
				}
			} else if (chopDownRate == 0) {
				if (chopped == false) {
					if (lightForest == true) {
						chopped = true;
						if (ownedByCity = true) {
							this.tileType = "Grassland\nOwned by City";
						} else {
							this.tileType = "Grassland";
						}
						tileInfo.text = " ";
						sr.sprite = GameManager.Instance.grass; 
						GameManager.Instance.wood = GameManager.Instance.wood + 2;
						resourceType = "Food";
						resourceFoodAmount = 1.5f;
						cityTile.Instance.cityFoodResource += resourceFoodAmount;
						cityTile.Instance.cityProductionResource -= .5f;
					} else if (heavyForest == true) {
						chopped = true;
						this.tileType = "Grassland";
						tileInfo.text = " ";
						sr.sprite = GameManager.Instance.grass;
						GameManager.Instance.wood = GameManager.Instance.wood + 4;
						resourceType = "Food";
						resourceFoodAmount = 2;
						cityTile.Instance.cityFoodResource += resourceFoodAmount;
						cityTile.Instance.cityProductionResource -= 2;
					} 
				}
			}
		} else if (GameManager.Instance.farmPlace == true) { //placing a farm selection
			tileType = "Farm";
			sr.sprite = GameManager.Instance.farm;
			GameManager.Instance.wood -= 5;
			GameManager.Instance.manpower -= 2;
			GameManager.Instance.farmPlace = false;
			GameManager.Instance.farmNumber++;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "Collider") {
			ownedByCity = true;
			if (resourceType == "Food") {
				cityTile.Instance.cityFoodResource += resourceFoodAmount;
			}
			if (resourceType == "Production") {
				cityTile.Instance.cityProductionResource += resourceProductionAmount;
			}
			if (resourceType == "Food/Production") {
				cityTile.Instance.cityFoodResource += resourceFoodAmount;
				cityTile.Instance.cityProductionResource += resourceProductionAmount;
			}
		}

	}
}
/*/if (GameManager.Instance.cabinPlace == false) {
			if (GameManager.Instance.tileSelected == false) {
				if (enabled == false) {
					tileOutlineSprite.SetActive (true);
					enabled = true;
					GameManager.Instance.tileSelected = true;
					this.tileInfo.text = tileType;
				}
			} else if (enabled == true) {
				tileOutlineSprite.SetActive (false);
				enabled = false;
				GameManager.Instance.tileSelected = false;
				this.tileInfo.text = " ";
			}
		} else if (GameManager.Instance.tileSelected == false) {
			tileType = "Cabin";
			sr.sprite = cabin;
			GameManager.Instance.food--;
			GameManager.Instance.wood--;
			GameManager.Instance.manpower--;
		}/*/