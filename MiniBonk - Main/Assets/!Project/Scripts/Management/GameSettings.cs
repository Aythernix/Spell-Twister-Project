using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Game Settings")]
    public bool showTutorial = true;
    
    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float gameplayVolume = 1f;
    [Range(0f, 1f)] public float enemyVolume = 1f;
    [Range(0f, 1f)] public float playerVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    public AudioMixer audioMixer;
    
    [Header("Visual Settings")]
    [Range(0f, 1f)] public float pixelisation = 0f;
    public RenderTexture renderTexture;
    
    [Header("Default Values")]
    [SerializeField] private float _defaultMasterVolume = 1f;
    [SerializeField] private float _defaultGameplayVolume = 1f;
    [SerializeField] private float _defaultEnemyVolume = 1f;
    [SerializeField] private float _defaultPlayerVolume = 1f;
    [SerializeField] private float _defaultMusicVolume = 1f;
    [SerializeField] private float _defaultPixelisation = 1f;
    [SerializeField] private int _defaultPixelWidth = 256;
    [SerializeField] private int _defaultPixelHeight = 144;
    [SerializeField] private AudioMixer _defaultAudioMixer;
    [SerializeField] private RenderTexture _defaultRenderTexture;
    
    private void OnEnable()
    {
        ResetToDefaults();
    }
    
    public void ResetToDefaults()
    {
        masterVolume = _defaultMasterVolume;
        gameplayVolume = _defaultGameplayVolume;
        enemyVolume = _defaultEnemyVolume;
        playerVolume = _defaultPlayerVolume;
        musicVolume = _defaultMusicVolume;
        pixelisation = _defaultPixelisation;
        audioMixer = _defaultAudioMixer;
        renderTexture = _defaultRenderTexture;
        
        ApplySettings();
        
        Debug.Log("GameSettings reset to default values");
    }

    private float LinearToDecibel(float linearValue)
    {
        return Mathf.Log10(linearValue) * 20;
    }
    
    public void ApplySettings()
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", LinearToDecibel(masterVolume));
            audioMixer.SetFloat("GameplayVolume", LinearToDecibel(gameplayVolume));
            audioMixer.SetFloat("EnemyVolume", LinearToDecibel(enemyVolume));
            audioMixer.SetFloat("PlayerVolume", LinearToDecibel(playerVolume));
            audioMixer.SetFloat("MusicVolume", LinearToDecibel(musicVolume));
        }

        ResizeRenderTexture(renderTexture, (int)(_defaultPixelWidth / pixelisation), (int)(_defaultPixelHeight / pixelisation));
        
       
    }
    
    void ResizeRenderTexture(RenderTexture rt, int newWidth, int newHeight)
    {
        if (rt == null) return;

        // 1. Release the current GPU resources
        rt.Release();

        // 2. Set new dimensions
        rt.width = newWidth;
        rt.height = newHeight;

        // 3. Re-create the texture with new dimensions
        rt.Create();
    }


}
