using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobAds : MonoBehaviour
{
    public static AdmobAds instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
