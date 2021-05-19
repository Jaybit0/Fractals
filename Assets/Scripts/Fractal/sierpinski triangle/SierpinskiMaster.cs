using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierpinskiMaster : MonoBehaviour
{
    public ComputeShader fractalShader;

    [Range(1, 200)]
    public float darkness = 50;
    [Range(1, 5)]
    public float scale = 1.45f;
    [Range(1, 70)]
    public int iterations = 30;

    [Header("Colour mixing")]
    [Range(0, 1)] public float blackAndWhite;
    [Range(0, 1)] public float redA;
    [Range(0, 1)] public float greenA;
    [Range(0, 1)] public float blueA = 1;

    RenderTexture target;
    Camera cam;
    Light directionalLight;

    [Header("Animation Settings")]
    public float scaleIncreaseSpeed = 0.05f;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Init()
    {
        cam = Camera.current;
        directionalLight = FindObjectOfType<Light>();
    }

    // Animate properties
    void Update()
    {
        if (Application.isPlaying)
        {
            scale += scaleIncreaseSpeed * Time.deltaTime;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Init();
        InitRenderTexture();
        SetParameters();

        int threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 8.0f);
        fractalShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(target, destination);
    }

    void SetParameters()
    {
        fractalShader.SetTexture(0, "Destination", target);
        fractalShader.SetFloat("darkness", darkness);
        fractalShader.SetFloat("blackAndWhite", blackAndWhite);
        fractalShader.SetVector("colourAMix", new Vector3(redA, greenA, blueA));

        fractalShader.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        fractalShader.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);
        fractalShader.SetVector("_LightDirection", directionalLight.transform.forward);

        // Own
        fractalShader.SetFloat("scale", scale);
        fractalShader.SetInt("iterations", iterations);

    }

    void InitRenderTexture()
    {
        if (target == null || target.width != cam.pixelWidth || target.height != cam.pixelHeight)
        {
            if (target != null)
            {
                target.Release();
            }
            target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }
}
