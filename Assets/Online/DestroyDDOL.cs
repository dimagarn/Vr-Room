using UnityEngine;

public class DestroyDDOL : MonoBehaviour
{
    public static DestroyDDOL Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
