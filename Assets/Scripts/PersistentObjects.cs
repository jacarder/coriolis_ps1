using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    public static PersistentObjects instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}
