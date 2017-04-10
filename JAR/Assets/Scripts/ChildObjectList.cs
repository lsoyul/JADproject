using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObjectList : MonoBehaviour {

    public Dictionary<string, Transform> childList;
    public Transform centerPosObject;

	// Use this for initialization
	void Start () {
        childList = new Dictionary<string, Transform>();
        childList.Add("centerPos", centerPosObject);
    }
	
    public Dictionary<string, Transform> getChildList()
    {
        return childList;
    }
}
