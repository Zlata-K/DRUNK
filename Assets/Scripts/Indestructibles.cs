using NPCs.Flocking;
using Player;
using Structs;
using UI;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

// General variables that we only need one instance of and that are present in multiple files
public static class Indestructibles
{
    //Flocking stuff
    public static FlockManager FlockManagerInstance;
    
    // Camera stuff
    public static PostProcessVolume Volume;
    
    // Time stuff
    public const float SoberingTime = 60.0f;

    // Player stuff
    public static GameObject Player;
    public static Renderer[] Renderers;
    public static PlayerData PlayerData;
    public static Animator PlayerAnimator;
    
    // Movement stuff
    public static MovementControls MovementControls;
    
    // Debug stuff
    public static DebugControls DebugControls;
    public static bool DebugEnabled;
    
    // UI Stuff
    public static UIManager UIManager;

    public static void SetDefaultValues()
    {
        FlockManagerInstance = GameObject.Find("NPCMovementCoordinator").GetComponent<FlockManager>();
        
        UIManager = GameObject.Find("UI").GetComponent<UIManager>();
        Volume = GameObject.Find("Camera").GetComponent<PostProcessVolume>();
        
        Player = GameObject.Find("player");
        Renderers = Player.GetComponentsInChildren<Renderer>();
        PlayerData = Player.GetComponent<PlayerDataManager>().PlayerData;
        PlayerAnimator = Player.GetComponent<Animator>();
        
        MovementControls = new MovementControls(KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A);
        
        DebugControls = new DebugControls(
            KeyCode.V,
            KeyCode.B,
            KeyCode.N,
            KeyCode.M,
            KeyCode.I,
            KeyCode.O);
        DebugEnabled = true;
    }
}