using CustomSystem.SaverLoader;
using UnityEngine;

public abstract class Saveable<T> : MonoBehaviour where T : Saveable<T>
{
    public void Load()
    {
        SaverLoader.Load(this as T);
    }

    public void Save()
    {
        SaverLoader.Save(this as T);
    }

    protected virtual void Awake() 
    {
        SaverLoader.OnLoad += Load;
        SaverLoader.OnSave += Save;
    }
}
