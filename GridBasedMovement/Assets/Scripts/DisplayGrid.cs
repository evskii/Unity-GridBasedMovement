using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGrid : MonoBehaviour
{
    public int width, height; //How wide and tall we will make our grid
    public GameObject gridSprite; //Prefab to spawn in that shows the grid

    private void Start() {
        GenerateGrid();
    }

    private void GenerateGrid() {
        //Used so the grid doesnt start in the world 0,0,0
        var startX = -width / 2;
        var startY = -height / 2;
        //Nested for loop to get the width and height
        for (int w = 0; w < width; w++) {
            for (int h = 0; h < height; h++) {
                var pos = new Vector2(startX + w, startY + h);
                Instantiate(gridSprite, pos, Quaternion.identity, transform); //Create the grid cells with the prefab provided.
            }
        }
    }
}
