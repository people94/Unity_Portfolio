using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneButton : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel = null;
    [SerializeField] private GameObject optionPanel = null;

    //시작 버튼 누를때
    public void OnStartButton()
    {
        //GameScene씬 로드
        SceneMgr.instance.LoadScene("GameScene");
    }

    //옵션 버튼 누를때
    public void OnOptionButton()
    {
        menuPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    //나가기 버튼 눌렀을때
    public void OnExitButton()
    {
        //게임 종료
        Application.Quit();
    }
    
}
