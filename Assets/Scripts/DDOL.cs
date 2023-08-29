using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public static DDOL instance;

    public SelectedGrid selectedGrid;

    public SelectedMode selectedMode;

    public int CrossWinCount, NoughtWinCount;

    public bool isGameContinues;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
