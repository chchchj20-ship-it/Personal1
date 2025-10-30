using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follew : MonoBehaviour
{
    //카메라가 따라가야 할 타겟
    public Transform target;
    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {

        transform.position = target.position + offset;

    }
}
