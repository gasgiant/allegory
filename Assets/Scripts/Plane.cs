using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour {

    public static Plane Instance;

    public Vector3 normal;
    public Vector3 p0;

	void Awake()
	{
        Instance = this;
	}

	void Update()
	{
        normal = transform.forward;
        p0 = transform.position;
	}

	void OnDrawGizmosSelected()
	{
        normal = transform.forward;
        p0 = transform.position;
	}
}
