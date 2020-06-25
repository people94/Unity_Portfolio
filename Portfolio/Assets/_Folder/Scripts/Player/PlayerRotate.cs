using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotSpeed = 5.0f;

    public void Rotate(Quaternion dir)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, dir, rotSpeed * Time.deltaTime);
    }
}
