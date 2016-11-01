using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
    static public Main S;
    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;

    public GameObject[]  prefabEnemies;
    public float         enemySpawnPerSecond = 0.5f;
    public float         enemySpawnPadding = 1.5f;
    public WeaponDefinition[]  weaponDefinitions;

    public bool          ______________________________;
    public WeaponType[]  activeWeaponTypes;

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
        //A generic Dictionary with WeaponType as the key 
        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions) {
            W_DEFS[def.type] = def;
        }
        }
    static public WeaponDefinition GetWeaponDefinition (WeaponType wt) {
        //check to make sure that the key exists in the dictionary 
        //attempting to retrieve a key that didn't exist, would throw an error,
        //so the folloing if statement is important 
        if (W_DEFS.ContainsKey(wt)) {
            return (W_DEFS[wt]);
        }
        //This will return a definition for WeaponType.non
        //which means it has failed to find the WeaponDefinitions
        return (new WeaponDefinition());

    }

	
	}

    void Start() {
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for( int i=0; i<weaponDefinitions.Length; i++) {
            activeWeaponTypes[i] = weaponDefinitions[i].type;

        }

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
    public void DelayedRestart(float delay) {
        //Invoke the restart ()method in delay seconds
        Invoke("Restart", delay);
    }
    public void Restart() {
        //reload_scene_0 to restart the game
        Application.LoadLevel("__Scene_0");

    }
	
	
	}
