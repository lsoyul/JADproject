using UnityEngine;
using System.Collections;

// Define ManagerTemplate for Entire Managers
// All Managers have to inherit this ManagerTemplate (except: CnInputManager)

public abstract class ManagerTemplate<T> : MonoBehaviour where T : ManagerTemplate<T>
{
    public static bool IsInitialized { private set; get; }

    private static T instance = null;

    public static T Instance
    {
        get
        {
            if(IsInitialized == false)
            {
                Debug.Log("Have to Instantiate the Manager!");
                return null;
            }

            return instance;
        }
    }

    public static bool Create()
    {
        if (instance != null)
        {
            Debug.LogError("This manager is already exist...");
            return false;
        }
        else
        {
            instance = GameObject.FindObjectOfType(typeof(T)) as T;     // Is there this manager in this scene??

            if(instance != null)
            {
                Debug.LogError("This manager is already exist...");
                return false;
            }

            // Try to allcate the manager
            instance = new GameObject("[Manager]" + typeof(T).ToString(), typeof(T)).GetComponent<T>();

            if (instance == null)
            {
                Debug.LogError("Fail to create : " + typeof(T).ToString());
                return false;
            }


            // Successfully Create the manager
            IsInitialized = true;
            instance.Init();
            DontDestroyOnLoad(instance.gameObject);
        }

        return true;
    }

    protected virtual void Init() { }

    // Make sure the instance isn't referrenced anymore when the user quit, just in case.
    protected virtual void OnDestroy()
    {
        IsInitialized = false;
        instance = null;
    }

    void OnApplicationQuit()
    {
        IsInitialized = false;
        instance = null;
    }
}
