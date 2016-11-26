using UnityEngine;
using System.Collections;

public class MoveModel : MonoBehaviour {
    public Transform m_model;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(CheckShaked())
        {
            m_model.position = transform.position;
            m_model.rotation = transform.rotation;
        }
	}

    bool CheckShaked()
    {
        return true;
    }

    public void OnFoundModel()
    {

    }

    public void OnLostModel()
    {

    }
}
