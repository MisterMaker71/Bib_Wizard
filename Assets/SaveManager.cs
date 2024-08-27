using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public Saves saves = new Saves();
    void Start()
    {
        Getsavebels();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SaveGame("Main");
        if (Input.GetKeyDown(KeyCode.F2))
            LoadGame("Main");
    }

    public void Getsavebels()
    {
        saves.savebels.Clear();
        foreach (Savebel item in FindObjectsOfType<Savebel>())
        {
            //saves.savebels.Add(new Savebel(item));
        }
    }
    public void SaveGame(string SaveName)
    {
        System.DateTime t = System.DateTime.Now;
        Debug.Log("Saving Game Save...");
        if (!Directory.Exists(Application.dataPath + "/Saves/"))
            Directory.CreateDirectory(Application.dataPath + "/Saves/");

        if (!File.Exists(Application.dataPath + "/Saves/" + SaveName + ".mssd"))
            File.Create(Application.dataPath + "/Saves/" + SaveName + ".mssd");

        string js = "";

        js = JsonUtility.ToJson(saves);

        File.WriteAllText(Application.dataPath + "/Saves/" + SaveName + ".mssd", js);

        Debug.Log("Completed in " + (System.DateTime.Now - t).TotalSeconds + " s");
    }
    public void LoadGame(string SaveName)
    {
        System.DateTime t = System.DateTime.Now;
        Debug.Log("Loading Game Save...");
        if (Directory.Exists(Application.dataPath + "/Saves/"))
        {
            if (File.Exists(Application.dataPath + "/Saves/" + SaveName + ".mssd"))
            {
                string j = File.ReadAllText(Application.dataPath + "/Saves/" + SaveName + ".mssd");
                Saves saves2 = JsonUtility.FromJson<Saves>(j);
                for (int i = 0; i < saves.savebels.Count; i++)
                {
                    saves.savebels[i].Load(saves2.savebels[i]);
                }
            }
        }
        Debug.Log("Completed in " + (System.DateTime.Now - t).TotalSeconds + " s");
    }

}
[System.Serializable]
public class Saves
{
    public List<Savebel> savebels = new List<Savebel>();
}