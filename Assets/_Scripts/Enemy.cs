using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float     speed = 10f; // the sspeed in m/s
    public float     fireRate = 0.3f; // seconds/shot (unused) 
    public float     health = 10;
    public int       score = 100; // points earned for destroying this 
    public int       showDamageForFrames = 2; //#frames to show damage

    public bool __________________________;

    public Color[]       originalColors;
    public Material[]    materials;
    public int           remainingDamageFrames = 0; // Damnage Frames left 

    public Bounds bounds; // the bounds of this and its children 
    public Vector3 boundsCenterOffset; // Dis of bounds.center from position 

    void Awake()  {
        materials = Utils.GetAllMaterials(gameObject);
       originalColors = new Color[materials.Length];
        for(int i=0; i<materials.Length; i++) {
            originalColors[i] = materials[i].color;

        }
        InvokeRepeating("CheckOffscreen", 0f, 2f);
    }

	// Update is called once per frame
	void Update () {
        Move();
        if (remainingDamageFrames > 0) {
            remainingDamageFrames--;
            if (remainingDamageFrames == 0) {
               UnShowDamage();
            }
        }
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

        void OnCollisionEnter(Collision coll) {
            GameObject other = coll.gameObject;
            switch (other.tag) {
                case "ProjectileHero":
                    Projectile p = other.GetComponent<Projectile>();
                    //Enemies don't take damnage unless they're on screen 
                    //this stops the player from shooting them before they are visable
                    bounds.center = transform.position + boundsCenterOffset;
                    if (bounds.extents == Vector3.zero ||
                   Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen) !=Vector3.zero) {
                        Destroy(other);
                         break; 
                     }
                    //Hurt this enemy 
                    ShowDamage();
                    //get the damnage amount from the projectile.type & Main.W_DEFS
                    health-= Main.W_DEFS[p.type].damageOnHit;
                    if (health<= 0) {
                        //Destroy this Enemy
                        Destroy(this.gameObject);
                    }
                    Destroy(other);
                    break;
            }
            void ShowDamage() {
                foreach (Material m in materials) {
                    m.color = Color.red;
                }
                remainingDamageFrames = showDamageForFrames;
            }
            void UnShowDamage() {
                for(int i=0; i<materials.Length; i++) {
                    materials[i].color = originalColors[i];
                }
            }
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

     

