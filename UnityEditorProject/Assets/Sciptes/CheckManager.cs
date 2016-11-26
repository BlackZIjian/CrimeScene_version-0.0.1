using UnityEngine;
using System.Collections;

public class CheckManager : MonoBehaviour {
    PlayerInfoManager m_play_info_manager;
	// Use this for initialization
	void Start () {
        m_play_info_manager = GameObject.Find("PlayerInfoManager").GetComponent<PlayerInfoManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool isCanInitModel(GameObject model)
    {

        return true;
    }
}
