using UnityEngine;
using System.Collections;



public class Hero : MonoBehaviour
{

    static public Hero S;//Singleton 
    public float gameRestartDelay = 2f;

    //These fields control the movement of the ship 
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    //Ship status information 
    [SerializeField]
    private float _shieldLevel = 1; //Add the underscore!
    //Weapon fields
    public Weapon[] weapons;
    public bool _____________________;
    public Bounds bounds;
    //declare a new delegate type weaponfire delegate
    public delegate void WeaponFireDelegate();
    //create a weaponfiredelegate field name fire delegate
    public WeaponFireDelegate fireDelegate;


    // Use this for initialization
    void Awake()
    {
        S = this;   // Set this singleton 
        bounds = Utils.CombineBoundsofChildren(this.gameObject);

        //reset the weapons to start _hero with 1 blaster 
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }

    // Update is called once per frame
    void Update()
    {
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
            //Rotate the ship to make it feel more dynamic 

            //use the fireDelegate to fire weapons 
            //first, make sure the axis("jump") button is pressed 
            //then ensure that fireDelegate isn't null to avoud an error
            if (Input.GetAxis("jump") == 1 && fireDelegate != null) {
                fireDelegate();
            }


            transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

            bounds.center = transform.position;

        }

    }
    //This variable holds a reference to the last triggering gameobject 
    public GameObject lastTriggerGo = null;

    void OnTriggerEnter(Collider other)
    {
        //find the tag of other.gameObjct or its parent GameObjects
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        //if there is a parent with a tag
        if (go != null)
        {
            //make sure its not the same triggering go as last time 
            if (go == lastTriggerGo)
            {
                return;
            }

            lastTriggerGo = go;
            if (go.tag == "Enemy")
            {
                //if the shueld was triggered by an enemy 
                //decrease the level of the shield by 1 
                shieldLevel--;
                //Destroy the enemy 
                Destroy(go);
            }else if (go.tag == "PowerUP"{
                //If the shield was triggered by a PowerUp
               AbsorbPowerUp(go);
            } else {
                print("Triggered: " + go.name);
            }
            //Announce it    
        } else {
            print("Triggered: " + other.gameObject.name);
        }
    }

    public void AbsorbPowerUp(GameObject go) {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type) {
            case WeaponType.shield: // if its the shield
                shieldLevel++;
                break;

            default: // if its any weapon PowerUp
                //check the current weapon type 
                if (pu.type == weapons[0].type)
                {
                    //then increase the number of weapons of this type
                    Weapon w = GetEmptyWeaponSlot(); //find an available weapon
                    if (w != null)
                    {
                        //set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    //if this is a different weapon
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }
    Weapon GetEmptyWeaponSlot() {
        for (int i = 0; i<weapons.Length; i++) {
            if( weapons[i].type == WeaponType.none) {
                return (weapons[i]);
            }
        }
        return(null);
    }
    void ClearWeapons() {
        foreach (Weapon w in weapons) {
            w.SetType(WeaponType.none);
        }

    }


        public float shieldLevel {
        get {
            return (_shieldLevel); 
           }

        set {
            _shieldLevel = Mathf.Min(value, 4);
            //if the shiled is going to be set to less than zero 
            if (value < 0) {
                Destroy(this.gameObject);
                //Tell Main.S to restart the game after a delay 
                Main.S.DelayedRestart(gameRestartDelay);
            }


        }

    }
    }

