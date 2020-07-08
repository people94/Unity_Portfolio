using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEntrance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            Debug.Log("튜토리얼 플레이어입장");
            SystemManager.instance.stageNum = SystemManager.Stage.Tutorial;
            this.GetComponentInParent<TutorialMap>().PlayerEnter();
            Destroy(this.gameObject);
        }
    }
}
