using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoadScript : MonoBehaviour {
	public static SaveData levels = new SaveData();
	public string fileName;

	public void SaveDataFun() {
	//Save Grid
		Grid grid = LevelManager.grid;

		levels.width = grid.width;
		levels.height = grid.height;
		levels.cellSize = grid.cellSize;
		levels.setOrigin(grid.originPos);
		levels.setGrid(grid.grid);

	//Save Objects
		List<string> name = new List<string>();
		List<int> x = new List<int>();
		List<int> y = new List<int>();
		List<string> prefabName = new List<string>();

		foreach (CharacterObject co in CharacterObjectList.list) {
			name.Add(co.name);
			x.Add(co.x);
			y.Add(co.y);
			prefabName.Add(co.name + "_Prefab");
		}

		foreach (CharacterObject co in CharacterObjectList.textList) {
			name.Add(co.name);
			x.Add(co.x);
			y.Add(co.y);
			prefabName.Add(co.name + "_Text_Prefab");
		}

		levels.name = name.ToArray();
		levels.x = x.ToArray();
		levels.y = y.ToArray();
		levels.prefabName = prefabName.ToArray();

	//Save
		SaveFile(SaveAndLoadScript.levels);
		Debug.LogWarning("SAVED");
	}

	public void SaveFile(SaveData data) {
		FileStream file = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create); //OpenOrCreate

		BinaryFormatter converter = new BinaryFormatter();
		converter.Serialize(file, data);

		file.Close();
	}

	public void Load() {
		LoadFile(out levels);

		LevelManager.grid = new Grid(levels);

		for (int i = 0; i < levels.prefabName.Length; i++) {
			GameObject temp = Object.Instantiate(LevelManager.instance.getPrefab(levels.prefabName[i]));

			CharacterObject co = temp.GetComponent<CharacterObject>();
			co.name = levels.name[i];
			co.x = levels.x[i];
			co.y = levels.y[i];
		}


	}

	public void LoadFile(out SaveData data) {
		if(! System.IO.File.Exists(Application.persistentDataPath + "/" + fileName)) {
			Debug.LogError("Loaded file was NOT found");
			data = new SaveData();
			return;
		}

		//OpenFile
		FileStream file = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Open);

		//
		BinaryFormatter converter = new BinaryFormatter();
		data = (SaveData)converter.Deserialize(file);

		file.Close();
		Debug.LogWarning("LOADED");
	}
}
