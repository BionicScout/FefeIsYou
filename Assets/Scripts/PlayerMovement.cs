using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public SaveAndLoadScript saveScript;

	private void Update() {
		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			RuleDecifer.applyBasicRules();
			CharacterObjectList.move(0, 1);

			if (RuleDecifer.adjust == true)
				RuleDecifer.adjustRules();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			RuleDecifer.applyBasicRules();
			CharacterObjectList.move(-1, 0);

			if (RuleDecifer.adjust == true)
				RuleDecifer.adjustRules();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			RuleDecifer.applyBasicRules();
			CharacterObjectList.move(0, -1);

			if (RuleDecifer.adjust == true)
				RuleDecifer.adjustRules();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			RuleDecifer.applyBasicRules();
			CharacterObjectList.move(1, 0);

			if (RuleDecifer.adjust == true)
				RuleDecifer.adjustRules();
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			saveScript.SaveDataFun();
		}

		if (Input.GetKeyDown(KeyCode.L)) {
			saveScript.Load();
		}

		if (Input.GetKeyDown(KeyCode.O)) {
			RuleDecifer.adjustRules();
		}

	}
}
