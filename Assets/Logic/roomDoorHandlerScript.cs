﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class roomDoorHandlerScript : MonoBehaviour {

	public bool paidScrap;
	bool tryOpen;
	bool open;
	List<GameObject> innerDoors;
	List<GameObject> outerDoors;
	float time;
	float transitionTime;
	int scrapCost;

	float playerCheck;
	float playerTime;

	// Use this for initialization
	void Start () {
		paidScrap = false;
		innerDoors = new List<GameObject>();
		outerDoors = new List<GameObject>();
		time = 0;
		transitionTime = .5f;
		scrapCost = 10;
		open = false;

		playerCheck = 0;
		playerTime = .5f;


		findNearDoors();

	}
	
	// Update is called once per frame
	void Update () {
		if (!paidScrap && playerCheck >= playerTime)
		{
			playerDetect();
		}
		else
		{
			playerCheck += Time.deltaTime;
		}
		if (paidScrap == true && !open)
		{
			if (time < transitionTime)
			{
				// open inner doors
				foreach (GameObject gameOBJ in innerDoors)
				{
					if (gameOBJ.GetComponent<Door>().state == 0)
					{
						gameOBJ.GetComponent<Door>().state = 1;
					}
				}
			}
			else
			{	// open outer doors
				foreach (GameObject gameOBJ in outerDoors)
				{
					if (gameOBJ.GetComponent<Door>().state == 0)
					{
						gameOBJ.GetComponent<Door>().state = 1;
						open = true;
					}
				}

			}
			
			time += Time.deltaTime;

		}

	}

	private void playerDetect()
	{
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, 10);
		
		foreach (Collider2D col in hitColliders)
		{
			if (col.gameObject.name.Equals("Slash(Clone)"))
			{
				tryOpen = true;
				break;
			}
		}
		if (tryOpen)
		{
			foreach (Collider2D col in hitColliders)
			{
				if (col.gameObject.name.Equals("Player"))
				{
					if (col.gameObject.GetComponent<Player>().stats.get_scrap() >= scrapCost)
					{
						col.gameObject.GetComponent<Player>().stats.change_scrap(-scrapCost);
						paidScrap = true;
						
					}
					else
					{
						tryOpen = false;
					}
				}
			}
			findNearDoors();
		}


	}

	private void findNearDoors()
	{
		Collider2D[]hitColliders = Physics2D.OverlapCircleAll(this.transform.position,10);
		
		foreach (Collider2D col in hitColliders)
		{
			if (col.gameObject.name.Equals("DoorPiece1(Clone)"))
			{
				innerDoors.Add(col.gameObject);
			}else if (col.gameObject.name.Equals("DoorPiece2(Clone)") )
			{
				outerDoors.Add(col.gameObject);
			}else if (col.gameObject.name.Equals("baseSpawner(Clone)"))
			{
				col.gameObject.GetComponent<EnemySpawner>().inSpecialRoom = true;
				if (paidScrap)
				{
					col.gameObject.GetComponent<EnemySpawner>().inSpecialRoom = false; ;
				}
			}

		}
		
	}
}
