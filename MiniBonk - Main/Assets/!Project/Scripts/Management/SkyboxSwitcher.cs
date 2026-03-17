using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour
{
    // Assigns a random skybox each go

    public Material[] skyboxMaterials;

    private void Start()
    {
        SetSkybox();
    }

    void SetSkybox()
    {
        if (skyboxMaterials.Length == 0) return;

        var randomNumber = Random.Range(0, skyboxMaterials.Length);

        RenderSettings.skybox = skyboxMaterials[randomNumber];
        DynamicGI.UpdateEnvironment();
    }
}
