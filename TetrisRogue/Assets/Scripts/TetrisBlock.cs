using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;

    private float previousTime;
    public float fallTime;

    public bool isPlacedDown = false;

    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0) Destroy(this.gameObject);
        if (!isPlacedDown)
        {
            #region Move Piece
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.position += Vector3.left;
                if (!ValidMove())
                    transform.position += Vector3.right;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position += Vector3.right;
                if (!ValidMove())
                    transform.position += Vector3.left;
            }
            MakePieceFall();
            #endregion
            #region Rotate Piece
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90f);
                if (!ValidMove())
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90f);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90f);
                if (!ValidMove())
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90f);
            }
            #endregion
        }
    }
    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            grid[roundedX, roundedY] = child;
        }
    }
    void MakePieceFall()
    {
        var newFallTime = fallTime;
        if (Input.GetKey(KeyCode.S))
        {
            newFallTime = fallTime / 10;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            while(ValidMove())
            {
                transform.position += Vector3.down;
            }
            transform.position += Vector3.up;
            AddToGrid();
            CheckForLines();

            isPlacedDown = true;
            FindObjectOfType<TetrominoSpawner>().NewTetromino();
        }
        if (Time.time - previousTime > newFallTime)
        {
            transform.position += Vector3.down;
            if (!ValidMove())
            {
                transform.position += Vector3.up;
                AddToGrid();
                CheckForLines();

                isPlacedDown = true;
                FindObjectOfType<TetrominoSpawner>().NewTetromino();
            }
                
            previousTime = Time.time;
        }
    }

    void CheckForLines()
    {
        for(int i = height-1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int yPos)
    {
        //Check for every square(#xPos) in the line #yPos, and if one is blank, return false
        for(int xPos=0; xPos<width; xPos++)
        {
            if(grid[xPos,yPos]  == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int yPos)
    {
        //Check for every square(#xPos) in the line #yPos, and if one is blank, return false
        for (int xPos = 0; xPos < width; xPos++)
        {
            Destroy(grid[xPos, yPos].gameObject);
            grid[xPos, yPos] = null;
            
        }
        
    }
    void RowDown(int i)
    {
        //For every line <= removed line
        for(int yPos = i; yPos < height; yPos++)
        {
            //For every cube in row
            for(int xPos=0; xPos < width; xPos++)
            {
                //If row under isn't empty
                if (grid[xPos, yPos] != null)
                {
                    //Set cube under to current cube
                    grid[xPos, yPos -1] = grid[xPos, yPos];
                    //Set current cube to null(to get replaced later)
                    grid[xPos, yPos] = null;
                    //Move real cube in board down one
                    grid[xPos, yPos -1].transform.position += Vector3.down;
                }
            }
        }
    }
    bool ValidMove()
    {
        foreach(Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }
            if (grid[roundedX,roundedY] != null)
            {
                return false;
            }
        }

        return true;
    }
}
