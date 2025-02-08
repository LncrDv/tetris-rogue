using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    public GameObject[] tetrominoes;

    private void Start()
    {
        NewTetromino();
    }
    public void NewTetromino()
    {
        Instantiate
            (
            tetrominoes[Random.Range(0, tetrominoes.Length)],
            transform.position,
            Quaternion.identity
            );
    }
}
