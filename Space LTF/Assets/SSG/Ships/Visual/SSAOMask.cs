using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof (Camera)), ExecuteAlways, DisallowMultipleComponent]
public class SSAOMask : MonoBehaviour
{
    [SerializeField] private Shader _maskShader = default;

    private static Mesh _quadMesh;
    private static readonly Matrix4x4 MatrixIdentity = Matrix4x4.identity;

    private Camera _camera;
    private CameraEvent _cameraEvent = CameraEvent.BeforeLighting;
    private CommandBuffer _commandBuffer;
    public OutlinePostEffect OutlinePostEffect;

    public RenderTexture _maskTexture;
    public RenderTexture _bluredTexture;
    private Material _material;
    private int _prevPixelHeight;
    private int _prevPixelWidth;
    private static readonly int _ssaoMask = Shader.PropertyToID("_SSAOMask");

    private void Start()
    {
        if (_quadMesh == null)
            _quadMesh = GetQuadMesh();
        if (_maskShader == null)
            _maskShader = Shader.Find("Hidden/SSAOMask");
        _commandBuffer = new CommandBuffer();
        _commandBuffer.name = "SSAO Mask";
        _camera = GetComponent<Camera>();
        _camera.RemoveCommandBuffer(_cameraEvent, _commandBuffer);
        _camera.AddCommandBuffer(_cameraEvent, _commandBuffer);
        _material = new Material(_maskShader);
        _prevPixelHeight = _camera.pixelHeight;
        _prevPixelWidth = _camera.pixelWidth;
        SetupBuffer();
    }

    private void SetupBuffer()
    {
        if (_commandBuffer == null)
            return;

        _commandBuffer.Clear();

        if (_maskTexture != null)
            DestroyImmediate(_maskTexture);

        _maskTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 0, RenderTextureFormat.R8)
            { name = "SSAOMask _maskTexture " + _camera.name };
        _bluredTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 0, RenderTextureFormat.R8)
            { name = "SSAOMask _bluredTexture " + _camera.name };

        OutlinePostEffect.srcrender = _maskTexture;
        OutlinePostEffect.destrender = _bluredTexture;
        _commandBuffer.SetRenderTarget(_maskTexture, BuiltinRenderTextureType.CurrentActive);
        _commandBuffer.ClearRenderTarget(false, true, Color.white);
        _commandBuffer.DrawMesh(_quadMesh, MatrixIdentity, _material);

        Shader.SetGlobalTexture(_ssaoMask, _maskTexture);
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
        if (_commandBuffer != null)
            _camera.RemoveCommandBuffer(_cameraEvent, _commandBuffer);

        if (_maskTexture != null)
            DestroyImmediate(_maskTexture);
    }

    public static Mesh GetQuadMesh(float z=0.1f) // I stole it from ambient light. Need to move in some helper.
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