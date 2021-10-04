using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using TMPro;
using UnityEditor;

using UnityEngine;
using UnityEngine.Experimental.AI;
public class PlayerMovement_GridBased : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Vector2 startPos; //Used to set the player's spawn point
    [SerializeField] private Grid worldGrid; //Reference to the grid gameobject that the player exists on
    [SerializeField] private List<Vector3> positionQue = new List<Vector3>(); //This is used to track the positions the player moves throughy
    public bool moving = false; //Used to say when the player is following the que
    private int moveIndex = 0;
    
    [Header("Player Settings")]
    [Range(0,1)][SerializeField] private float stepSpeed; //How fast we move between each point (1 = 1 second);


    private void Start() {
        transform.position = startPos; //Set our position to the start
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0) && !moving) { //When the player lifts the mouse button 
            GeneratePath(GridMousePos());
        }
    }

    //Gets the mouse position relative to the grid
    public Vector3Int GetGridPosition(Vector3 pos) {
        return worldGrid.WorldToCell(pos);
    }

    //Generates a path to the tile clicked
    public void GeneratePath(Vector3 pathDest) {
        if (!moving) {
            var currentGridPos = worldGrid.WorldToCell(transform.position);
        
            var horizontal = pathDest.x - currentGridPos.x;
            var vertical = pathDest.y - currentGridPos.y;

            for (int x = 0; x < Mathf.Abs(horizontal); x++) {
                if (horizontal < 0) {
                    //Left
                    currentGridPos = new Vector3Int(currentGridPos.x -= 1, currentGridPos.y, 0);
                    positionQue.Add(currentGridPos);
                } else {
                    //Right
                    currentGridPos = new Vector3Int(currentGridPos.x += 1, currentGridPos.y, 0);
                    positionQue.Add(currentGridPos);
                }
            }

            for (int y = 0; y < Mathf.Abs(vertical); y++) {
                if (vertical > 0) {
                    //Up
                    currentGridPos = new Vector3Int(currentGridPos.x, currentGridPos.y += 1, 0);
                    positionQue.Add(currentGridPos);
                } else {
                    //Down
                    currentGridPos = new Vector3Int(currentGridPos.x, currentGridPos.y -= 1, 0);
                    positionQue.Add(currentGridPos);
                }
            }
            
            Move(positionQue);
        }
    }

    //Get mouse position as a grid cell
    public Vector3 GridMousePos() {
        var temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); //Converts the mouse pos on screen to the world
        var zeroTemp = new Vector3(temp.x, temp.y, 0); //Sets the z of that to be 0 so its not the cameras z
        return worldGrid.WorldToCell(zeroTemp); //Return the mouse position to the world, but in the form of the grid values
    }

    //Call when you want the player to move to a position (using world co-ordinates)
    public void Move(Vector3 moveTo) {
        var newPos = new Vector3(moveTo.x, moveTo.y, 0); //Set the z value to 0
        // Debug.Log("Moving to: " + worldGrid.WorldToCell(newPos));
        transform.position = worldGrid.WorldToCell(newPos); //Move the player to the new position (translated from world space to grid space)
    }

    //Method used for moving through the que
    public void Move(List<Vector3> movePositions) {
        StartCoroutine(MoveThroughQue());
        moving = true;
    }

    //Runs through the que of positions one at a time
    IEnumerator MoveThroughQue() {
        //Freaky little self made loop
        if (moveIndex != positionQue.Count) {
            yield return new WaitForSeconds(stepSpeed);
            Move(positionQue[moveIndex]);
            moveIndex++;
            StartCoroutine(MoveThroughQue());
        }else if (moveIndex >= positionQue.Count) {
            positionQue.Clear(); //Clear the whole path (this is a better code than the one commented out below cause it clears at the end and not during)
            moving = false; //Also the player is no longer moving
            moveIndex = 0; //Reset the moveIndex
        }
    }

}
