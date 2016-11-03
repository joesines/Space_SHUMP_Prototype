using UnityEngine;
using System.Collections;

//This is an enum of the various possible weapon types 
//it also includes a "shield" type to alllow a shiled power-up
//items marked [NI] below are not implemented in this book 

public enum WeaponType {
    none,       //the default/ no weapon 
    blaster,    //a simple blaster 
    spread,     //two shots simultaneously
    phaser,     //shots that move in waves [NI]
    missle,     //homing missles [NI]
    laser,      //Damage over time [NI]
    shield      //Raise shieldLevel
}
//The weapondefintion class allows you to set the properties
//of a specific weapon in the inspector.main has an array 
//of weatons that makes this possible
//[System.Serializable] tell Unity to try to view weapondefinition
//in the inspector pane.it doesn't work for everything, but it
//will work for simple classes like this!

[System.Serializable]
public class WeaponDefinition {
    public WeaponType      type = WeaponType.none; 
    public string           letter;                 // The letter to show on the power-up
    public Color            color = Color.white;    //Color of collar & power-up
    public GameObject       ProjectilePrefab;       //prefab for projectile 
    public Color            projectileColor = Color.white;
    public float            damnageOnHit = 0;       //Amount of damnage caused 
    public float            continiousDamnage = 0;  //Damage per econd (laser) 
    public float            delayBetweenShots = 0;
    public float            velocity = 20;          // speed of projectile 
}
//Note: Weapon prefabs, color, and son on. are set in the class min.
public class Weapon : MonoBehaviour {
    static public Transform PROJECTILE_ANCHOR;


    public bool                ________________;
    [SerializeField]
    private WeaponType       _type = WeaponType.blaster;
    public WeaponDefinition     def;
    public GameObject        collar;
    public float             lastShot; //Time last shot was fired 

    void start() {
        collar = transform.Find("Collar").gameObject;
        //call SetType() properly for the default _type 
        SetType(_type);

        if (PROJECTILE_ANCHOR == null) {
            GameObject go = new GameObject("_Projectile_Anchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //Fine the fireDelegate of the parent
        GameObject parentGO = transform.parent.gameObject;
        if (parentGO.tag == "Hero") {
            Hero.S.fireDelegate += Fire;
        }
    }
    public WeaponType type { 
        get { return(_type); }
        set { SetType( value ); }
}
    public void SetType(WeaponType wt) {
        _type = wt;
        if (type == WeaponType.none) {
            this.gameObject.SetActive(false);
            return;
        }else {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collar.GetComponent<Renderer>().material.color = def.color;
        lastShot = 0; // You can always fire immediately after _type is set
        }
    public void Fire() {
        // if this gameObject is inactive, return
        if (!gameObject.activeInHierarchy) return;
        //if it hasn't been enough time between shots, return
        if (Time.time - lastShot < def.delayBetweenShots) {
            return;
        }
        Projectile p;
        switch (type) {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                break;

            case WeaponType.spread:
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(-.2f, 0.9f,0) *
                    def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody>().velocity = new Vector3(.2f, 0.9f, 0) *
                    def.velocity;
                break;
        }
    }
    public Projectile MakeProjectile() {
        GameObject go = Instantiate(def.ProjectilePrefab) as GameObject;
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }else {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShot = Time.time;
        return (p);
        }


    }
