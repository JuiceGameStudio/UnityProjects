using UnityEngine;

public class Folder : MonoBehaviour
{
    private void Awake()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);
        }
        Destroy(gameObject);
    }
}
