using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Savebel : MonoBehaviour
{
    public string saveKey;
    virtual public void Load(Savebel savebel)
    {
        
    }
    public Savebel(Savebel savebel)
    {
        saveKey = savebel.saveKey;
    }
}
