using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    void LateUpdate()
    {
        if(Camera.current != null)
            transform.LookAt(transform.position + Camera.current.transform.forward);
    }
}
