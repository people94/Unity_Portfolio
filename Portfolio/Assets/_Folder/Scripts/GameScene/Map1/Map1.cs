using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map1 : MonoBehaviour
{
    private bool mapClear;                              //맵 클리어 여부
    EnemyCounter enemyCnt;                              //에너미 개수
    public GameObject[] spawnPoint;                     //에너미 스폰할 장소
    public GameObject enemyPref;                        //에너미 프리팹
    private List<GameObject> enemys;                   //에너미들 담은 리스트
    private int enemyIdx;                              //에너미 리스트 인덱스
    //public GameObject spawnEffect;                    //에너미 스폰할 때 발생할 파티클
    public GameObject spawningCameraPos;                //에너미 스폰될 때 카메라 위치
    private bool playerEnter = false;
    private int spawnIdx = 0;                           //스폰인덱스
    Vector3 prevCameraPos;
    Quaternion prevCameraRot;
    private GameObject player;
    private DialogueSystem ds;
    private bool attackGuide = true;
    public GameObject guideButton;

    private void Start()
    {
        enemyCnt = GetComponent<EnemyCounter>();
        ds = GetComponent<DialogueSystem>();
        player = GameObject.Find("Player");
        enemys = new List<GameObject>();
        for(int i = 0; i < spawnPoint.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPref);
            enemy.SetActive(false);
            //enemy.transform.position = spawnPoint[i].transform.position;
            enemys.Add(enemy);
        }
    }
    
    public void PlayerEnter()
    { 
        prevCameraPos = Camera.main.transform.position;
        prevCameraRot = Camera.main.transform.rotation;
        Camera.main.GetComponent<CameraMove>().enabled = false;
        Camera.main.transform.LookAt(spawningCameraPos.transform);
        Camera.main.transform.position = spawningCameraPos.transform.position;
        StartCoroutine(SpawnProc());
    }
        
    IEnumerator SpawnProc()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            SpawningEnemy();
            if (enemyIdx >= enemys.Count)
            {
                player.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                Camera.main.transform.position = prevCameraPos;
                Camera.main.transform.rotation = prevCameraRot;
                Camera.main.GetComponent<CameraMove>().enabled = true;
                ds.StartDialogue();
                break;
            }
        }
    }

    private void SpawningEnemy()
    {
        enemys[enemyIdx].transform.position = spawnPoint[spawnIdx++].transform.position;
        enemys[enemyIdx].transform.rotation = Quaternion.Euler(enemys[enemyIdx].transform.rotation.x, enemys[enemyIdx].transform.rotation.y + 180, enemys[enemyIdx].transform.rotation.z);
        enemys[enemyIdx].SetActive(true);
        enemys[enemyIdx++].transform.SetParent(this.transform);
        //player.SetActive(false);
    }

    public void OnClickGuide()
    {
        if(!attackGuide)
        {
            attackGuide = true;
            ds.NextDialogue();
            Time.timeScale = 1;
            guideButton.SetActive(false);
        }
    }

    public void OnClickNext()
    {
        if (SystemManager.instance.stageNum == SystemManager.Stage.First)
        {
            Time.timeScale = 0;
            if (ds.dialogueIdx == ds.dialogue.Length)
            {
                ds.HideDialogue();
                for (int i = 0; i < enemys.Count; i++)
                {
                    enemys[i].GetComponent<EnemyFSM>().Patrol();
                }
                return;
            }
            if (attackGuide)
            {
                ds.NextDialogue();
            }
            if (ds.dialogueIdx == 2)
            {
                attackGuide = false;
                guideButton.SetActive(true);
                StartCoroutine(ChangeColor());
                Time.timeScale = 1;
            }
        }
    }

    IEnumerator ChangeColor()
    {
        while(!attackGuide)
        {
            guideButton.transform.GetChild(0).gameObject.SetActive(true);
            guideButton.transform.GetChild(1).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            guideButton.transform.GetChild(0).gameObject.SetActive(false);
            guideButton.transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
