using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public GameObject spark;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.score++;
            spark.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
