﻿using UnityEngine;
using System.Collections;

public class FlyerDrone : Baseenemy
{
    //public GameObject Slash;
	public EnemyBullet bullet;
	public AudioClip X_Bullet_Shoot;
    

    // Use this for initialization
    void Awake()
    {

		base.Maxhealth = 20;
		base.health = base.Maxhealth;
        base.speed = 4f;
        base.rate = 1.5f;
		base.rateVariance = 0.1f;
        base.range = 8f;
        base.damage = 6;
		base.pointValue = 25;
    }


    public override void TimeIncrease(float time) {
		// How fast it takes for enemy params to go from 1x to 2x, 2x to 3x, etc.
		var timeScale = time / 150f;

		Maxhealth = System.Math.Min(24000, Maxhealth + (int)(2f * Maxhealth * Mathf.Pow(timeScale, 2f)));
		health = Maxhealth;

		speed = Mathf.Min(20f, speed + (0.16f * speed * timeScale));
		rate = Mathf.Max(0.35f, rate - (0.032f * rate * timeScale));
		damage = System.Math.Min(400, damage + (int)(0.115f * damage * Mathf.Pow(timeScale, 2f)));
    }

    public override void attack()
    {
        // Play Shoot Sound
		CameraRunner.gAudio.PlayOneShot(X_Bullet_Shoot, 1.0f);

		float chance = UnityEngine.Random.value;

		if (chance <= 0.5f) {
			// Fire a single bullet at the player
			var b = (EnemyBullet)Instantiate(bullet, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation) + 180));
			b.damage = damage / 2;
			b.set_duration(2f);
			// Impart velocity to bullet
			b.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z - 90.0f, 12.0f);
		} else {
			// Fire two bullets at an angle from the direction that the drone is facing
			var b = (EnemyBullet)Instantiate(bullet, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation) + 180));
			b.damage = damage / 2;
			b.set_duration(2f);
			// Impart velocity to bullet
			b.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z - 70.0f, 12.0f);

			b = (EnemyBullet)Instantiate(bullet, gameObject.GetComponent<Rigidbody2D>().position, Tools.AngleToQuaternion(Tools.QuaternionToAngle(transform.rotation) + 180));
			b.damage = damage / 2;
			b.set_duration(2f);
			// Impart velocity to bullet
			b.GetComponent<Rigidbody2D>().velocity = Tools.AngleToVec2((body.rotation * transform.forward).z - 110.0f, 12.0f);
		}
	}

	public override void Change() {
		// Flyer drones slowdown near player's and speed up otherwise
		if (nearest != null) {
			if (Vector3.Distance(transform.position, nearest.position) <= (4 * range / 5)) {
				body.drag = 2.5f;
			} else {
				body.drag = 0.5f;
			}
		}
	}

}