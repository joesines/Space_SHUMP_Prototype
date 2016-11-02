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
        public GameObject       cube;
        public TextMesh         letter;
        public Vector3          rotPerSeconds;//Euler rotation speed
        public float            birthTime;
	}

void Awake() {
    //fine the cube reference 
    cube = transform.FindObjectOfType("Cube").gameObject;


}
	

	
	}

