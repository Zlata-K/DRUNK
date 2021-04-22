using Player;
using Structs;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// General variables that we only need one instance of and that are present in multiple files
public static class Indestructibles
{
    // Camera stuff
    public static readonly PostProcessVolume Volume = GameObject.Find("Camera").GetComponent<PostProcessVolume>();
    
    // Time stuff
    public const float SoberingTime = 20.0f;

    // Player stuff
    public static readonly GameObject Player = GameObject.Find("player");
    public static readonly Renderer[] Renderers = Player.GetComponentsInChildren<Renderer>();
    public static PlayerData PlayerData = Player.GetComponent<PlayerDataManager>().PlayerData;
    public static readonly Animator PlayerAnimator = Player.GetComponent<Animator>();
    
    // Movement stuff
    public static MovementControls MovementControls =
        new MovementControls(KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A);
    // Debug stuff
    public static DebugControls DebugControls =
        new DebugControls(KeyCode.U, KeyCode.I, KeyCode.O);
    public static bool DebugEnabled = true;
}