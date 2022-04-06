using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterObjectList {
	public static List<CharacterObject> list = new List<CharacterObject>();
	public static List<CharacterObject> textList = new List<CharacterObject>();
	public static List<CharacterObject> youList = new List<CharacterObject>();
	public static List<CharacterObject> youDelete = new List<CharacterObject>();

	//Player Input
	public static void move(int x, int y) {
		foreach (CharacterObject co in youList) {
			co.move(x, y);
		}

		for (int i = youDelete.Count - 1; i >= 0; i--) {
			youDelete[i].Delete();
		}

		youDelete.Clear();
	}


	public static void checkNull() {
		for (int i = textList.Count - 1; i >= 0; i--) {
			if(textList[i] == null) {
				textList.Remove(textList[i]);
			}				
		}

		for (int i = youList.Count - 1; i >= 0; i--) {
			if (youList[i] == null) {
				youList.Remove(youList[i]);
			}
		}

		for (int i = youDelete.Count - 1; i >= 0; i--) {
			if (youDelete[i] == null) {
				youDelete.Remove(youDelete[i]);
			}
		}

		for (int i = list.Count - 1; i >= 0; i--) {
			if (list[i] == null) {
				list.Remove(list[i]);
			}
		}
	}




	//Clear
	public static void clearModifers() {
		foreach(CharacterObject co in list) {
			co.clearMods();
		}

		youList.Clear();
	}

	public static void clearHas() {
		foreach (CharacterObject co in list) {
			co.clearHas();
		}
	}

//Replace Objects and Has Objects
	public static void replaceObjects(string oldObj, string newObj) {
		for (int i = 0; i < list.Count; i++) {
			if (list[i].name == oldObj) {
				//Copy Info
				GameObject currentObject = list[i].getObject();
				Vector3 pos = currentObject.transform.position;
				list[i].Delete();

				//Put In Info
				GameObject temp = Object.Instantiate(LevelManager.instance.findPrefab(newObj));
				temp.transform.position = pos;
				//list[i] = temp.GetComponent<CharacterObject>();					
			}
		}
	}

	public static void setHasObject(string obj, string hasThisObject) {
		foreach (CharacterObject co in list) {
			if (co.name == obj) {
				co.hasObject = hasThisObject;
			}
		}
	}

//Set Mods
	public static void isDefeat(string name) {
		foreach (CharacterObject co in list) {
			if(co.name == name) {
				co.isDefeat = true;
			}
		}
	}

	public static void isPull(string name) {
		foreach (CharacterObject co in list) {
			if (co.name == name) {
				co.isPull = true;
			}
		}
	}

	public static void isPush(string name) {
		foreach (CharacterObject co in list) {
			Debug.Log("Name: " + name + "\tName: " + co.name + "\nMatch: " + (co.name == name));
			if (co.name == name) {
				co.isPush = true;
			}
		}
	}

	public static void isStop(string name) {
		foreach (CharacterObject co in list) {
			if (co.name == name) {
				co.isStop = true;
			}
		}
	}

	public static void isWeak(string name) {
		foreach (CharacterObject co in list) {
			if (co.name == name) {
				co.isWeak = true;
			}
		}
	}

	public static void isWin(string name) {
		foreach (CharacterObject co in list) {
			if (co.name == name) {
				co.isWin = true;
			}
		}
	}

	public static void isYou(string name) {
		foreach (CharacterObject co in list) {
			if (co.name == name) {
				co.isYou = true;
				youList.Add(co);
			}
		}
	}

}
