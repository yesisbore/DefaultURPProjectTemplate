Shader "Custum/DefaultShader"
{
    Properties
    {
        _MainTex("MainTexture",2D) = "white"{}
        _PainterColor("Painter Color", Color) = (0, 0, 0, 0)
    } 

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        } 
        
        Pass
        {
            Cull Off ZWrite Off ZTest Off

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
                float2 uv     : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            // 변수 사용 
            
            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 _PainterPosition;
            float _Radius;
            float _Hardness;
            float _Strength;
            float4 _PainterColor;
            float _PrepareUV;
            
            // 버텍스 셰이더
            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                // o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                // o.uv = v.uv;
                // float4 uv = float4(0, 0, 0, 1);
                // uv.xy = float2(1, _ProjectionParams.x) * (v.uv.xy * float2(2, 2) - float2(1, 1));
                // o.vertex = uv;

                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                
                return o;
            }

            // 픽셀 셰이더
            half4 frag(VertexOutput i) : SV_Target
            {
                //float4 flow = _FlowMap.Sample(sampler_MainTex,i.uv);
                //i.uv += frac(_Time.x * _FlowTime) + flow.rg * _FlowIntensity;
                // float3 light = _MainLightPosition.xyz;
                // float3 lightColor = _MainLightColor.rgb;
                // float4 color = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);//_MainTex.Sample(sampler_MainTex,i.uv);
                //
                // float3 NdotL = saturate(dot(i.normal,light));
                // //float3 toonLight = NdotL > 0 ? lightColor : 0;
                // float3 toonLight = ceil(NdotL* _LightWidth) / _LightStep ;
                // color.rgb *= toonLight;
                // float4 color = tex2D(_MainTex,i.uv) * _MainColor;
                // float2 uv = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw // xy는 scale zw 는 offset
                // clip(color.a - 0.5); 그레이 스케일로 변겹후 플롯 값 하나 추가해서 알파채널 날려주기
                float4 red = float4(1.0,0.0,0.0,1.0);
                return _PainterColor;
            }
            ENDHLSL // HLSL 끝
        }
    }
}