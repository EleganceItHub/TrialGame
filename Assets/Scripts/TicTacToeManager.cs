using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
using UnityEngine.SceneManagement;

public class TicTacToeManager : MonoBehaviour
{
    public GameObject CrossObj, NoughtObject, GridBoard;

    public GameObject[] AllGridObj;

    public Turn CurrentTurn;

    public Color CrossColor, NoughtColor;

    public TextMeshProUGUI TurnText;

    public static TicTacToeManager Instance;

    public List<Turn> SelectedObject = new List<Turn>();

    public LineRenderer lineRenderer;

    public SelectedMode selectedMode;

    public TextMeshProUGUI WhoisWon;

    public GameObject WinPanel, AllGridObjPanel;



    public TextMeshProUGUI ScoreComapareBoardTxt;

    

    private void OnEnable()
    {
        DDOL.instance.isGameContinues = true;

    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        selectedMode = DDOL.instance.selectedMode;

        ScoreComapareBoardTxt.text = DDOL.instance.CrossWinCount + " : " + DDOL.instance.NoughtWinCount;

        GridBoard.GetComponent<SpriteRenderer>().color = CrossColor;

        TurnText.text = "Cross Player Turn";
        TurnText.color = CrossColor;

    }

    public void PutCrossOrNought(GameObject G)
    {
        //  Debug.Log(G.name);

        if (CurrentTurn == Turn.Cross)
        {
            GameObject G1 = Instantiate(CrossObj, G.transform.position, Quaternion.identity);

            SoundManager.instance.PlaySound(4);


            G1.transform.SetParent(AllGridObjPanel.transform);


            iTween.PunchScale(G1, new Vector3(1.2f, 1.2f, 1.2f), 1f);

            SelectedObject[G.GetComponent<OnBoxClick>().BoxNumber] = CurrentTurn;
            StartCoroutine( WinCondition());


            bool isDraw = DrawCheck();

            if (isDraw)
            {
                return;
            }
            CurrentTurn = Turn.Nought;
            // GridBoard.GetComponent<SpriteRenderer>().color = NoughtColor;

            StartCoroutine(ChangeGridColor(NoughtColor, "Nought Player Turn"));


        }
        else if (CurrentTurn == Turn.Nought)
        {
            GameObject G1 = Instantiate(NoughtObject, G.transform.position, Quaternion.identity);
            SoundManager.instance.PlaySound(4);


            G1.transform.SetParent(AllGridObjPanel.transform);

            iTween.PunchScale(G1, new Vector3(1.2f, 1.2f, 1.2f), 1f);
            SelectedObject[G.GetComponent<OnBoxClick>().BoxNumber] = CurrentTurn;

            StartCoroutine(WinCondition());

            bool isDraw = DrawCheck();

            if (isDraw)
            {
                return;
            }

            CurrentTurn = Turn.Cross;
            //  GridBoard.GetComponent<SpriteRenderer>().color = CrossColor;

            StartCoroutine(ChangeGridColor(CrossColor, "Cross Player Turn"));

        }

        if (selectedMode == SelectedMode.PlayerVsCPU)
        {
            //  StartCoroutine(CPUTurn());

            StartCoroutine(CPUTurnUsingBot());
        }



        G.GetComponent<BoxCollider2D>().enabled = false;


    }

    public IEnumerator CPUTurnUsingBot()
    {

        yield return new WaitForSeconds(1f);

        char[,] MyBoard = new char[3, 3];
        int counter = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (SelectedObject[counter] == Turn.none)
                {
                    MyBoard[i, j] = '_';
                }
                else if (SelectedObject[counter] == Turn.Cross)
                {
                    MyBoard[i, j] = 'o';
                }
                else if (SelectedObject[counter] == Turn.Nought)
                {
                    MyBoard[i, j] = 'x';
                }

                counter++;
            }
        }


        //char[,] board = {{ 'x', 'o', 'x' },
        //                { 'o', 'o', 'x' },
        //                { '_', '_', '_' }};

        Move bestMove = findBestMove(MyBoard);

        Debug.Log("The Optimal Move is :\n");

        Debug.Log("ROW :: " + bestMove.row + "  COL :: " + bestMove.col);

        int counter1 = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == bestMove.row && j == bestMove.col)
                {
                    Debug.Log("<color=yellow>" + counter1 + "</color>");

                    GameObject G1 = Instantiate(NoughtObject, AllGridObj[counter1].transform.position, Quaternion.identity);
                    SoundManager.instance.PlaySound(4);

                    AllGridObj[counter1].GetComponent<BoxCollider2D>().enabled = false;

                    G1.transform.SetParent(AllGridObjPanel.transform);

                    iTween.PunchScale(G1, new Vector3(1.2f, 1.2f, 1.2f), 1f);
                    SelectedObject[AllGridObj[counter1].GetComponent<OnBoxClick>().BoxNumber] = CurrentTurn;

                    StartCoroutine(WinCondition());

                    bool isDraw = DrawCheck();

                    if (!isDraw)
                    {
                        CurrentTurn = Turn.Cross;
                        //  GridBoard.GetComponent<SpriteRenderer>().color = CrossColor;

                        StartCoroutine(ChangeGridColor(CrossColor, "Cross Player Turn"));
                    }


                }
                counter1++;
            }
        }
    }

    public IEnumerator CPUTurn()
    {
        yield return new WaitForSeconds(1);

        List<int> AllPos = new List<int>();

        Debug.Log("CPU Turn");

        for (int i = 0; i < SelectedObject.Count; i++)
        {
            if (SelectedObject[i] == Turn.none)
            {
                // Debug.Log(i);
                AllPos.Add(i);
            }
        }

        int R = Random.Range(0, AllPos.Count);


        int RandomValue = AllPos[R];

        Debug.Log(R + "::" + RandomValue);

        // AllGridObj[RandomValue] 

        GameObject G1 = Instantiate(NoughtObject, AllGridObj[RandomValue].transform.position, Quaternion.identity);

        SoundManager.instance.PlaySound(4);

        G1.transform.SetParent(AllGridObjPanel.transform);


        iTween.PunchScale(G1, new Vector3(1.2f, 1.2f, 1.2f), 1f);
        SelectedObject[AllGridObj[RandomValue].GetComponent<OnBoxClick>().BoxNumber] = CurrentTurn;

        StartCoroutine(WinCondition());

        bool isDraw = DrawCheck();

        if (!isDraw)
        {
            CurrentTurn = Turn.Cross;
            //  GridBoard.GetComponent<SpriteRenderer>().color = CrossColor;

            StartCoroutine(ChangeGridColor(CrossColor, "Cross Player Turn"));
        }



    }

    IEnumerator ChangeGridColor(Color color, string TurnTxt)
    {
        yield return new WaitForSeconds(0.5f);
        GridBoard.GetComponent<SpriteRenderer>().color = color;

        TurnText.color = color;

        if (selectedMode == SelectedMode.PlayerVsCPU && CurrentTurn == Turn.Nought)
        {
            TurnText.text = "CPU Turn";
        }
        else
        {
            TurnText.text = TurnTxt;
        }

    }

    public bool DrawCheck()
    {
        int counter = 0;

        bool isDraw = false;

        for (int i = 0; i < SelectedObject.Count; i++)
        {
            if (SelectedObject[i] == Turn.none)
            {
                counter++;
            }
        }

        if (counter == 0)
        {
            Debug.Log("Draw Game");

            StartCoroutine(ChangeScene());


            isDraw = true;
        }

        return isDraw;
    }

    IEnumerator ChangeScene()
    {
        SoundManager.instance.PlaySound(0);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator WinCondition()
    {
        int[,] AllWinCondition = new int[8, 3] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };

        bool isWinDeclare = false;

        for (int i = 0; i < 8; i++)
        {
            if (CurrentTurn == SelectedObject[AllWinCondition[i, 0]] &&
                CurrentTurn == SelectedObject[AllWinCondition[i, 1]] &&
                CurrentTurn == SelectedObject[AllWinCondition[i, 2]])
            {
                Debug.Log("Yes Win");

                SoundManager.instance.PlaySound(1);


                lineRenderer.positionCount = 3;

                lineRenderer.SetPosition(0, AllGridObj[AllWinCondition[i, 0]].transform.position);
                lineRenderer.SetPosition(1, AllGridObj[AllWinCondition[i, 1]].transform.position);
                lineRenderer.SetPosition(2, AllGridObj[AllWinCondition[i, 2]].transform.position);

                yield return new WaitForSeconds(1f);

                if (CurrentTurn == Turn.Cross)
                {
                    


                    DDOL.instance.NoughtWinCount += 1;
                    ScoreComapareBoardTxt.text = DDOL.instance.CrossWinCount + " : " + DDOL.instance.NoughtWinCount;

                   

                    if (DDOL.instance.NoughtWinCount == 3)
                    {
                        Debug.Log("Win Panel Open");

                        WinPanel.SetActive(true);

                        UnityAds.instance.ShowInterstitial();


                        AllGridObjPanel.SetActive(false);

                        isWinDeclare = true;

                    }
                }
                else if (CurrentTurn == Turn.Nought)
                {
                    DDOL.instance.CrossWinCount += 1;
                    ScoreComapareBoardTxt.text = DDOL.instance.CrossWinCount + " : " + DDOL.instance.NoughtWinCount;

                    if (DDOL.instance.CrossWinCount == 3)
                    {
                        Debug.Log("Win Panel Open");

                        UnityAds.instance.ShowInterstitial();


                        WinPanel.SetActive(true);
                        AllGridObjPanel.SetActive(false);

                       isWinDeclare = true;
                    }
                }
                else
                {
                    Debug.Log("Draw");
                    SoundManager.instance.PlaySound(2);

                }

                if (isWinDeclare == false)
                {
                    StartCoroutine(ChangeScene());
                }

              

            }


        }
    }

    public void BackBtn()
    {
        SoundManager.instance.PlaySound(0);

        Initiate.Fade("TicTacToe_Index", Color.white, 1f);
    }

    // MinMax

    static char player = 'x', opponent = 'o';

    // This function returns true if there are moves
    // remaining on the board. It returns false if
    // there are no moves left to play.
    static bool isMovesLeft(char[,] board)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == '_')
                    return true;
        return false;
    }

    // This is the evaluation function as discussed
    // in the previous article ( http://goo.gl/sJgv68 )
    static int evaluate(char[,] b)
    {
        // Checking for Rows for X or O victory.
        for (int row = 0; row < 3; row++)
        {
            if (b[row, 0] == b[row, 1] &&
                b[row, 1] == b[row, 2])
            {
                if (b[row, 0] == player)
                    return +10;
                else if (b[row, 0] == opponent)
                    return -10;
            }
        }

        // Checking for Columns for X or O victory.
        for (int col = 0; col < 3; col++)
        {
            if (b[0, col] == b[1, col] &&
                b[1, col] == b[2, col])
            {
                if (b[0, col] == player)
                    return +10;

                else if (b[0, col] == opponent)
                    return -10;
            }
        }

        // Checking for Diagonals for X or O victory.
        if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
        {
            if (b[0, 0] == player)
                return +10;
            else if (b[0, 0] == opponent)
                return -10;
        }

        if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
        {
            if (b[0, 2] == player)
                return +10;
            else if (b[0, 2] == opponent)
                return -10;
        }

        // Else if none of them have won then return 0
        return 0;
    }

    // This is the minimax function. It considers all
    // the possible ways the game can go and returns
    // the value of the board
    static int minimax(char[,] board,
                    int depth, bool isMax)
    {
        int score = evaluate(board);

        // If Maximizer has won the game
        // return his/her evaluated score
        if (score == 10)
            return score;

        // If Minimizer has won the game
        // return his/her evaluated score
        if (score == -10)
            return score;

        // If there are no more moves and
        // no winner then it is a tie
        if (isMovesLeft(board) == false)
            return 0;

        // If this maximizer's move
        if (isMax)
        {
            int best = -1000;

            // Traverse all cells
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (board[i, j] == '_')
                    {
                        // Make the move
                        board[i, j] = player;

                        // Call minimax recursively and choose
                        // the maximum value
                        best = Mathf.Max(best, minimax(board,
                                        depth + 1, !isMax));

                        // Undo the move
                        board[i, j] = '_';
                    }
                }
            }
            return best;
        }

        // If this minimizer's move
        else
        {
            int best = 1000;

            // Traverse all cells
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (board[i, j] == '_')
                    {
                        // Make the move
                        board[i, j] = opponent;

                        // Call minimax recursively and choose
                        // the minimum value
                        best = Mathf.Min(best, minimax(board,
                                        depth + 1, !isMax));

                        // Undo the move
                        board[i, j] = '_';
                    }
                }
            }
            return best;
        }
    }

    // This will return the best possible
    // move for the player
    static Move findBestMove(char[,] board)
    {
        int bestVal = -1000;
        Move bestMove = new Move();
        bestMove.row = -1;
        bestMove.col = -1;

        // Traverse all cells, evaluate minimax function
        // for all empty cells. And return the cell
        // with optimal value.
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Check if cell is empty
                if (board[i, j] == '_')
                {
                    // Make the move
                    board[i, j] = player;

                    // compute evaluation function for this
                    // move.
                    int moveVal = minimax(board, 0, false);

                    // Undo the move
                    board[i, j] = '_';

                    // If the value of the current move is
                    // more than the best value, then update
                    // best/
                    if (moveVal > bestVal)
                    {
                        bestMove.row = i;
                        bestMove.col = j;
                        bestVal = moveVal;
                    }
                }
            }
        }

        Debug.Log("The value of the best Move " +
                             "is : {0}\n\n" + bestVal);

        return bestMove;
    }

    public void HomeBtn()
    {

        StartCoroutine(ChangeScene());
    }

    public void NextBtn()
    {
        DDOL.instance.NoughtWinCount = 0;
        DDOL.instance.CrossWinCount = 0;
        ScoreComapareBoardTxt.text = DDOL.instance.CrossWinCount + " : " + DDOL.instance.NoughtWinCount;

        StartCoroutine(ChangeScene());
    }


}

/// <summary>
/// Algorithm MinMax
/// </summary>
/// 



class Move
{
    public int row, col;
};


//// Driver code
//public static void Main(String[] args)
//{
//    char[,] board = {{ 'x', 'o', 'x' },
//                { 'o', 'o', 'x' },
//                { '_', '_', '_' }};

//    Move bestMove = findBestMove(board);

//    Console.Write("The Optimal Move is :\n");
//    Console.Write("ROW: {0} COL: {1}\n\n",
//            bestMove.row, bestMove.col);
//}




public enum Turn
{
    none,
    Cross,
    Nought
}
