using Structs;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// General variables that we only need one instance of and that are present in multiple files
public static class Indestructibles
{
    // Camera stuff
    public static readonly PostProcessVolume Volume = GameObject.Find("Camera").GetComponent<PostProcessVolume>();

    // Scoring stuff
    public static int ScoreMultiplier = 1;

    // Time stuff
    public const float SoberingTime = 20.0f;

    // Movement stuff
    public static KeyControls Controls =
        new KeyControls(KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A);
}