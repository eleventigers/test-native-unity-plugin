using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyPlugin : MonoBehaviour
{
    // Import the native function with platform-specific library names
#if UNITY_WEBGL && !UNITY_EDITOR
    private const string LibraryName = "__Internal"; // WebAssembly
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private const string LibraryName = "my_plugin"; // Windows
#elif UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
    private const string LibraryName = "libmy_plugin"; // Linux
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    private const string LibraryName = "libmy_plugin"; // macOS
#else
    private const string LibraryName = ""; // Unsupported platform
#endif

    [DllImport(LibraryName, EntryPoint = "Add")]
    private static extern int AddInternal(int a, int b);

    // Unified method to call the native function
    public static int Add(int a, int b)
    {
        try
        {
            return AddInternal(a, b);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to call native function: " + e.Message);
            return -1;
        }
    }
}
