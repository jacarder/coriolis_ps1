using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObjects : MonoBehaviour
{
    private static GameObject instance;
    void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = gameObject;
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Neoptra_Spaceport");
    }
}
