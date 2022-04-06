using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RuleDecifer {
	public static List<List<string>> listOfRules = new List<List<string>>();
	public static List<List<string>> finalRules = new List<List<string>>();

	static List<List<string>> objIsProRules = new List<List<string>>();
	static List<List<string>> objIsObjRules = new List<List<string>>();
	static List<List<string>> objHasObjRules = new List<List<string>>();

	public static bool adjust = false;

	public enum typeIdentifier { OBJECT, OPERATOR, PROPERTY, NULL };

	static string[,] textArr;

//Main Functions
	public static void adjustRules() {
		adjust = false;

		getRules();
		//Debug.Log("Test");
		getBasicRules();
		//Debug.Log("Test1");
		applyBasicRules();
		//Debug.Log("Test2");
	}

	static void getRules() {
		textArr = LevelManager.grid.getTextArray();

		//For all grid[x, y] that has a word 
		for (int y = 0; y < textArr.GetLength(1); y++) {
			for (int x = 0; x < textArr.GetLength(0); x++) {
				//Debug.Log("Name: " + textArr[x, y] + "\tX: " + x + "Y: " + y);

				if (textArr[x, y] != "" && textArr[x, y] != null) {
					//Get Text Horizantally
					List<string> rule1 = new List<string>();

					//Debug.Log("Sep");
					for (int offset = 0; x + offset < textArr.GetLength(0) && textArr[x + offset, y] != "" && textArr[x + offset, y] != null; offset++) {
						//Debug.Log(textArr[x + offset, y]);
						rule1.Add(textArr[x + offset, y]);
					}

					RuleDecifer.listOfRules.Add(rule1);

					//Get Text Vertically
					List<string> rule2 = new List<string>();

					for (int offset = 0; y - offset > 0 && textArr[x, y - offset] != "" && textArr[x, y - offset] != null; offset++) {
						rule2.Add(textArr[x, y - offset]);
					}

					listOfRules.Add(rule2);
				}
			}
		}

		//Test for Valid Rules
		//foreach (List<string> rule in listOfRules) {
		//	string output = "";
		//	foreach (string str in rule) {
		//		output += str + " ";
		//	}
		//	Debug.Log(output + "\tWords: " + rule.Count);
		//}

	}

	static void getBasicRules() {
		finalRules.Clear();
		objIsObjRules.Clear();
		objHasObjRules.Clear();
		objIsProRules.Clear();

		for (int i = 0; i < listOfRules.Count; i++) {
			List<string> rule = listOfRules[i];
			List<typeIdentifier> type = getTypeList(rule);

			//If there is an AND, split up And if valid
			bool badAnd = false;

			for (int j = 0; j < rule.Count; j++) {
				if (rule[j] == "AND") {
					if (isValidAnd(type, j)) {
						addNewAndRules(rule, type, j);
					}
					else { //Not Valid And
						//Debug.Log("Bad And");
						badAnd = true;
					}
				}
			}

			if (badAnd)
				continue;

		//Check Each 3 piece segmen
			if (rule.Count > 3) {
				for (int index = 0; index + 2 < rule.Count; index++) {
					List<string> temp = new List<string>();
					temp.Add(rule[index]);
					temp.Add(rule[index + 1]);
					temp.Add(rule[index + 2]);

					listOfRules.Add(temp);
				}

				continue;
			}

			//IS Basic Rule
			if (!isBasicRule(rule, type)) {
				//Debug.Log("Not Baisc\t" + rule.Count);
				continue;
			}

		//Ignore Repeated Rules
			bool repeatedRule = false;

			foreach (List<string> basicRule in finalRules) {
				if (basicRule.SequenceEqual(rule))
					repeatedRule = true;
			}

			if (repeatedRule)
				continue;

			finalRules.Add(rule);

		//AssignRule List
			if (rule[1] == "IS") {
				if (getType(rule[2]) == typeIdentifier.PROPERTY) {
					objIsProRules.Add(rule);
				}
				else if (getType(rule[2]) == typeIdentifier.OBJECT) {
					objIsObjRules.Add(rule);
				}
			}
			else if (rule[1] == "HAS") {
				objHasObjRules.Add(rule);
			}
		}

		listOfRules.Clear();

		//Output Basic Rules
		//foreach (List<string> rule in objIsProRules) {
		//	string output = "";
		//	foreach (string str in rule) {
		//		output += str + " ";
		//	}
		//	Debug.Log(output);
		//}
	}

	public static void applyBasicRules() {
		CharacterObjectList.clearHas();
		CharacterObjectList.clearModifers();

		foreach (List<string> rule in objIsObjRules) {
			CharacterObjectList.replaceObjects(rule[0], rule[2]);
			Debug.Log(rule[2]);
		}

		foreach (List<string> rule in objHasObjRules) {
			CharacterObjectList.setHasObject(rule[0], rule[2]);
			Debug.Log(rule[2]);
		}

		CharacterObjectList.clearModifers();

		foreach (List<string> rule in objIsProRules) {

			if (rule[2] == "DEFEAT")
				CharacterObjectList.isDefeat(rule[0]);
			else if (rule[2] == "PULL")
				CharacterObjectList.isPull(rule[0]);
			else if (rule[2] == "PUSH")
				CharacterObjectList.isPush(rule[0]);
			else if (rule[2] == "STOP")
				CharacterObjectList.isStop(rule[0]);
			else if (rule[2] == "WEAK")
				CharacterObjectList.isWeak(rule[0]);
			else if (rule[2] == "WIN")
				CharacterObjectList.isWin(rule[0]);
			else if (rule[2] == "YOU")
				CharacterObjectList.isYou(rule[0]);

			//Debug.Log(rule[2]);

		}

		if (CharacterObjectList.youList.Count == 0) {
			LevelManager.instance.lossed.SetActive(true);
			LevelManager.instance.lossedBool = true;
		}

		//Check For you && win  or  you && defeat
		for (int i = CharacterObjectList.youList.Count - 1; i >= 0; i--) {
			CharacterObjectList.youList[i].checkWinOrDefeat();
		}

		for (int i = CharacterObjectList.youDelete.Count - 1; i >= 0; i--) {
			CharacterObjectList.youDelete[i].Delete();
		}

		CharacterObjectList.youDelete.Clear();
	}

//Other Functions
	static List<typeIdentifier> getTypeList(List<string> rule) {
		List<typeIdentifier> type = new List<typeIdentifier>();

		for (int i = 0; i < rule.Count; i++) {
			switch (rule[i]) {
				case "AND":
				case "IS":

					type.Add(typeIdentifier.OPERATOR);
					break;

				case "DEFEAT":
				case "PULL":
				case "PUSH":
				case "STOP":
				case "WEAK":
				case "WIN":
				case "YOU":

					type.Add(typeIdentifier.PROPERTY);
					break;

				default:

					type.Add(typeIdentifier.OBJECT);
					break;
			}
		}

		return type;
	}
	static typeIdentifier getType(string word) {

			switch (word) {
				case "AND":
				case "IS":

					return typeIdentifier.OPERATOR;

			case "DEFEAT":
			case "PULL":
			case "PUSH":
			case "STOP":
			case "WEAK":
			case "WIN":
			case "YOU":

				return typeIdentifier.PROPERTY;

				default:

					return typeIdentifier.OBJECT;

			}


		return typeIdentifier.NULL;
	}

	static bool isValidAnd(List<typeIdentifier> type, int andIndex) {
		if (andIndex - 1 < 0 || andIndex + 1 >= type.Count)
			return false;

	//Check if the words on both sides of And are both objects or props
		bool isObjects = (type[andIndex - 1] == typeIdentifier.OBJECT) && (type[andIndex + 1] == typeIdentifier.OBJECT);
		bool isProp = (type[andIndex - 1] == typeIdentifier.PROPERTY) && (type[andIndex + 1] == typeIdentifier.PROPERTY);
		if (!isObjects && !isProp)
			return false;

		return true;
	}

	//CAN OPTIMIZE TO REMOVE DUPLICATES
	static void addNewAndRules(List<string> rule, List<typeIdentifier> type, int andIndex) {
		List<string> newRule1 = new List<string>();
		List<string> newRule2 = new List<string>();

		for (int i = 0; i < rule.Count; i++){
			if(andIndex - 1 == i) {
				newRule1.Add(rule[i]);
				newRule2.Add(rule[i+2]);

				i += 2;

				continue;
			}

			newRule1.Add(rule[i]);
			newRule2.Add(rule[i]);
		}

		listOfRules.Add(newRule1);
		listOfRules.Add(newRule2);
	}

	/*
		There are 3 basic rule types:
			OBJ IS PROP
			Obj IS OBJ
			OBJ has OBJ
	*/
	static bool isBasicRule(List<string> rule, List<typeIdentifier> type) {
		if (rule.Count < 3)
			return false;

		//Test First word 
		if (type[0] != typeIdentifier.OBJECT)
			return false;

	//Test Middle Word
		bool isHas = false;

		if (rule[1] == "HAS")
			isHas = true;
		else if (rule[1] != "IS")
			return false;

		//Test Last Word
		if (type[2] == typeIdentifier.OPERATOR || (type[2] == typeIdentifier.PROPERTY && isHas))
			return false;

		return true;
	}
}
