using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShavePath : MonoBehaviour
{
    public CustomRenderTexture _splatmap;

    public Material _drawMaterial;
    public Material _snowMaterial;

    public Transform _drawer;

    RaycastHit _groundHit;
    int _layerMask;

    [Range(0, 1000)]
    public float _brushSize;
    [Range(0, 50)]
    public float _brushStrength;

    private void Awake()
    {
        _splatmap = new CustomRenderTexture(2048, 2048, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

        _splatmap.updateMode = CustomRenderTextureUpdateMode.OnDemand;
        _splatmap.material = _drawMaterial;
        _splatmap.initializationSource = CustomRenderTextureInitializationSource.Material;
        _splatmap.initializationMaterial = _drawMaterial;
        _splatmap.doubleBuffered = true;

        _drawMaterial.SetVector("_DrawColor", Color.red);
        _drawMaterial.SetTexture("_SplatMap", _splatmap);
        _snowMaterial.SetTexture("_SplatMap", _splatmap);
    }

    void Start()
    {
        _layerMask = LayerMask.GetMask("Ground");
        StartCoroutine(DrawEachWheel());
    }

    IEnumerator DrawEachWheel()
    {
        _drawMaterial.SetFloat("_BrushStrength", _brushStrength);
        _drawMaterial.SetFloat("_BrushSize", _brushSize);
        _splatmap.Initialize();

        if (Physics.Raycast(_drawer.position, Vector3.down, out _groundHit, 1f, _layerMask))
        {
            string wheelCoord = "_ShaveCoord";
            Debug.DrawRay(_drawer.position, Vector3.down, Color.red);
            _drawMaterial.SetVector(wheelCoord, new Vector4(_groundHit.textureCoord.x, _groundHit.textureCoord.y, 0, 0));
        }

        _splatmap.Update();

        yield return new WaitForFixedUpdate();

        StartCoroutine(DrawEachWheel());
    }
}
