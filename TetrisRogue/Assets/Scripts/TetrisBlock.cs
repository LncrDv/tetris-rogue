using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime;
    public static int height = 20;
    public static int width = 10;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            transform.position += Vector3.left;
            if(!ValidMove())
                transform.position += Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right;
            if (!ValidMove())
                transform.position += Vector3.left;
        }
        MakePieceFall();
        
    }
    void MakePieceFall()
    {
        var newFallTime = fallTime;
        if (Input.GetKey(KeyCode.S))
        {
            newFallTime = fallTime / 10;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            newFallTime = 0;
        }
        if (Time.time - previousTime > newFallTime)
        {
            transform.position += Vector3.down;
            if (!ValidMove())
                transform.position += Vector3.up;
            previousTime = Time.time;
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
        }

        return true;
    }
}
