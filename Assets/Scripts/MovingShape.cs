using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShape : MonoBehaviour {

    public Transform start;
    public Transform end;
    public float duration = 5;
    [Range(0,1)]
    public float timeOffset;
    public AnimationCurve curve;

    Vector3 startVec;
    Vector3 endVec;
    Rigidbody rb;
    float t;
    bool back;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startVec = start.position;
        endVec = end.position;
        t = timeOffset;
    }

    void FixedUpdate()
    {
        if (back)
            t -= Time.fixedDeltaTime / duration;
        else
            t += Time.fixedDeltaTime / duration;
        if (t < 0) 
        {
            back = false;
            //t = 0;
        }
        if (t > 1) 
        {
            back = true;
            //t = 1;
        }

        rb.MovePosition(Vector3.Lerp(startVec, endVec, t));
    }
}
