using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public GameObject TurnObject;
    
    public float turnvalue = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LeftTurn()
    {
        TurnObject.transform.Rotate(Vector3.up,turnvalue);
    }
    public void RightTurn()
    {
        TurnObject.transform.Rotate(Vector3.down, turnvalue);
    }
}
