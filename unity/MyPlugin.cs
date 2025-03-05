using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyPlugin : MonoBehaviour
{
    // Platform-specific library names
    private const string WindowsLibrary = "my_plugin";
    private const string LinuxLibrary = "libmy_plugin";
    private const string MacLibrary = "libmy_plugin";
    private const string WebAssemblyLibrary = "__Internal";

    // Import the native plugin
    [DllImport(WindowsLibrary, EntryPoint = "Add")]
    private static extern int AddWindows(int a, int b);

    [DllImport(LinuxLibrary, EntryPoint = "Add")]
    private static extern int AddLinux(int a, int b);

    [DllImport(MacLibrary, EntryPoint = "Add")]
    private static extern int AddMac(int a, int b);

    [DllImport(WebAssemblyLibrary, EntryPoint = "Add")]
    private static extern int AddWebAssembly(int a, int b);

    // Unified method to call the native function
    public static int Add(int a, int b)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return AddWebAssembly(a, b);
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            return AddWindows(a, b);
        }
        else if (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            return AddLinux(a, b);
        }
        else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
        {
            return AddMac(a, b);
        }
        else
        {
            throw new PlatformNotSupportedException("Platform not supported: " + Application.platform);
        }
    }

    void Start()
    {
        int result = Add(5, 10);
        Debug.Log("Result from native plugin: " + result);
    }
}