using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] Vector2Int[] cells;
    [SerializeField] Shapes shapeIndicator;
    public ShapesData shape;
    public Vector3Int position;
    public Board board;
    public bool stack;
    public void Initiate(Board boardN,ShapesData shapeN,Vector3Int startPosN)
    {
        StopAllCoroutines();
        this.board = boardN;
        this.shape = shapeN;
        position = startPosN;
        stack = false;
        StartCoroutine(MoveDown());
        cells = shape.cells;
        shapeIndicator = shape.shape;
        if(!board.CheckValid(shape.cells, position))
        {
            board.GameOver();
        }
    }

    private void Update()
    {
        if (!stack && board.active)
        {
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Vector3Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Vector3Int.left);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Rotate(-1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Rotate(1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SlamDown();
            }
            board.SetOnBoard(this);
        }
    }

    void SlamDown()
    {
        while (Move(Vector3Int.down))
        {
            continue;
        }
        board.SetOnBoard(this);
        board.CreateNewPiece(this);
    }

    IEnumerator MoveDown()
    {
        board.ClearFromBoard(this);
        while (!stack) 
        {
            if (board.active)
            {

                print("Move Down");
                board.ClearFromBoard(this);
                if (Move(Vector3Int.down))
                {
                    board.SetOnBoard(this);
                }
                else
                {
                    stack = true;
                    board.SetOnBoard(this);
                    board.CreateNewPiece(this);                
                }
                yield return new WaitForSeconds(board.deltaTime);
            }
        }
    }

    public bool Move(Vector3Int direction)
    {
        board.ClearFromBoard(this);
        Vector2Int[] newCells = new Vector2Int[shape.cells.Length];
        Vector3Int newPos = position + direction;
        bool valid = board.CheckValid(shape.cells,newPos);
        if (valid)
        {
            position = newPos;
        }
        return valid;
    }

    public void Rotate(int dir)
    {
        board.ClearFromBoard(this);
        Vector2Int[] newCells = shape.Rotate(dir);
        if (board.CheckValid(newCells,position))
        {
            cells = shape.cells;
            shape.cells = newCells;
        }
    }
}
