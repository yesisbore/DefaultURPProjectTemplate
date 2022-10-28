using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWebCamTexture : MonoBehaviour
{
    #region GetWebCamTexture

    // 몇번째 카메라를 사용할것 인가 ? 
    public int deviceIndex = 0;

    public int width = 128;
    public int height = 128;
    public int fps = 30;

    public RenderTexture renderTexture;

    private WebCamTexture _webCamTexture;

    private void SetWebCamTexture(int index)
    {
        if (_webCamTexture != null && _webCamTexture.isPlaying) _webCamTexture.Stop();

        WebCamDevice[] devices = WebCamTexture.devices;
        try
        {
            _webCamTexture = new WebCamTexture(devices[index].name, width, height, fps);
        }
        catch (Exception e)
        {
            _webCamTexture = new WebCamTexture(devices[0].name, width, height, fps);
        }

        _webCamTexture.Play();
    }

    #endregion

    #region MyRegion

    public RenderTexture frame01;
    private Texture2D _frame01Tex2D;

    public RenderTexture frame02;
    private Texture2D _frame02Tex2D;

    // 결과 텍스쳐  
    public RenderTexture resultRenderTexture;
    private Texture2D _resultRenderTex2D;

    private Color[] _result;
    [Range(0f, 1f)] public float threshold;

    private Color[] _frame01Tex2DColors;
    private Color[] _frame02Tex2DColors;

    #endregion

    public void Start()
    {
        SetWebCamTexture(deviceIndex);
        if (renderTexture == null) return;

        // 렌더 텍스쳐를 Texture2D로 변환해서 픽셀 연산을 진행
        _frame01Tex2D = new Texture2D(frame01.width, frame01.height);
        _frame02Tex2D = new Texture2D(frame02.width, frame02.height);

        // 결과값을 저장함 
        _resultRenderTex2D = new Texture2D(frame02.width, frame02.height);
        _result = new Color[width * height];

        _frame01Tex2DColors = new Color[width * height];
        _frame02Tex2DColors = new Color[width * height];
        // 무한 코루틴을 이용해 시간 차가 있는 웹캠 텍스쳐를 실시간으로 받아온다.
        StartCoroutine(GetFrameTexure());
    }

    // public void Update()
    // {
    //     Graphics.Blit(_webCamTexture, renderTexture);
    // }

    IEnumerator GetFrameTexure()
    {
        var waitTime = new WaitForSeconds(0.03f);
        while (true)
        {
            Graphics.Blit(_webCamTexture, frame01);
            _frame01Tex2D = ToTexture2D(frame01);
            _frame01Tex2DColors = _frame01Tex2D.GetPixels();

            // 한프레임 정도 쉬고 
            yield return waitTime;

            // 다음 프레임의 웹켐 텍스쳐를 받아온다.
            Graphics.Blit(_webCamTexture, frame02);
            _frame02Tex2D = ToTexture2D(frame02);
            _frame02Tex2DColors = _frame02Tex2D.GetPixels();

            #region 01. 차연산을 바로 텍스쳐에 적용해 줄때

            // for (int i = 0; i < _result.Length; i++)
            // {
            //     Color subColor = _frame01Tex2DColors[i] - _frame02Tex2DColors[i];
            //
            //     // 그레이 스케일 변환
            //     float sum = subColor.r + subColor.g + subColor.b;
            //     subColor.r = sum / 3;
            //     subColor.g = sum / 3;
            //     subColor.b = sum / 3;
            //     subColor.a = 1f;
            //
            //     // 이진화
            //     if (subColor.maxColorComponent < threshold)
            //     {
            //         subColor = Color.black;
            //     }
            //     else
            //     {
            //         subColor = Color.white;
            //     }
            //
            //     _result[i] = subColor;
            // }

            #endregion

            #region 2, 보간을 넣어줄때

            for (int i = 0; i < _result.Length; i++)
            {
                Color subColor = _frame01Tex2DColors[i] - _frame02Tex2DColors[i];

                // 그레이 스케일 변환
                float sum = subColor.r + subColor.g + subColor.b;
                subColor.r = sum / 3;
                subColor.g = sum / 3;
                subColor.b = sum / 3;
                subColor.a = 1f;

                // 이진화
                if (subColor.maxColorComponent < threshold)
                {
                    subColor = Color.black;
                }
                else
                {
                    subColor = Color.white;
                }

                _result[i] = Color.Lerp(_result[i], subColor, 1.0f - Mathf.Exp(-7 * Time.deltaTime));
            }

            #endregion
            _resultRenderTex2D.SetPixels(_result);
            _resultRenderTex2D.Apply();

            RenderTexture.active = resultRenderTexture;
            Graphics.Blit(_resultRenderTex2D, resultRenderTexture);
            yield return null;
        }
    }

    private Texture2D ToTexture2D(RenderTexture rTexture)
    {
        Texture2D resultTexture2D =
            new Texture2D(rTexture.width, rTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTexture;
        resultTexture2D.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0);

        resultTexture2D.Apply();
        return resultTexture2D;
    }
}