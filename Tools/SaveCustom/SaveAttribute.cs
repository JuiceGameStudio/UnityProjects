using UnityEngine;

public class SaveAttribute : PropertyAttribute
{
    public string key;

    public SaveAttribute(string key)
    {
        this.key = key;
    }
}
