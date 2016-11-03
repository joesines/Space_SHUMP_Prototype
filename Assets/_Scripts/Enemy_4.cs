using UnityEngine;
using System.Collections;

//part of another serlizable data storage class just like WeaponDefinition
[System.Serializable]
public class Part{
    //these three fields need to be defined in the inspector pane 
    public string       name; //the name of this part
    public float        health; // the amount of health this part has 
    public string[]     protectedBy; // the other pars that protect this 

    //these two fields are set automatically in Start()
    //caching like this makes it faster and easier to find these later 
    public GameObject    go; //this GameObject of this part 
    public Material      mat;//the Material to show damage 

}

public class Enemy_4 : Enemy {
    //Enemy_4 will start Offscreeen and then pick a random point on screen to 
    //movee to. Once it has arrived, it will pick another random point and
    //continute until the player has shot it down

    public Vector3[] points; // stores p0 & p1 for interpoilation 
    public float timeStart; //birth time for this Enemy_4
    public float duration = 4; // duration of movement 
    public Part[] parts;//the array of ship parts 

    // Use this for initialization
    void Start() {
        points = new Vector3[2];
        //there is already an intial position chose by Main.Spawn Enemy()
        //so add it to points as this intial p0 & p1
        points[0] = pos;
        points[1] = pos;

        InitMovement();

        //cache gameObject & material of each part in parts 
        Transform t;
        foreach (Part prt in parts) {
            t = transform.Find(prt.name);
            if (t != null) {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }



        }

    }

    void InitMovement() {
        //pick a new point to move that is on screen 
        Vector3 p1 = Vector3.zero;
        float esp = Main.S.enemySpawnPadding;
        Bounds cBounds = Utils.camBounds;
        p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

        points[0] = points[1]; //shift points [1] to points [0]
        points[1] = p1; ; // ass p1 as points [1]

        //reset the time 
        timeStart = Time.time;
    }
    public override void Move()
    {
        //this completely overrides Enemy.Move() with linear interpolation

        float u = (Time.time - timeStart) / duration;
        if (u >= 1) {// then initialize movement to a new point 
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2); // apply ease out easing to u 
        pos = (1 - u) * points[0] + u * points[1]; // simple linear interpolation 
    }
    //this will override the Oncollision Enter that is part of Enemy.cs
    //because of the way the Monobehaviour declares common Unity functions
    //like OncollisionEnter(), the override keywords is not necessary 
    void OnCollisionEnter(Collision coll) {
        GameObject other = coll.gameObject;
        switch (other.tag) {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //enemies don't take damage unless they're on screen
                //this stop the player from shotting before they are visable
                bounds.center = transform.position + boundsCenterOffset;
                if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds,
                    BoundsTest.offScreen) != Vector3.zero) {
                    Destroy(other);
                    break;
                }
                //Hurt this enemy 
                //find the hameObject that was hit
                //the collision coll has contacts [], an array Contact points 
                //because there wasa collision, we're guaranteed there is at 
                //least a contacts [0]m and contact points have reference to 
                //thi collider, which will be the collider for the part off the 
                //enemy _5 that was hit 
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {// if prtHit wasnt found 
                    //...then it's usually because, very rarely, thisCollider on
                    // contacts [0] will be the ProjectileHeroi instead of the ship
                    //part. If so, just look for other Collider instead
                    goHit = coll.contacts[0].thisCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //Check whether this paet is still protected 
                if (prtHit.protectedBy != null) {
                    foreach (string s in prtHit.protectedBy) {
                        // If one of the protecting parts hasn't been destroyed....
                        if (!Destroyed(s)) {
                            //...then don't damage this part yet 
                            Destroy(other); // Destoy the ProjectileHero
                            return; // return before causing damage 
                        }
                    }
                }
                //it's not protected, so make it take damage
                //get the damage amount from the Projectile.type & Main.W_DEFS
                prtHit.health -= Main.W_DEFS[p.type].damnageOnHit;
                //Show damage on the part 
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0) {
                    //Instead of Destroying this enemy, diable the damaged part 
                    prtHit.go.SetActive(false);
                }
                //Check to see if the whole ship is destroted 
                bool allDestroyed = true; //Assume itis destroyed 
                foreach (Part prt in parts) {
                    if (!Destroyed(prt)) { // if a part still exists 
                        allDestroyed = false; //...change allDestroyed to false
                        break;                //and break out of the foreach loop
                    }
                }
                if (allDestroyed) {// if it is completelt destoyed 
                    //tell the main singleton that this ship has been destoyed 
                    Main.S.ShipDestroyed(this);
                    //Destoy this Enemy
                    Destroy(this.gameObject);
                }
                Destroy(other); // Destory the ProjectileHero
                break;
        }

    }
    //these two functions find a part in parts baes on name or GameObject 
    Part FindPart(string n) {
        foreach (Part prt in parts) {
            if (prt.name == n) {
                return (prt);
            }
        }
        return (null);
    }
    Part FindPart(GameObject go) {
        foreach (Part prt in parts) {
            if (prt.go == go) {
                return (prt);
            }
        }
        return (null);
    }
    //These functions return true if the part has been destroyed 
    bool Destroyed(GameObject go) {
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed (string n){
        return (Destroyed(FindPart(n)));
    }
    bool Destroyed (Part prt)  {
        if (prt == null) {
            //If no real ph was passed in
            return (true); // return true (meaning, yes it was destroyed
        }
        //Return the result of rhe comparison: prt.health <=0
        //if prt.health is 0 or less, return true (yes, ut was destoyed)
        return (prt.health <= 0);
    }
   //This changes the color just one Part to red instead of the whole ship
   void ShowLocalizedDamage (Material m) {
        m.color = Color.red;
        remainingDamageFrames = showDamageForFrames;

    }



    // Update is called once per frame
    void Update () {
	
	}
}
