using System;
using UnityEngine;

public class ParallaxTexture : MonoBehaviour {
    public ParallaxTexture() { }

    private void Awake() {
        this.mainCameraTransform = ReplaceableSingleton<CameraManager>.main.mainCameraTransform;
        Renderer renderer = base.GetComponent<Renderer>();
        this.texture = renderer.material.GetTexture("_MainTex");
        Material newMaterial = new Material(Shader.Find("Custom/WorldShadowShader"));
        newMaterial.CopyPropertiesFromMaterial(renderer.material);
        renderer.material = newMaterial;
        this.cMaterial = renderer.material;
        this.defaultScale = this.container.localScale;
        this.defaultZoom = ReplaceableSingleton<CameraManager>.main.mainCamera.orthographicSize;
    }

    private void Update() {
        if (base.gameObject.name == "Cavern Front") {
            this.offsetFactor = 0.0025f;
            this.cMaterial.SetTexture("_MainTex", this.texture);
            this.cMaterial.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 0.5f));
        }
        if (base.gameObject.name == "Cavern") {
            this.offsetFactor = 0.01f;
            this.cMaterial.SetTexture("_MainTex", this.texture);
            this.cMaterial.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            base.transform.GetChild(1).gameObject.SetActive(false);
        }
        this.cMaterial.mainTextureOffset = new Vector2(this.mainCameraTransform.position.x * this.offsetFactor, this.mainCameraTransform.position.y * this.offsetFactor);
        float num = 1f / (1f + (ReplaceableSingleton<CameraManager>.main.mainCamera.orthographicSize / this.defaultZoom - 1f) * 0.3f);
        this.container.localScale = new Vector3(this.defaultScale.x * num, this.defaultScale.y * num, 1f);
    }

    public Transform container;
    public float offsetFactor = 0.01f;
    private Transform mainCameraTransform;
    private Material cMaterial;
    private Vector2 defaultScale;
    private float defaultZoom;
    private Texture texture;
}