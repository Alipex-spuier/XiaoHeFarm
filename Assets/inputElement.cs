using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class inputElement
{   
    public bool risingEdge=false;
    public bool longPress=false;
    public bool fallingEdge=false;

    public void releaseEdges()
    {
        longPress = false;
        fallingEdge = true;
    }
    public void resetEdges()
    {
        risingEdge = false;
        fallingEdge = false;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
