using UnityEngine;
using System.Collections;

//Enemy_1 extends the Enemy class
public class Enemy_1 : Enemy {
    //because Enemy_1 extends Enemy, the ___bool won't work
    //the sa,e way in the Inspector pane. :/

    //#seconds for a full sine wave 
    public float        waveFrequencey = 2;
    //since wave width in meters
    public float        waveWidth=4;
    public float        waveRotY = 45;

    private float       x0 = -12345; // the initial x values of pos
    private float       birthTime;

	// Use this for initialization
	void Start () {
        //set x0 to the initial x postion of Enemy_1
        //this works fine because the position will have already 
        //been set by Main.SpawnEnemy() before start() runs
        //(though Awake() would have been too early!).
        //This is also good because there is no Start() method
        //On enemy
        x0 = pos.x;

        birthTime =Time.time;
	}

    //override the move function on enemy
    public override void Move() {
        //because pos is a property, you can't directly set pos.x
        //so get tgepos an editable Vector3
        Vector3 tempPos = pos;
        //theta adjusts based on time
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequencey;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        tempPos = tempPos;

        // rotate a bit about y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        //base.Move() still heandles the movement down in Y
        base.Move();


    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
