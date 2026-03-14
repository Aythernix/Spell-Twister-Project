using Minibonk.Management;
using Minibonk.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameSettings _gameSettings;
    
    // Volume Sliders
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _gameplayVolumeSlider;
    [SerializeField] private Slider _playerVolumeSlider;
    [SerializeField] private Slider _enemyVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    
    // Visual Sliders
    [SerializeField] private Slider _pixelisationSlider;
    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadSettings()
    {
        _masterVolumeSlider.value = _gameSettings.masterVolume;
        _gameplayVolumeSlider.value = _gameSettings.gameplayVolume;
        _playerVolumeSlider.value = _gameSettings.playerVolume;
        _enemyVolumeSlider.value = _gameSettings.enemyVolume;
        _musicVolumeSlider.value = _gameSettings.musicVolume;
        _pixelisationSlider.value = _gameSettings.pixelisation;
    }
    
    // Volume
    public void MaserVolumeChanged(float volume)
    {
        _gameSettings.masterVolume = volume;
        _gameSettings.ApplySettings();
    }
    public void GameplayAudioChanged(float volume)
    {
        _gameSettings.gameplayVolume = volume;
        _gameSettings.ApplySettings();
    }
    public void PlayerAudioChanged(float volume)
    {
        _gameSettings.playerVolume = volume;
        _gameSettings.ApplySettings();
    }
    public void EnemyAudioChanged(float volume)
    {
        _gameSettings.enemyVolume = volume;
        _gameSettings.ApplySettings();
    }
    public void MusicAudioChanged(float volume)
    {
        _gameSettings.musicVolume = volume;
        _gameSettings.ApplySettings();
    }
    
    // Visual
    public void PixelisationChanged(float pixelisation)
    {
        _gameSettings.pixelisation = pixelisation;
    }
    
    public void ResetSettings()
    {
        _gameSettings.ResetToDefaults();
        LoadSettings();
    }
    
    public void CloseSettings()
    {
        _gameSettings.ApplySettings();
        GameManager.Instance.uiManager.UIEvents.OnCloseUIScene?.Invoke(UISceneNames.Settings, false);
    }
}
