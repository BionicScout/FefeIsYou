using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	public Sprite[] spriteList;
	public static string[,] textArr;
	public static Grid grid;

	public GameObject[] objectPrefabs;

	public static LevelManager instance;

	public int x, y;
	public float size;
	public Vector3 origin;

	float timer = 0;
	public bool won;
	float timerTarget = 2;

	public bool paused;
	public GameObject menu;

	public bool lossedBool;
	public GameObject lossed;


	void Awake() {
		textArr = new string[x, y];

		grid = new Grid(x, y, size, origin, textArr, spriteList);
		instance = this;

		menu.SetActive(false);
		lossed.SetActive(false);
	}

	public LevelManager(GameObject menu, GameObject lossed) {
		this.menu = menu;
		this.lossed = lossed;
	}

	private void Update() {
		if (won) {
			timer += Time.deltaTime;

			if (timerTarget <= timer) {
				

				CharacterObjectList.list.Clear();
				CharacterObjectList.youList.Clear();
				CharacterObjectList.textList.Clear();
				CharacterObjectList.youDelete.Clear();

				SceneSwitcher.instance.A_LoadScene(SceneSwitcher.currentScene + 1);

				CharacterObjectList.checkNull();


				RuleDecifer.applyBasicRules();

				instance = new LevelManager(menu, lossed);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			paused = !paused;
			menu.SetActive(paused);
		}
	}

	IEnumerator Start() {
		yield return new WaitForSeconds(0.1f);
		RuleDecifer.adjustRules();
	}

	public GameObject findPrefab(string name) {
		for(int i = 0; i < objectPrefabs.Length; i++) {
			if (objectPrefabs[i].name == name + "_Prefab") {
				return objectPrefabs[i];
			}
		}

		Debug.LogError("Object Prefab not found");
		return null;
	}

	public GameObject getPrefab(string name) {
		for (int i = 0; i < objectPrefabs.Length; i++) {
			if (objectPrefabs[i].name == name) {
				return objectPrefabs[i];
			}
		}

		Debug.LogError("Object Prefab not found");
		return null;
	}

	public void win() {
		AudioManager.instance.Play("Win");
		won = true;

		//CharacterObject.disable();
	}
}
