using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
    static public Main S;

    public GameObject[]  prefabEnemies;
    public float         enemySpawnPerSecond = 0.5f;
    public float         enemySpawnPadding = 1.5f;

    public bool          ______________________________;

    public float        enemySpawnRate; //Delay between enemu spawns 


	// Use this for initialization
	void Awake () {
        S = this;
        //set Utils.camBounds
        Utils.SetCameraBounds(this.GetComponent<Camera>());
        // 0.5 enemies/second = enemySpawnRate of 2
        enemySpawnRate = 1f / enemySpawnPerSecond;
        // invoke call spawn enemy() once after a 2 second delay 
        Invoke("SpawnEnemy", enemySpawnRate);
	
	}
	
    public void SpawnEnemy() {
        // pick a random enemy prefab to instantiate 
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies [ndx]) as GameObject;
        //position the enemy above the screen with a random x position 
        Vector3 pos = Vector3.zero;
        float xMin = Utils.camBounds.min.x + enemySpawnPadding;
        float xMax = Utils.camBounds.max.x - enemySpawnPadding;
        go.transform.position = pos;
        //call SpawnEnemy() again in a coupld of seconds 
        Invoke("SpawnEnemy", enemySpawnRate);

    }
	
	
	}
