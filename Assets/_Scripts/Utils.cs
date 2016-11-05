using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This is actually outside of the Utils class 
public enum BoundsTest {

    center,       // is the center of the GameObject on the screen 
    onScreen,    //Are the bounds entirely on  screen 
    offScreen    // Are the Bounds entirelly off screen 

}


public class Utils : MonoBehaviour
{
    //===========================================Bounds Functions ===========================================================//

    // create bounds that encapsulates the two Bounds passed in 
    public static Bounds BoundsUnion(Bounds b0, Bounds b1)
    {
        //if the size of one of the bounds is Vector3.zeo, ignore that one 
        if (b0.size == Vector3.zero && b1.size != Vector3.zero)
        {
            return (b1);
        }
        else if (b0.size != Vector3.zero && b1.size == Vector3.zero)
        {
            return (b0);
        }
        else if (b0.size == Vector3.zero && b1.size == Vector3.zero)
        {
            return (b0);
        }
        //stretch b0 to include the b1.min and b1 max
        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return (b0);
    }

    public static Bounds CombineBoundsOfChildren(GameObject go)
    {
        //Create an empty bounds b 
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        //If this GameObject has a renderer Compenent 
        if (go.GetComponent<Renderer>() != null)
        {
            //Expand b to contain the Renderer's Bounds 
            b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
        }
        // if this GameObject has a Collider Component 
        if (go.GetComponent<Collider>() != null)
        {
            //expands b to contain the collider's bounds 
            b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
        }
        //iterate through each child of this gameObject.transform 
        foreach (Transform t in go.transform)
        {
            //Expand b to contain their bounds as well 
            b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
        }
        return (b);
    }

    // Make a static read-only public propert camBounds
    static public Bounds camBounds
    {
        get
        {
            // if _camBound hasn't been set yet 
            if (_camBounds.size == Vector3.zero)
            {
                // SetCamerabounds using the default Camera 
                SetCameraBounds();
            }
            return (_camBounds);

        }
    }
    // This is the private stattic field that CamBound uses 
    static private Bounds _camBounds;

    public static void SetCameraBounds(Camera cam = null)
    {
        //if no camera wass passed in use the main camera 
        if (cam == null) cam = Camera.main;
        // this makes a coupd important assumptions about the camera!:
        //1. the camera is orthographic 
        //2. the camera is at rotation of R:[0,0,0]

        // Make Vector3s at the topLeft and bottomRight of the Screen coords
        Vector3 topLeft = new Vector3(0, 0, 0);
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);

        // Convert these to world coordinated 
        Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
        Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

        //Adjust the z to be at the near and far camera clipping planes 
        boundTLN.z += cam.nearClipPlane;
        boundBRF.z += cam.farClipPlane;

        //Find the center of the Bounds 
        Vector3 center = (boundTLN + boundBRF) / 2f;
        //Expans _camBounds to encapsulate the extents
        _camBounds.Encapsulate(boundTLN);
        _camBounds.Encapsulate(boundBRF);
    }

    // checks to see whether the bounds bnd are within the camBounds 
    public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center)
    {
        return (BoundsInBoundsCheck(camBounds, bnd, test));
    }

    // test to see whether lilB is inside bigB
    public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB,
        BoundsTest test = BoundsTest.onScreen)
    {
        //Get the Center of lilB 
        Vector3 pos = lilB.center;

        // The behavior of this functiion is different based on the BoundTest 
        //that has been selected 
        //Initialize the offset at [0,0,0]
        Vector3 off = Vector3.zero;

        switch (test)
        {
            //the enter test determins what off (offset) would have to be applied to lilb to move -its center back inside big B
            case BoundsTest.center:
                // if the center is contained, returm Vector3.zero
                if (bigB.Contains(pos))
                {
                    return (Vector3.zero);
                }

                // if not contained find the offset 

                if (pos.x > bigB.max.x)
                {
                    off.x = pos.x = bigB.max.x;
                }
                else if (pos.x < bigB.min.x)
                {
                    off.x = pos.x = bigB.min.x;
                }
                if (pos.y > bigB.max.y)
                {
                    off.y = pos.y - bigB.max.y;
                }
                else if (pos.y < bigB.min.y)
                {
                    off.y = pos.y - bigB.min.y;
                }
                if (pos.z > bigB.max.z)
                {
                    off.z = pos.z - bigB.max.z;
                }
                else if (pos.z < bigB.min.z)
                {
                    off.z = pos.z - bigB.min.z;
                }
                return (off);
            // this onScreen test determines what off would have to be applied to keep all of the lil B inseide of bigB
            case BoundsTest.onScreen:
                //find whether bigB contains all og lilB
                if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max))
                {
                    return (Vector3.zero);
                }
                // if not, find the offset 
                if (lilB.max.x > bigB.max.x)
                {
                    off.x = lilB.max.x - bigB.max.x;
                }
                else if (lilB.min.x < bigB.min.x)
                {
                    off.x = lilB.min.x - bigB.min.x;
                }
                if (lilB.max.y > bigB.max.y)
                {
                    off.y = lilB.max.y - bigB.max.y;
                }
                else if (lilB.min.y < bigB.min.y)
                {
                    off.y = lilB.min.y - bigB.min.y;
                }
                if (lilB.max.z > bigB.max.z)
                {
                    off.z = lilB.max.z - bigB.max.z;
                }
                else if (lilB.min.z < bigB.min.z)
                {
                    off.z = lilB.min.z - bigB.min.z;
                }
                return (off);
            // the off screentest detrmins what off would need to be applied to move any tiny part of lil B inside of bigB
            case BoundsTest.offScreen:
                //Find whether bigB contains any of lilB
                bool cMin = bigB.Contains(lilB.min);
                bool cMax = bigB.Contains(lilB.max);
                if (cMin || cMax)
                {
                    return (Vector3.zero);
                }
                //if not, find the offset 
                if (lilB.min.x > bigB.max.x)
                {
                    off.x = lilB.min.x - bigB.max.x;
                }
                else if (lilB.max.x < bigB.min.x)
                {
                    off.x = lilB.max.x - bigB.min.x;
                }
                if (lilB.min.y > bigB.max.y)
                {
                    off.y = lilB.min.y - bigB.max.y;
                }
                else if (lilB.max.y < bigB.min.y)
                {
                    off.y = lilB.max.y - bigB.min.y;
                }
                if (lilB.min.z > bigB.max.z)
                {
                    off.z = lilB.min.z - bigB.max.z;
                }
                else if (lilB.max.z < bigB.min.z)
                {
                    off.z = lilB.max.z - bigB.min.z;
                }
                return (off);
        }
        return (Vector3.zero);
    }

    //=====================================================Transform Functions=================================================\\
    //util it either finds a parent with a tag != "untagged" or no parent 
    public static GameObject FindTaggedParent(GameObject go)
    {
        // if this gameObject has a tag
        if (go.tag != "Untagged")
        {
            //then return this gameObject
            return (go);
        }
        //if there is no parent of this transform 
        if (go.transform.parent == null)
        {
            //we've reached the top of the hierarchy with no interesting tag
            //so return null 
            return (null);
        }
        //Otherwise, recursively climb up the tree
        return (FindTaggedParent(go.transform.parent.gameObject));
    }
    //This version of the function handles things if a transform is passes in 
    public static GameObject FindTaggedParent(Transform t)
    {
        return (FindTaggedParent(t.gameObject));
    }
    //=====================================Materials Functions==================================

    //Returns a list of all Materials in this GameObject or its Children 
    static public Material[] GetAllMaterials(GameObject go)
    {
        List<Material> mats = new List<Material>();
        if (go.GetComponent<Renderer>() != null)
        {
            mats.Add(go.GetComponent<Renderer>().material);
        }
        foreach (Transform t in go.transform)
        {
            mats.AddRange(GetAllMaterials(t.gameObject));
        }
        return (mats.ToArray());
    }
}


                
   
	
	
