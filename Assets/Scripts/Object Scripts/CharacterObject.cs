using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour {
	public string name;
	public int x, y;

	protected Vector3 targetLoc;
	

	[HideInInspector]
	public string hasObject = "NONE";

	//[HideInInspector]
	public bool isDefeat;
	//[HideInInspector]
	public bool isPull;
	//[HideInInspector]
	public bool isPush;
	//[HideInInspector]
	public bool isStop;
	//[HideInInspector]
	public bool isWeak;
	//[HideInInspector]
	public bool isWin;
	//[HideInInspector]
	public bool isYou;

	private void Start() {
		customStart();
	}

	protected virtual void customStart() {
		CharacterObjectList.list.Add(this);
		transform.position = LevelManager.grid.getWorldPosition(x, y) + LevelManager.grid.originPos;
	}


	//Move
	public virtual bool move(int x, int y) {
		//Debug.Log("Name: " + name + "\tY: " + index);

		if (LevelManager.instance.won || LevelManager.instance.paused || LevelManager.instance.lossedBool) {
			return false;
		}

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

		targetLoc = LevelManager.grid.getWorldPosition(this.x , this.y) + LevelManager.grid.originPos;
		transform.position = targetLoc;
		AudioManager.instance.Play("Move");

		return true;
	}

	protected bool checkNextTile(int nextX, int nextY) {
	//Normal Object
		foreach (CharacterObject co in CharacterObjectList.list) {
			if (co.x == nextX && co.y == nextY && co != this) {
				if (co.isWin)
					LevelManager.instance.win();
				if (co.isDefeat) {
					CharacterObjectList.youDelete.Add(this);
					CharacterObjectList.youDelete.Add(co);
					AudioManager.instance.Play("Defeat");
				}

				if (co.isPush) {
					return co.move(nextX - x, nextY - y);
				}

			//Stop + Weak interactions
				if (co.isStop && co.isWeak) {
					CharacterObjectList.youDelete.Add(co);
					return true;
				}

				if (co.isStop && !isWeak) {
					return false;
				}

				if (co.isStop && isWeak) {
					CharacterObjectList.youDelete.Add(this);
					CharacterObjectList.youDelete.Add(co);
					return false;
				}
			}
		}

	//Text Obj
		foreach (CharacterObject co in CharacterObjectList.textList) {
			if (co.x == nextX && co.y == nextY && co != this && co.isPush) {
				return co.move(nextX - x, nextY - y);
			}
		}

		return true;
	}

	public void setPos(int tempX, int tempY) {
		x = tempX;
		y = tempY;
		transform.position = LevelManager.grid.getWorldPosition(x, y) + LevelManager.grid.originPos;
	}

	//Rules
	public void clearHas() {
		hasObject = "NONE";
	}

	public void clearMods() {
		isDefeat = false;
		isPull = false;
		isPush = false;
		isStop = false;
		isWeak = false;
		isWin = false;
		isYou = false;
	}

	public GameObject getObject() {
		return this.gameObject;
	}

	public virtual void Delete() {
		if (hasObject != "NONE") {
			Debug.Log(hasObject);
			//Copy Info
			Vector3 pos = transform.position;


			//Put In Info
			GameObject temp = Object.Instantiate(LevelManager.instance.findPrefab(hasObject));
			temp.transform.position = pos;
			temp.GetComponent<CharacterObject>();
		}

		if (isYou) 
			CharacterObjectList.youList.Remove(this);

		CharacterObjectList.list.Remove(this);
		Destroy(this.gameObject);
	}

//You + Win or Defeat
	public virtual void checkWinOrDefeat() {
		if (isYou && isWin)
			LevelManager.instance.win();

		if (isYou && isDefeat) {
			CharacterObjectList.youDelete.Add(this);
			AudioManager.instance.Play("Defeat");
		}
	}
}
