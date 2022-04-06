using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public int width, height;
    public float cellSize;
    public Vector3 originPos;

    public GridTile[,] grid;

    GameObject gridTileHolder;

    //Constructors
    public Grid(int width, int height, float cellSize, Vector3 origin, Sprite[] spriteList) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        originPos = origin;

        grid = new GridTile[width, height];

        gridTileHolder = new GameObject("Grid");

        //Draw Lines
        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                newGridTile(x, y);
            }
        }
    }

    public Grid(int width, int height, float cellSize, Vector3 origin) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        originPos = origin;

        grid = new GridTile[width, height];

        gridTileHolder = new GameObject("Grid");
    }

    public void newGridTile(int x, int y/*, int value, Sprite[] spriteList, int currentSprite, int rotations*/) {
        grid[x, y] = new GridTile(new Vector3(x + .5f, y + .5f, 0) * cellSize, cellSize, gridTileHolder);
    }

    public Grid(int width, int height, float cellSize, Vector3 origin, string[,] text, Sprite[] spriteList) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        originPos = origin;

        grid = new GridTile[width, height];

        gridTileHolder = new GameObject("Grid");

        //Draw Lines
        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                newGridTile(x, y);

                grid[x, y].CreateText(getWorldPosition(x, y), cellSize, origin);
                
                grid[x, y].CreateSprite(getWorldPosition(x, y), cellSize, origin, spriteList);
            }
        }
    }

	public Grid(SaveData levelInfo) {
		width = levelInfo.width;
		height = levelInfo.height;
		cellSize = levelInfo.cellSize;
		originPos = levelInfo.getOrigin();

        grid = new GridTile[width, height];

        gridTileHolder = new GameObject("Grid");

        GridTileInfo[,] tileInfo = levelInfo.getGrid();

        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                newGridTile(x, y);
            }
        }
    }

	//World Pos <---> Grid Pos
	public Vector3 getWorldPosition(float x, float y) {
        return new Vector3(x, y) * cellSize + originPos;
    }

    public void getGridPosition(Vector3 worldPos, out int x, out int y) {
        x = Mathf.FloorToInt(((worldPos - originPos).x / cellSize) + .5f);
        y = Mathf.FloorToInt(((worldPos - originPos).y / cellSize) + .5f);
    }
/*
//Text
    public void setFontSize(int fontSize) {
        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                grid[x, y].setFontSize(fontSize);
            }
        }
    }

    //Tile Functions
    public void nextTile(int x, int y) {
        if (x < 0 || x > width || y < 0 || y > height) {
            Debug.LogError("Invalid x or y in Grid\nX: " + x + "\tY: " + y);
            return;
        }

        grid[x, y].nextSpriteTile();
    }

    public void nextTile(Vector3 worldPos) {
        int x, y;
        getGridPosition(worldPos, out x, out y);
        nextTile(x, y);
    }
*/
    //Get Text Array
    public string[,] getTextArray() {
        string[,] textArr = new string[width, height];

        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                textArr[x, y] = "";
            }
        }

        foreach (CharacterObject co in CharacterObjectList.textList) {
            textArr[co.x, co.y] = co.name;
		}

        return textArr;
	}
}
