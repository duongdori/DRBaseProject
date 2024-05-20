using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            _instance = FindObjectOfType<T>();

            if (_instance != null) return _instance;
            
            Debug.LogWarning($"No instance of {typeof(T).ToString()}, a temporary one is created.");
            _instance = new GameObject($"Temp Instance of {typeof(T).ToString()}").AddComponent<T>();
                    
            if (_instance == null)
            {
                Debug.LogError($"Problem during the creation of {typeof(T).ToString()}");
            }
            
            _instance.Initiate();
            return _instance;
        }
    }
    
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (_instance != null) _instance.Initiate();
        }
        else
        {
            if (this != _instance)
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual void Initiate()
    {
        Debug.Log($"Initiate {typeof(T).ToString()}");
        DontDestroyOnLoad(gameObject);
    }
    
    private void OnApplicationQuit()
    {
        _instance = null;
    }
    
}
