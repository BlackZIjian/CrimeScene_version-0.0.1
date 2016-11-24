using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PlayerInfo {
    public string user_name;
    public string role_name;
    public int total_save_num;
    public int used_save_num;
    public int level;
    public string[] saved_clue_names;
}

[Serializable]
public class Clue
{
    public string[] pri_clue_names;
    public string clue_name;
    public int level;
    public Texture2D screenshot;
    public string discreption;
}
