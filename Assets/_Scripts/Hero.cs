using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
    static public Hero S;//Singleton 

    //These fields control the movement of the ship 
    public float     speed = 30;
    public float     rollMult = -45;
    public float     pitchMult = 30;

    //Ship status information 
    public float     shieldLevel = 1;
    public bool      _____________________;
    public Bounds    bounds; 


	// Use this for initialization
	void Awake () {
        S = this;   // Set this singleton 
        bounds = Utils.CombineBoundsofChildren(this.gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
        // Pull in information form the input class 
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //change transform.position based on the axes 
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //keep the shhip constrainted to the screen bounds 
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if (off != Vector3.zero)
        {
            pos -= off;
            transform.position = pos;
            //Rotate the ship to make it feel more dybamic 

            transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        bounds.center = transform.position;

      
        }
	
	}
}
