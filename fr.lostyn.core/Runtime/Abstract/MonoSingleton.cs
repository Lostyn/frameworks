using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : Component {
    #region single implementation
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();

            return instance;
        }
    }
    #endregion
}