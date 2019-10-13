using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveZone : MonoBehaviour {

    public Vector3 offset = Vector3.up * 8;
    public float deathLine = -20;

    public Vector3 position;

    void Awake()
    {
        position = transform.position;
    }
}
