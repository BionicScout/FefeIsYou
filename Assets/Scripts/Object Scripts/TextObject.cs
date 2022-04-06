using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextObject : CharacterObject {
	protected override void customStart() {
		CharacterObjectList.textList.Add(this);
		transform.position = LevelManager.grid.getWorldPosition(x, y) + LevelManager.grid.originPos;
	}

	public override bool move(int x, int y) {
		//Debug.Log("Name: " + name + "\tY: " + index);

		if (this.x + x < 0 || this.x + x > LevelManager.grid.width - 1 || this.y + y < 0 || this.y + y > LevelManager.grid.height - 1) {
			Debug.LogError("Invalid x or y in Move\nX: " + this.x + x + "\tY: " + this.y + y);
			return false;
		}

		if (checkNextTile(this.x + x, this.y + y) == false) {
			//Debug.LogError("You is Stopped");
			return false;
		}

		foreach (CharacterObject co in CharacterObjectList.list) {
			if (co.x == this.x - x && co.y == this.y - y && co != this && co.isPull == true) {
				co.move(x, y);
				break;
			} //Add objects to move to list and then move
		}

		this.x = this.x + x;
		this.y = this.y + y;

		targetLoc = LevelManager.grid.getWorldPosition(this.x, this.y) + LevelManager.grid.originPos;
		transform.position = targetLoc;

		RuleDecifer.adjust = true;

		return true;
	}


	public void clearMods() {
		isDefeat = false;
		isPull = false;
		isPush = true;
		isStop = false;
		isWeak = false;
		isWin = false;
		isYou = false;
	}

	public override void Delete() {
		return;
	}

	//You + Win or Defeat
	public virtual void checkWinOrDefeat() {

	}
}
