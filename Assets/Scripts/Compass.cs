using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

    public RectTransform arrow;
    public Transform player;

    void Update()
    {
        arrow.rotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
    }


}
