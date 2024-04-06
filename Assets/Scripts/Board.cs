
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] TMP_Text lineDownIndicator;
    public int lineDown;
    
    public float deltaTime;

    [SerializeField] Button nextLevelButton;
    [SerializeField] TMP_Text lvlIndicator, NLWScoreIndicator;
    [SerializeField] GameObject nextLevelWindow;
    public bool active;
    int lvl;

    public Tilemap board;
    public Piece activePiece;
    public Vector3Int startPos;
    public Vector2Int borderSize;

    public RectInt border
    {
        get
        {
            Vector2Int pos = new Vector2Int(-borderSize.x/2,-borderSize.y/2);
            return new RectInt(pos, borderSize);
        }
    }

    private void Awake()
    {
        lineDown = 0;
        board = GetComponentInChildren<Tilemap>();
        int r = Random.Range(0, 2);
        activePiece.Initiate(this, ShapesData.AvailableShapes[r], startPos);
    }

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        nextLevelWindow.SetActive(!false);
        lvl = 1;
        InvokeRepeating(nameof(NextLevel), 180, 180);
        active = true;
        lineDownIndicator.text = "Kines Down: " + lineDown;
        SetOnBoard(activePiece);
    }


    public bool CheckValid(Vector2Int[] newCells,Vector3Int position)
    {
        for (int i = 0;i<newCells.Length;i++)
        {
            if (!border.Contains(newCells[i] + (Vector2Int)position))
            {
                return false;
            }
            if (board.HasTile((Vector3Int)newCells[i] + position))
            {
                return false;
            }
        }
        return true;
    }

    public void SetOnBoard(Piece piece)
    {
        for(int i = 0; i < piece.shape.cells.Length; i++)
        {
            board.SetTile((Vector3Int)piece.shape.cells[i] + piece.position,piece.shape.tile);
        }
    }

    public void ClearFromBoard(Piece piece)
    {
        for (int i = 0; i < piece.shape.cells.Length; i++)
        {
            board.SetTile((Vector3Int)piece.shape.cells[i] + piece.position, null);
        }
    }

    public void CreateNewPiece(Piece piece)
    {
        CheckLines(piece);
        int r = Random.Range(0, 2);
        piece.Initiate(this, ShapesData.AvailableShapes[r], startPos);
    }

    public void CheckLines(Piece piece)
    {
        int startLineDowns = lineDown;
        int rowLength = border.x + border.width;
        int currY = 100, cleared = 0;
        for(int i = 0;i < piece.shape.cells.Length;i++)
        {
            int y = piece.shape.cells[i].y + piece.position.y;
            int rowCount = 0;
            for(int j = 0;j < rowLength;j++)
            {
                if(board.HasTile(new Vector3Int(j + border.x, y, 0)))
                {
                    rowCount++;
                }
            }
            if(rowCount == rowLength && currY != y) 
            {
                cleared++;
                currY = y< currY ? y : currY;
                ClearRow(y);     
            }
        }
        for(int i = 0;i < cleared;i++)
        {
            LowerRows(currY);
        }
        
    }

    void ClearRow(int y)
    {
        lineDown++;
        lineDownIndicator.text = "Lines Down: " + lineDown;
        print("Cleared Line At " +  y);
        int rowLength = border.x + border.width;
        for (int i = border.x; i <= rowLength; i++)
        {
            print("Clear at " +  i + "," + y);
            board.SetTile(new Vector3Int(i, y, 0), null);
        }
    }

    void LowerRows(int y)
    {
        
        int colHeight = border.y + border.height; 
        int rowLength = border.x + border.width;
        int startY = y > border.y ? y : border.y + 1;
        for(int r = border.x; r < rowLength; r++)
        {
            for(int c = startY; c < colHeight; c++)
            {
                TileBase temp = board.GetTile(new Vector3Int(r, c, 0));
                if(temp != null)
                {
                    print("Lower Row from " + c +" to "+ (c-1));
                    board.SetTile(new Vector3Int(r, c, 0), null);
                    board.SetTile(new Vector3Int(r, c - 1, 0), temp);
                }
            }
        }
    }

    public void NextLevel()
    {
        active = false;
        deltaTime = Mathf.Clamp(deltaTime - 0.1f,0.5f,1.5f);
        lvlIndicator.text = "Cleard " + lineDown + " Rows";
        NLWScoreIndicator.text = "Finished Level: " + lvl;
        nextLevelWindow.SetActive(true);
    }

    public void AdvanceLevel()
    {
        nextLevelWindow.SetActive(!false);
        active = true;
        lvl++;
    }

    public void GameOver()
    {
        NLWScoreIndicator.text = "Finished with Score:" + lineDown;
        lvlIndicator.text = "Finished till level " + lvl;
        nextLevelButton.gameObject.SetActive(false);
        nextLevelWindow.SetActive(true);
        StopAllCoroutines();

    }

    IEnumerator StartOver()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
