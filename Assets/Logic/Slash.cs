﻿using UnityEngine;
using System.Collections;

public class Slash : MonoBehaviour {

	double slashTimer;
    int damage;

	// Use this for initialization
	void Start () {
	
		slashTimer = 0.25;
        damage = 5;

	}
	
	// Update is called once per frame
	void Update () {
	
		slashTimer -= Time.deltaTime;

		if (slashTimer <= 0) {

			Destroy (gameObject);

		}


	}

	void OnTriggerEnter2D( Collider2D col ) {

		if (col.tag == "Enemy") {

			col.gameObject.SendMessage ("OnHit", Tools.AngleToVec2(Tools.QuaternionToAngle(transform.rotation) + 90.0f, 300.0f ));
            col.GetComponent<Baseenemy>().health -= damage;

		}

	}
}
