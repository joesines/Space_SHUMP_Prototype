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
public class Weapon : MonoBehaviour{

}

	