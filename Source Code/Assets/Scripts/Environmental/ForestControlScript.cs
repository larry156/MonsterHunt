/* Name: Larry Y.
 * Date: January 9, 2019
 * Desc: Handles map randomization for the forest map. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class MapConditionChances
{
	public int clearChance, rainyChance, muddyChance;
}

[System.Serializable]
public class RainWeights
{
	public int puddleChance, mudChance;
}

public class ForestControlScript : NetworkBehaviour
{
	public enum MapCondition { Clear, Rainy, Muddy }

	private MapCondition mapCondition;
	private GameObject[] objSpawnPoints;

	public GameObject[] mudPuddlePrefabs, rainPuddlePrefabs;
	public MapConditionChances mapConChances;
	public RainWeights rainWeights;

	private void Start ()
	{
		// The map should have a bunch of spawn points around.
		objSpawnPoints = GameObject.FindGameObjectsWithTag("ObjSpawnPoint");

		// Determine the map condition
		int rollLimit = mapConChances.clearChance + mapConChances.rainyChance + mapConChances.muddyChance;
		int mapRoll = Random.Range(0, rollLimit);
		if (mapRoll < mapConChances.clearChance)
		{
			mapCondition = MapCondition.Clear;
		}
		else if (mapRoll < mapConChances.rainyChance + mapConChances.clearChance)
		{
			mapCondition = MapCondition.Rainy;
		}
		else if (mapRoll < mapConChances.muddyChance + mapConChances.rainyChance + mapConChances.clearChance)
		{
			mapCondition = MapCondition.Muddy;
		}
		Debug.Log("It's " + mapCondition + " out today.");

		// Nothing special happens if it's clear out.
		if (mapCondition == MapCondition.Rainy)
		{
			// Spawned objects can be rain puddles or mud puddles
			rollLimit = rainWeights.puddleChance + rainWeights.mudChance;
			for (int i = 0; i < objSpawnPoints.Length; i++)
			{
				int puddleOrMudRoll = Random.Range(0, rollLimit);
				//Debug.Log("puddleOrMudRoll is " + puddleOrMudRoll);
				if (puddleOrMudRoll < rainWeights.puddleChance)
				{
					Debug.Log("Spawning a rain puddle");
					// Spawn a puddle at each spawnpoint, and randomize its rotation.
					int puddleRoll = Random.Range(0, rainPuddlePrefabs.Length);
					int objRotation = Random.Range(0, 360);
					GameObject puddleToSpawn = Instantiate(rainPuddlePrefabs[puddleRoll], objSpawnPoints[i].GetComponent<Transform>());
					puddleToSpawn.transform.Rotate(new Vector3(0, 0, objRotation));
					NetworkServer.Spawn(puddleToSpawn);
				}
				else
				{
					Debug.Log("Spawning a mud puddle");
					int puddleRoll = Random.Range(0, mudPuddlePrefabs.Length);
					int objRotation = Random.Range(0, 360);
					GameObject puddleToSpawn = Instantiate(mudPuddlePrefabs[puddleRoll], objSpawnPoints[i].GetComponent<Transform>());
					puddleToSpawn.transform.Rotate(new Vector3(0, 0, objRotation));
					NetworkServer.Spawn(puddleToSpawn);
				}
			}
		}
		else if (mapCondition == MapCondition.Muddy)
		{
			for (int i = 0; i < objSpawnPoints.Length; i++)
			{
				int puddleRoll = Random.Range(0, mudPuddlePrefabs.Length);
				int objRotation = Random.Range(0, 360);
				GameObject puddleToSpawn = Instantiate(mudPuddlePrefabs[puddleRoll], objSpawnPoints[i].GetComponent<Transform>());
				puddleToSpawn.transform.Rotate(new Vector3(0, 0, objRotation));
				NetworkServer.Spawn(puddleToSpawn);
			}
		}
	}
}
