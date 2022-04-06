using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAdder : MonoBehaviour {
    public Vector2Int[] startPos, endPos;
    public int[] prefabIndex;

    void Start() {
		for (int i = 0; i < startPos.Length; i++) {
            createLine(startPos[i], endPos[i], prefabIndex[i]);
		}
    }

	void createLine(Vector2Int start, Vector2Int end, int index) {
        for (int y = start.y; y <= end.y; y++) {
            for (int x = start.x; x <= end.x; x++) {
                GameObject temp = Object.Instantiate(LevelManager.instance.objectPrefabs[index]);
                temp.GetComponent<CharacterObject>().setPos(x, y);
            }
        }
    }
}
