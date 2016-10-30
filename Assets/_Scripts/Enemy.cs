using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float     speed = 10f; // the sspeed in m/s
    public float     fireRate = 0.3f; // seconds/shot (unused) 
    public float     health = 10;
    public int       score = 100; // points earned for destroying this 

    public bool __________________________;

    public Bounds bounds; // the bounds of this and its children 
    public Vector3 boundsCenterOffset; // Dis of bounds.center from position 

    void Awake()  {
        InvokeRepeating("CheckOffscreen", 0f, 2f);
    }

	// Update is called once per frame
	void Update () {
        Move();
	}

    public virtual void Move(){
        Vector3 temPos = pos;
        temPos.y -= speed * Time.deltaTime;
        pos = temPos;
    }
    //This is a property : A method that acts like a field 
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
     void CheckOffScreen () {
        // If bounds are still their default value 
        if (bounds.size == Vector3.zero) {
            //then set them 
            bounds = Utils.CombineBoundsofChildren(this.gameObject);
            //Also find the diff between bounds.center & transform.position
            boundsCenterOffset = bounds.center - transform.position;
        }

        //Every time, Update the bounds to the current position
        bounds.center = transform.position + boundsCenterOffset;
        //check to see whether the bounds are completely off screen 
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero) {
            // if this enemy has gone off the bottom edge of the screen 
            if (off.y < 0) {
                // then destroy it
                Destroy(this.gameObject);

            }

        }
    }
        

            }

     

