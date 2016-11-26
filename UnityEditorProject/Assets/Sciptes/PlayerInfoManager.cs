using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerInfoManager : MonoBehaviour {
    public string jsonPath;
    public PlayerInfo player_info;
	// Use this for initialization
	void Start () {
        player_info = new PlayerInfo();
        player_info = ReadJson<PlayerInfo>(jsonPath);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public T ReadJson<T>(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (!File.Exists(Application.dataPath + "/Resources/" + path))
        {
            return default(T);
        }

        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/" + path);

        

        if (sr == null)
        {
            return default(T);
        }
        string json = sr.ReadToEnd();

        if (json.Length > 0)
        {
            return JsonUtility.FromJson<T>(json);
        }

        return default(T);
    }
}
