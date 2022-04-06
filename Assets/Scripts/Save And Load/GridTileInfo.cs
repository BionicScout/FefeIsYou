using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridTileInfo {
    public string text;
    public int fontSize;

    public GridTileInfo(string text, int fontSize) {
        this.text = text;
        this.fontSize = fontSize;
	}
}
