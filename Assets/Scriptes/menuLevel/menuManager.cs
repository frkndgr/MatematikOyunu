using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startBtn, exitBtn;
    
    
    void Start()
    {
        FadeOut();
    }

    void FadeOut()
    {
        startBtn.GetComponent<CanvasGroup>().DOFade(1,1f);
        exitBtn.GetComponent<CanvasGroup>().DOFade(1,1f).SetDelay(0.5f);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGameLevel()
    {
        SceneManager.LoadScene("gameLevel");
    }
}
