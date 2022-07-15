using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public enum Allegiance { 
        Friendly,
        Enemy
    }
    public GameObject unitParent;
    public Vector3 location {
        get { return unitParent.transform.position; }
        }
    public float Width;
    public float Height;
    public float Depth;
}
