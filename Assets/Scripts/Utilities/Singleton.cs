using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (FindObjectsOfType(typeof(T)).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = (T)FindObjectOfType(typeof(T));
            DontDestroyOnLoad(gameObject);
        }
    }
}