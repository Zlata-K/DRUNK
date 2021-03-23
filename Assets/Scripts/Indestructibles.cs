using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public static class Indestructibles
{
    // Camera stuff
    public static readonly PostProcessVolume Volume = GameObject.Find("Camera").GetComponent<PostProcessVolume>();

    // Scoring stuff
    public static int ScoreMultiplier = 1;
    
    // Time stuff
    public const float SoberingTime = 20.0f;
}