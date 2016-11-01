﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    [SerializeField]
    private WeaponType      _type;

    //this public property masks the field _type & takes action when it is set 
    public WeaponType   type {
        get {
            return (_type);
        }
        set {
            SetType(value);
        }
    }


	// Use this for initialization
	void Awake () {
        //Test to see whether this has passed off screen every 2 seconds 
        InvokeRepeating("CheckOffscreen", 2f, 2f);
	}
    public void SetType( WeaponType eType) {
        //set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        GetComponent<Renderer>().material.color = def.projectileColor;
    }
	
	// Update is called once per frame
	void CheckOffscreen () {
        if (Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds,
                BoundsTest.offScreen) != Vector3.zero) {
            Destroy(this.gameObject);
        }

        }

	
	}
}
