Shader "Custum/DefaultShader"
{
    Properties
    {
        _MainTex("MainTexture",2D) = "white"{}
        _MainColor("MainColor",color) = (1,1,1,1)
        //_Intensity("Intensity",Range(0,1)) = 0.5
        //float _Intensity;
        
        // Blend Operation 을 토글로 바꿔줌 
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend",Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend",Float) = 0
        
        // Culling 
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode",Float) = 1

        // z
        [Enum(Off,0, On,1)] _ZWrite ("ZWrite",Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest",Float) = 0

        // 에트리뷰트
        [Space(20)]
        [Toggle] _AlphaOn("AlphaTest",float) = 1
    } 

    SubShader
    {
        Tags
        {
            // Render type과 Render Queue를 여기서 결정합니다.
            "RenderPipeline"="UniversalPipeline"
            
            "RenderType"="Opaque"
            // Opaque, Transparent, TransparentCutout, Background, Overlay 
            // (터레인 랜더타입)TreeOpaque. TreeTransparentCutout, TreeBillboard, Grass, GrassBillboard
            
            "Queue"="Geometry"
            // 2500 까지는 불투명메시 이후로 투명으로 간주 ( Geometry + 500)
            // Background(1000), Geometry(2000), AlphaTest(2450), Transparent(3000), Overlay(4000)
        } 
        
        Pass
        {
            // Transparent 
            // Blend [_SrcBlend] [_DstBlend]- Src 계산된 컬러 (소스 값), Dst 이미 화면에 표시된 컬러( 배경값)
            //Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency
            //Blend One OneMinusSrcAlpha      // Premultiplied transparency
            //Blend One One                   // Additive
            //Blend OneMinusDstColor One      // Soft Additive
            //Blend DstColor Zero             // Multiplicative
            //Blend DstColor SrcColor         // 2x Multiplicative
            
            // Culling 
            // Cull back front off [_Cull]
            
            // ZWrite on off [_ZWrite]
            
            // ZTest - 오브젝트가 겹치면 어떻게 처리할지 기본값은 LEqual ( 기존 오브젝트와 같은 거리나 그 앞에 있는 오브젝트를 드로우하고 그뒤는 숨김 )
            // ZTest Less Greater LEqual GEqual Equal NotEqual Always [_ZTest]
            
            //Offset 두개의 오브젝트가 겹치면 특정 오브젝트의 폴리곤을 조정하는거
            //Offset 0,-1 
            
            //AlphaToMask On - 알파텍스쳐에 안티에일리어싱 적용시 사용 ( VR)
            
            //Name "ShadowCaster"
            //Tags
            //{
            //    "LightMode" = "ShadowCaster"
            //}
            //Name "GBuffer"
            //Tags
            //{
            //    "LightMode" = "UniversalGBuffer"
            //}
            //Name "DepthOnly"
            //Tags
            //{
            //    "LightMode" = "DepthOnly"
            //}
            //Name "Universal Forward"
            //Tags
            //{
            //    "LightMode" = "UniversalForward"
            //}
            

            HLSLPROGRAM // HLSL 시작
            // 컴파일러 지시자, 전처리기 
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag

            // cg shader는 .cginc를 hlsl shader는 .hlsl을 include하게 됩니다.
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // vertex buffer에서 읽어올 정보를 선언합니다.   
            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                // 앞의 이름은 상관없고 뒤의 Semantic은 문법이라 지켜줘여함 
                // float3 normal : NORMAL 
                // float4 tangent : TANGENT
                // float4 color : COLOR
            };

            // 보간기를 통해 버텍스 셰이더에서 픽셀 셰이더로 전달할 정보를 선언합니다.
            struct VertexOutput
            {
                float4 vertex : SV_POSITION; // 투영공간으로 변환된 후의 버텍스 포지션
                float2 uv : TEXCOORD0;
                // float3 normal : NORMAL 뷰 공간으로 변형된 후 버텍스의 노멀값
                // float4 tangent : TANGENT
                // diff,spec : COLOR
                // Any : 사용자 정의 Vertex stage에서 계산한 값을 Pixel shader로 전달할 때 임의로 정의할 수 있다.
            };

            // 변수 사용 
            half4 _MainColor;
            sampler2D _MainTex;
            float4 _MainTex_ST; // 타일링 scale 과 offset을 한번에 가져옴
            
            // Separate textures and Samples
            // (분리해서 사용하면 더 많은 텍스쳐를 사용가능) OpenGL ES2.0 이상에서 사용
            //Texture2D _MainTex1;
            //Texture2D _MainTex2;
            //SamplerState sampler_MainTex; Sampler + MainTex
            //half4 color = _MainTex1.Sampler(sampler_MainTex.uv);
            //color += _MainTex2.Sampler(sampler_MainTex.uv);

            
            // 버텍스 셰이더
            VertexOutput vert(VertexInput v)
            {
                VertexOutput o; 
                o.vertex = TransformObjectToHClip(v.vertex.xyz); // MVP 한번에 처리 
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            // 픽셀 셰이더
            half4 frag(VertexOutput i) : SV_Target
            {
                float4 color = tex2D(_MainTex,i.uv) * _MainColor;
                // float2 uv = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw // xy는 scale zw 는 offset
                // clip(color.a - 0.5); 그레이 스케일로 변겹후 플롯 값 하나 추가해서 알파채널 날려주기  
                return color;
            }
            ENDHLSL // HLSL 끝
        }
    }
}