using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GameName, PlayBtn, SettingBtn, BackToHomeBtn, ModeTxt, ThreeVsThreeBtn, FiveVsFiveBtn, SevenVsSevenBtn, BackBtnToMode, GameModeTxt, PlayerVsPlayerBtn, PlayervsCPUBtn,SettingPanel, SettingPopUp;
    public GameObject HomePanel, ModePanel, SelectedPanel;
    public string scene;
    public Color loadToColor = Color.white;

    public Image SoundImage;

    public Sprite SoundOn, SoundOff;

    public Image MusicImage;

    public Sprite MusicOn, MusicOff;

    public bool isSound, isMusic;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Music") == false)
        {
            PlayerPrefs.SetFloat("Music", 0.4f);
        }
        else
        {
            PlayerPrefs.GetFloat("Music");
        }

        if (PlayerPrefs.HasKey("Sound") == false)
        {
            PlayerPrefs.SetFloat("Sound", 1);
        }
        else
        {
            PlayerPrefs.GetFloat("Sound");
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Music") == 0)
        {
            isMusic = false;
            MusicImage.sprite = MusicOff;
            MusicManager.instance.MusicAudioSource.volume = 0;
        }
        else
        {
            isMusic = true;
            MusicImage.sprite = MusicOn;
            MusicManager.instance.MusicAudioSource.volume = 0.4f;
            PlayerPrefs.SetFloat("Music", 0.4f);
        }


        if (PlayerPrefs.GetFloat("Sound") == 0)
        {
            isSound = false;
            SoundImage.sprite = SoundOff;
            SoundManager.instance.SoundAudioSource.volume = 0;
            
        }
        else
        {
            isSound = true;
            SoundImage.sprite = SoundOn;
            SoundManager.instance.SoundAudioSource.volume = 1;
        }


        //	iTween.MoveTo(GameName, iTween.Hash("y", 500, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
            iTween.MoveTo(GameName, iTween.Hash("y", 1500, "easeType", "spring", "delay", .1));
            iTween.MoveTo(PlayBtn, iTween.Hash("x", 540, "easeType", "spring", "delay", .1));
            iTween.MoveTo(SettingBtn, iTween.Hash("x", 540, "easeType", "spring", "delay", .1));
            iTween.MoveTo(BackToHomeBtn, iTween.Hash("x", 50, "easeType", "spring", "delay", .1));
            iTween.MoveTo(ModeTxt, iTween.Hash("y", 1890, "easeType", "spring", "delay", .1));
            iTween.MoveTo(ThreeVsThreeBtn, iTween.Hash("x", 540, "easeType", "spring", "delay", .1));
            iTween.MoveTo(FiveVsFiveBtn, iTween.Hash("x", 540, "easeType", "spring", "delay", .1));
            iTween.MoveTo(SevenVsSevenBtn, iTween.Hash("y", 460, "easeType", "spring", "delay", .1));
            iTween.MoveTo(BackBtnToMode, iTween.Hash("x", 50, "easeType", "spring", "delay", .1));
            iTween.MoveTo(GameModeTxt, iTween.Hash("y", 1890, "easeType", "spring", "delay", .1));
            iTween.MoveTo(PlayerVsPlayerBtn, iTween.Hash("x", 540, "easeType", "spring", "delay", .1));
            iTween.MoveTo(PlayervsCPUBtn, iTween.Hash("x", 540, "easeType", "spring", "delay", .1));


        if (DDOL.instance.isGameContinues == true)
        {
            PlayBtnClick();
            DDOL.instance.NoughtWinCount = 0;
            DDOL.instance.CrossWinCount = 0;
          
        }

        PlayBtn.GetComponent<Button>().onClick.AddListener(PlayBtnClick);
        BackToHomeBtn.GetComponent<Button>().onClick.AddListener(BackToHomeClick);

        PlayerVsPlayerBtn.GetComponent<Button>().onClick.AddListener(PlayerVsPlayerSelect);
        PlayervsCPUBtn.GetComponent<Button>().onClick.AddListener(PlayerVsCPUSelect);

        //ThreeVsThreeBtn.GetComponent<Button>().onClick.AddListener(ThreeVsThreeBtnSelect);
        //FiveVsFiveBtn.GetComponent<Button>().onClick.AddListener(FiveVsFiveBtnSelect);
        //SevenVsSevenBtn.GetComponent<Button>().onClick.AddListener(SevenVsSevenBtnSelect);

        ThreeVsThreeBtn.GetComponent<Button>().onClick.AddListener(() => ThreeVsThreeBtnSelect(SelectedGrid.Three));
        FiveVsFiveBtn.GetComponent<Button>().onClick.AddListener(() => ThreeVsThreeBtnSelect(SelectedGrid.Five));
        SevenVsSevenBtn.GetComponent<Button>().onClick.AddListener(() => ThreeVsThreeBtnSelect(SelectedGrid.Seven));




    }


    public void PlayBtnClick()
    {
        SoundManager.instance.PlaySound(0);
        ModePanel.SetActive(true);
        HomePanel.SetActive(false);
    }

    public void ThreeVsThreeBtnSelect(SelectedGrid selectedGrid)
    {
        SoundManager.instance.PlaySound(0);

        DDOL.instance.selectedGrid = selectedGrid;
        SelectedPanel.SetActive(true);
        ModePanel.SetActive(false);
    }

    //public void FiveVsFiveBtnSelect()
    //{
    //    DDOL.instance.selectedGrid = SelectedGrid.Five;
    //    SelectedPanel.SetActive(true);
    //    ModePanel.SetActive(false);
    //}

    //public void SevenVsSevenBtnSelect()
    //{
    //    DDOL.instance.selectedGrid = SelectedGrid.Seven;
    //    SelectedPanel.SetActive(true);
    //    ModePanel.SetActive(false);
    //}

    public void PlayerVsPlayerSelect()
    {
        SoundManager.instance.PlaySound(0);

        DDOL.instance.selectedMode = SelectedMode.PlayerVsPlayer;

        Initiate.Fade(scene, loadToColor, 1f);
    }

    public void PlayerVsCPUSelect()
    {
        SoundManager.instance.PlaySound(0);

        DDOL.instance.selectedMode = SelectedMode.PlayerVsCPU;

        Initiate.Fade(scene, loadToColor, 1f);
    }

    public void BackToHomeClick()
    {
        SoundManager.instance.PlaySound(0);

        ModePanel.SetActive(false);
        HomePanel.SetActive(true);
        iTween.MoveTo(GameName, iTween.Hash("y", 1500, "easeType", "spring", "delay", .1));
        iTween.MoveTo(PlayBtn, iTween.Hash("x", 540, "easeType", "spring", "delay", .1));
        iTween.MoveTo(SettingBtn, iTween.Hash("x", 540, "easeType", "spring", "delay", .1));
    }

    public void SettingBtnClick()
    {
        SoundManager.instance.PlaySound(0);

        SettingPanel.SetActive(true);


        iTween.ScaleTo(SettingPopUp,new Vector3(1,1,1),1f);
    }

    public void CloseBtn()
    {
        SoundManager.instance.PlaySound(0);

        iTween.ScaleTo(SettingPopUp, new Vector3(0, 0, 0), 0.7f);

        Invoke("CloseBtnInvoke", 0.5f);

    }

    void CloseBtnInvoke() 
    {
       

        SettingPanel.SetActive(false);
    }

    public void Privacy()
    {
        SoundManager.instance.PlaySound(0);

        Application.OpenURL("https://minirogstudio.blogspot.com/2023/01/privacy-policy.html");
    }

    public void ResetGame()
    {
        SoundManager.instance.PlaySound(0);

        PlayerPrefs.DeleteAll();
    }

    public void SoundBtn()
    {
        if (isSound == true)
        {
            isSound = false;
            SoundImage.sprite = SoundOff;
            SoundManager.instance.SoundAudioSource.volume = 0;
            PlayerPrefs.SetFloat("Sound", 0);
        }
        else
        {
            isSound = true;
            SoundImage.sprite = SoundOn;
            SoundManager.instance.SoundAudioSource.volume = 1;
            PlayerPrefs.SetFloat("Sound", 1);

        }
    }

    public void MusicBtn()
    {
        if (isMusic == true)
        {
            isMusic = false;
            MusicImage.sprite = MusicOff;
            MusicManager.instance.MusicAudioSource.volume = 0;

            PlayerPrefs.SetFloat("Music", 0);
        }
        else
        {
            isMusic = true;
            MusicImage.sprite = MusicOn;
            MusicManager.instance.MusicAudioSource.volume = 0.4f;
            PlayerPrefs.SetFloat("Music", 0.4f);


        }
    }

    
}

public enum SelectedGrid
{
    none,
    Three,
    Five,
    Seven
}

public enum SelectedMode
{
    none,
    PlayerVsPlayer,
    PlayerVsCPU
}
