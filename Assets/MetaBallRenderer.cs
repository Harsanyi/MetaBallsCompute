using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaBallRenderer : MonoBehaviour
{
    public bool isPlaying { get; private set; } = true;
    public int ballCount { get; private set; } = 5;

    public float lineThickness = 0.1f;
    public float lineDistance = 0.5f;
    public Color backgroundColor = new Color(0,0,0,1);
    public Color lineColor = new Color(1,1,1,1);
    public bool shaded = true;


    [Header("Objs")]
    [SerializeField] RawImage img = null;
    [SerializeField] ComputeShader shader = null;

    RenderTexture texture;
    MetaBall[] balls = new MetaBall[0];
    int kernelIndex = 0;
    ComputeBuffer ballBuffer;
    BallData[] ballDatas;

    private void Awake()
    {
        //Setup texture
        texture = new RenderTexture(Screen.width, Screen.height, 16);
        texture.name = $"{Screen.width}x{Screen.height}";
        texture.enableRandomWrite = true;
        texture.Create();
        img.texture = texture;

        //Setup shader
        kernelIndex = shader.FindKernel("RadiantColor");
        shader.SetTexture(kernelIndex, "result", texture);

        UpdateShaderSettings();
    }

    private void Update()
    {
        if (isPlaying) {
            UpdatePos();
            Render();
        }
    }

    private void OnDestroy()
    {
        ballBuffer.Dispose();
        Destroy(texture);
    }

    public void UpdateShaderSettings() {
        shader.SetFloat("_LineDistance", lineDistance);
        shader.SetFloat("_LineThickness", lineThickness);
        shader.SetFloats("_BackgroundColor", new float[] { backgroundColor.r, backgroundColor.g, backgroundColor.b, backgroundColor.a });
        shader.SetFloats("_LineColor", new float[] { lineColor.r, lineColor.g, lineColor.b, lineColor.a });
        shader.SetBool("_Shaded", shaded);
    }

    public void Play(bool play) {
        isPlaying = play;
    }

    public void UpdatePos() {
        for (int i = 0; i < ballDatas.Length; ++i)
        {
            balls[i].Move(Time.deltaTime);
            ballDatas[i] = (BallData)balls[i];
        }
    }

    public void SpawnBalls() {
        //Balls
        balls = new MetaBall[ballCount];
        ballDatas = new BallData[ballCount];

        for (int i = 0; i < balls.Length; ++i)
        {
            balls[i] = new MetaBall();
            balls[i].pos = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
            balls[i].AddMass(Random.Range(1, 20));
            balls[i].velocity = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f));
            ballDatas[i] = (BallData)balls[i];
        }

        //Buffers
        if (ballBuffer != null) ballBuffer.Dispose();
        ballBuffer = new ComputeBuffer(ballCount, BallData.size);
        shader.SetBuffer(kernelIndex,"balls",ballBuffer);       
    }

    public void Render()
    {
        ballBuffer.SetData(ballDatas);
        shader.Dispatch(kernelIndex, texture.width / 8 + 1, texture.height / 8 + 1, 1);
    }

    public void SetBallCount(int count) {
        ballCount = count;
        SpawnBalls();
    }
}
