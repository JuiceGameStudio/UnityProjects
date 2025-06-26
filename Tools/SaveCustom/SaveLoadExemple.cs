using UnityEngine;

public class SaveLoadExemple : Saveable<SaveLoadExemple>
{
    [Save("stringTest")]
    public string exempleString;
    [Save("floatTest")]
    public float exemplefloat;
    [Save("intTest")]
    public int exempleInt;

    [ContextMenu("Save")]
    public void TSave()
    {
        Save();
    }

    [ContextMenu("Load")]
    public void TLoad()
    {
        Load();
    }
}
