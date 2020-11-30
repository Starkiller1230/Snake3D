using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SnakeTail : MonoBehaviour
{

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

}
