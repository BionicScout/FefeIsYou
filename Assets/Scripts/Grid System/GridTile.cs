using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class GridTile {
    GameObject tileHolder;

    GameObject textObject;
    TextMeshPro text;

	GameObject tileObject;
	Sprite[] spriteList;
	public int currentSprite = 0;

	//Constructor
	public GridTile(Vector3 pos, float cellSize, GameObject parent) {
        tileHolder = new GameObject("(" + (int)(pos.x / cellSize) + ", " + (int)(pos.y / cellSize) + ")");
        tileHolder.transform.SetParent(parent.transform, true);
    }

    //Text
    public void CreateText(Vector3 pos, float cellSize, Vector3 origin) {
        textObject = new GameObject("Text");
        textObject.transform.position = pos + origin;
        textObject.transform.position = new Vector3(textObject.transform.position.x, textObject.transform.position.y, 10);
        textObject.transform.SetParent(tileHolder.transform, true);

        textObject.AddComponent<TextMeshPro>();
        text = textObject.GetComponent<TextMeshPro>();
        text.text = "";
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 30; //36 for ###
        text.transform.localScale = new Vector3(0.2f * cellSize, 0.2f * cellSize, 0);
    }

    public void setFontSize(int fontSize) {
        text.fontSize = fontSize;
	}

    public int getFontSize() {
        return (int)text.fontSize;
	}

    public void setText(string text) {
        if (text == null) {
            setText("");
            return;
        }

        textObject.GetComponent<TextMeshPro>().text = text;
    }

    public string getText() {
        return text.text;
    }

//Sprite Functions
    public void CreateSprite(Vector3 pos, float cellSize, Vector3 origin, Sprite[] list) {
        spriteList = list;

        tileObject = new GameObject("Sprite");
        tileObject.transform.position = pos + origin;
        tileObject.transform.position = new Vector3(tileObject.transform.position.x, tileObject.transform.position.y, 10);
        tileObject.transform.localScale = tileObject.transform.localScale * cellSize;
        tileObject.transform.SetParent(tileHolder.transform, true);


        tileObject.AddComponent<SpriteRenderer>();
        tileObject.GetComponent<SpriteRenderer>().sprite = spriteList[currentSprite];
    }

    public void nextSpriteTile() {
        currentSprite++;

        if (currentSprite >= spriteList.Length) {
            currentSprite = 0;
        }

        tileObject.GetComponent<SpriteRenderer>().sprite = spriteList[currentSprite];
    }

    public int getSpriteIndex() {
        return currentSprite;
    }
}