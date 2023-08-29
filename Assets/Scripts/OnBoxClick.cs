using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoxClick : MonoBehaviour
{
    public int BoxNumber;

    private void OnMouseDown()
    {
      //  Debug.Log("Mouse Down");

        TicTacToeManager.Instance.PutCrossOrNought(this.gameObject);
    }
}
