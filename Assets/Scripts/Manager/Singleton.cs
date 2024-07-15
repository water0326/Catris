using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    private static T _instance = null;
    public static T Instance {
        get {
            _instance = FindObjectOfType(typeof(T)) as T;
            if( _instance == null ) {
                Debug.Log("No Singleton object");
            }
            return _instance;
        }
        
    }

    private void Awake() {
        if (_instance == null) {
            _instance = gameObject.GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
