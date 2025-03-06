using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    void Start()
    {
        // Call the native function through the MyPlugin class
        int result = MyPlugin.Add(5, 10);
        Debug.Log("Result from native plugin: " + result);
    }
}