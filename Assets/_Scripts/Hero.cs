using UnityEngine;
using System.Collections;



public class Hero : MonoBehaviour
{

    static public Hero S;//Singleton 
    public float  gameRestartDelay = 2f;

    //These fields control the movement of the ship 
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    //Ship status information 
    [SerializeField]
    private float _shieldLevel = 1;
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
        //find the tag of other.gameobject ot its parent game objects
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        //if there is a parent with a tag
        if (go != null)
            //Make sure it's not the same triggering go as last time 
            if (go == lastTriggerGo)
            {
                return;
            }
        lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            //if the shield was triggered by an enemy 
            //Decreases the level of the shild by 1 
            shieldLevel--;
            //destroy the enemy 
            Destroy(go);
        }
        else
        {
            //Annouce it 
            print("Triggered:" + go.name);
            //Otherwise announce the original other.gameOject
            print("Triggered:" + other.gameObject.name);
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

