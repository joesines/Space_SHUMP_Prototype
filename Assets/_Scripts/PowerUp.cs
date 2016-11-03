using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {
        //This is an unusual but handy use of vector2s. xhold a min value 
        //and y a max value fro a random.Range() that will be called later
        public Vector2            rotMinMax = new Vector2(15, 90);
        public Vector2           driftMinMax = new Vector2(.25f, 2);
        public float             lifeTime = 6f;//Seconds the PowerUp Exists
        public float            fadeTime = 4f;//Seconds it will then fade
        public bool             _____________________;
        public WeaponType       type;
        public GameObject       cube; //Reference to the cube child
        public TextMesh         letter;// reference to the textmesh
        public Vector3          rotPerSeconds;//Euler rotation speed
        public float            birthTime;

    void Awake() {
        //fine the cube reference 
        cube = transform.Find("Cube").gameObject;
        //Find the textmesh 
        letter = GetComponent<TextMesh>();

        // Set a random Velocity 
        Vector3 vel = Random.onUnitSphere; //Get Random XYZ velocity 
        //Rnadom .onUnitSphere gives you a vector point that is somewhere on
        //the surface of the sphere with a radius of 1m around the origin
        vel.z = 0;  //Flattern the vel to the XY plane 
        vel.Normalize(); // Make the length of the vel 1
        //Normalizing a Vector3 makes it length 1m
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        //Above sets the velocity length to something between x and Y 
        //values of driftMinMAx
        GetComponent<Rigidbody>().velocity = vel;

        //set the rotation of this GameObject to R:[0,0,0]
        transform.rotation = Quaternion.identity;
        //Quaternion.dentify is equal to no rotation

        //Set up the rotPetSecond for the cub child using rotMinMax x&y
        rotPerSeconds = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
                                     Random.Range(rotMinMax.x, rotMinMax.y),
                                     Random.Range(rotMinMax.x, rotMinMax.y));

        //CheckOffscreen() every 2 seconds 
        InvokeRepeating("CheckOffscreen", 2f, 2f);

        birthTime = Time.time;
    }

    void Update() {
        //Manually rotate the Cube chld every Update()
        //Multpluing it by the Time.time causes the rotation to be time-based
        cube.transform.rotation = Quaternion.Euler(rotPerSeconds * Time.time);

        //Fade out the PowerUp over time
        //Given the default values, a powerUp will exist for 10 seconds
        //and then fade out pver 4 seconds.
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //for lifetime seconds, u will be <=0. Then it will transition to 1
        //over fadeTime seconds.
        //if u >=1, destory this PowerUp
        if (u >= 1) {
            Destroy(this.gameObject);
            return;
        }
        //Use U to determine the alpha value of the cube & letter 
        if (u > 0) {
            Color c = cube.GetComponent<Renderer>().material.color;
            c.a = 1f - u;
            cube.GetComponent<Renderer>().material.color = c;
            //Fade the Letter Too, just not as much 
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
    }

    public void SetType(WeaponType wt) {
        //Grab the Weapon Definition from Main 
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        //set the color of the cube child
        cube.GetComponent<Renderer>().material.color = def.color;
        //leter.text = def.letter; // set the letter that is shown 
        letter.text = def.letter;
        type = wt; // finall actually set the type 
    }

    public void AbsorbedBy(GameObject target) {
        //This function is called by the hero class when a powerUp is collected
        //we could tween into the target and shrink in size
        //but for now, just destroy this.gameObject
        Destroy(this.gameObject);
    }

    void CheckOffscreen() {
        //If the PowerUp has defifted entirel off screen
        if (Utils.ScreenBoundsCheck(cube.GetComponent<Collider>().bounds,
            BoundsTest.offScreen) != Vector3.zero)
            //...then destroy (this.gameObject)
            Destroy(this.gameObject);

        }

    }






