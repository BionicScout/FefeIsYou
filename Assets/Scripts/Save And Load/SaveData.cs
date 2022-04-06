using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
//Grid Data
    public int width, height;
    public float cellSize;
    public float[] originPos = new float[3];

    public GridTileInfo[,] grid;

//Object Data
    public string[] name;
    public int[] x;
    public int[] y;
    public string[] prefabName;

    public void setOrigin(Vector3 origin) {
        originPos[0] = origin.x;
        originPos[1] = origin.y;
        originPos[2] = origin.z;
    }

    public Vector3 getOrigin() {
        return new Vector3(originPos[0], originPos[1], originPos[2]);
    }

    public void setGrid(GridTile[,] tiles) {
        grid = new GridTileInfo[width, height];

        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                grid[x, y] = new GridTileInfo(tiles[x,y].getText(), tiles[x, y].getFontSize());                
            }
        }
    }

    public GridTileInfo[,] getGrid() {
        return grid;
    }
}
