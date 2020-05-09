using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof (Camera)), ExecuteAlways, DisallowMultipleComponent]
public class SSAOMask : MonoBehaviour
{
    public const int STENCI_GREEN = 3;
    public const int STENCI_RED = 6;

    [SerializeField] private Shader _maskShader3 = default;
    [SerializeField] private Shader _maskShader4 = default;

    private static Mesh _quadMesh3;
    private static Mesh _quadMesh4;
    private static readonly Matrix4x4 MatrixIdentity = Matrix4x4.identity;

    public OutlinePostEffect OutlinePostEffect;

    private Camera _camera;
    private CameraEvent _cameraEvent = CameraEvent.AfterForwardOpaque;
    private CommandBuffer _commandBuffer3;
    private CommandBuffer _commandBuffer4;

    public RenderTexture _maskTextureIndex3;
    public RenderTexture _maskTextureIndex4;
    public RenderTexture _bluredTextureIndex3;    
    public RenderTexture _bluredTextureIndex4;    
    public Material MaterialIndex3;
    public Material MaterialIndex4;
    private int _prevPixelHeight;
    private int _prevPixelWidth;
//    private static readonly int _ssaoMask = Shader.PropertyToID("_SSAOMask");

    private void Start()
    {
        if (_quadMesh3 == null)
            _quadMesh3 = GetQuadMesh();  
        if (_quadMesh4 == null)
            _quadMesh4 = GetQuadMesh();
        if (_maskShader3 == null)
            _maskShader3 = Shader.Find("Hidden/SSAOMask");  
        if (_maskShader4 == null)
            _maskShader4 = Shader.Find("Hidden/SSAOMask");
        _commandBuffer3 = new CommandBuffer {name = "SSAO Mask3"};
        _commandBuffer4 = new CommandBuffer {name = "SSAO Mask4"};
        _camera = GetComponent<Camera>();
        _camera.RemoveCommandBuffer(_cameraEvent, _commandBuffer3);
        _camera.RemoveCommandBuffer(_cameraEvent, _commandBuffer4);
        _camera.AddCommandBuffer(_cameraEvent, _commandBuffer3);    
        _camera.AddCommandBuffer(_cameraEvent, _commandBuffer4);
        MaterialIndex3 = new Material(_maskShader3);
        MaterialIndex3.SetInt("_StencilType", STENCI_GREEN);
        MaterialIndex4 = new Material(_maskShader4);
        MaterialIndex4.SetInt("_StencilType", STENCI_RED);
        _prevPixelHeight = _camera.pixelHeight;
        _prevPixelWidth = _camera.pixelWidth;
        SetupBuffer();
    }

    private void SetupBuffer()
    {
        if (_commandBuffer3 == null)
            return;    
        if (_commandBuffer4 == null)
            return;

        _commandBuffer3.Clear();
        _commandBuffer4.Clear();

        SetupTexture("SSAOMask _maskTextureIndex3 ", ref _maskTextureIndex3);
        SetupTexture("SSAOMask _bluredTextureIndex3 ", ref _bluredTextureIndex3);    
        SetupTexture("SSAOMask _maskTextureIndex4 ", ref _maskTextureIndex4);
        SetupTexture("SSAOMask _bluredTextureIndex4 ", ref _bluredTextureIndex4);
//        SetupTexture("SSAOMask _maskTextureIndex4 ", ref _maskTextureIndex4);



        OutlinePostEffect.masktexture3 = _maskTextureIndex3;
        OutlinePostEffect.blured3 = _bluredTextureIndex3;       
        OutlinePostEffect.masktexture4 = _maskTextureIndex4;
        OutlinePostEffect.blured4 = _bluredTextureIndex4;

        _commandBuffer3.SetRenderTarget(_maskTextureIndex3, BuiltinRenderTextureType.CurrentActive);
        _commandBuffer3.ClearRenderTarget(false, true, Color.clear);
        _commandBuffer3.DrawMesh(_quadMesh3, MatrixIdentity, MaterialIndex3);


        _commandBuffer4.SetRenderTarget(_maskTextureIndex4, BuiltinRenderTextureType.CurrentActive);
        _commandBuffer4.ClearRenderTarget(false, true, Color.clear);
        _commandBuffer4.DrawMesh(_quadMesh4, MatrixIdentity, MaterialIndex4);

//        _commandBuffer4.SetRenderTarget(_maskTextureIndex4, BuiltinRenderTextureType.CurrentActive);
//        _commandBuffer4.DrawMesh(_quadMesh4, MatrixIdentity, MaterialIndex3);
//        _commandBuffer4.ClearRenderTarget(false, true, Color.clear);

        Shader.SetGlobalTexture("_ssaoMask1", _maskTextureIndex3);
        Shader.SetGlobalTexture("_ssaoBlur1", _bluredTextureIndex3); ;    
        Shader.SetGlobalTexture("_ssaoMask2", _maskTextureIndex4);
        Shader.SetGlobalTexture("_ssaoBlur2", _bluredTextureIndex4); ;
    }

    private void SetupTexture(string name,ref RenderTexture txTexture)
    {

        if (txTexture != null)
            DestroyImmediate(txTexture);
        txTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 0, RenderTextureFormat.R8)
            { name = name + _camera.name };
    }

    private void OnPreCull()
    {
        if (_camera == null)
            return;

        if (_camera.pixelHeight != _prevPixelHeight || _camera.pixelWidth != _prevPixelWidth)
        {
            _prevPixelHeight = _camera.pixelHeight;
            _prevPixelWidth = _camera.pixelWidth;
            SetupBuffer();
        }
    }

    private void OnDestroy()
    {
        if (_commandBuffer3 != null)
            _camera.RemoveCommandBuffer(_cameraEvent, _commandBuffer3);     
        if (_commandBuffer4 != null)
            _camera.RemoveCommandBuffer(_cameraEvent, _commandBuffer4);

        if (_maskTextureIndex3 != null)
            DestroyImmediate(_maskTextureIndex3);    
        if (_maskTextureIndex4!= null)
            DestroyImmediate(_maskTextureIndex4);    
        if (_bluredTextureIndex3 != null)
            DestroyImmediate(_bluredTextureIndex3);    
        if (_bluredTextureIndex4 != null)
            DestroyImmediate(_bluredTextureIndex4);
    }

    public static Mesh GetQuadMesh(float z=0.1f)
    {
        //0 - 1
        //| \ |
        //2 - 3
        Vector3[] vertices =
        {
            new Vector3(-1, -1, z), new Vector3(1, -1, z), new Vector3(-1, 1, z), new Vector3(1, 1, z)
        };
        Vector2[] uv = {new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1)};
        int[] triangles =
        {
            0, 1, 3,
            3, 2, 0
        };
        Mesh mesh = new Mesh {vertices = vertices, uv = uv, triangles = triangles};
        mesh.RecalculateBounds();
        //mesh.hideFlags = HideFlags.DontSave;
        mesh.name = "QuadMeshForAmbientLight";
        return mesh;
    }
}