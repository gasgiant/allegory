using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Vector3 inMenuPosition;

    GameManager gm;
    Transform tr;
    float vel;
    float y;


    void Start()
    {
        tr = transform;
        y = tr.position.y + 1;
        gm = GameManager.Instance;
    }

    void Update()
    {
        if (gm.state == GameManager.GameState.Normal)
        {
            y = Mathf.SmoothDamp(y, target.position.y, ref vel, 0.3f);
            tr.position = new Vector3(target.position.x, 0, target.position.z) + Vector3.up * y;
            tr.rotation = target.rotation;
        }
        else
        {
            Vector3 vec = inMenuPosition;
            tr.position = vec;
            tr.rotation = Quaternion.identity;
            y = vec.y;
        }
    }

    public bool Reset(Vector3 position, Quaternion rotation, Vector3 currTargPos)
	{
        Vector3 vec = position + Vector3.up * 30;
        tr.position = vec;
        tr.rotation = rotation;
        y = vec.y;
        return false;
	}
}
        
