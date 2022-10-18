Shader "Custum/RippleShader"
{
    Properties
    {
        _RippleScale("Ripple Scale",Range(1,100)) = 1
        _RimPower("Rim Power",Range(0.01,1)) = 0.1
        _RimIntensity("Rim Intensity",Range(0.01,100)) = 1
        [HDR] _RimColor("Rim Color",color) = (1,1,1,1)
    } 

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparency"
            "Queue"="Transparent"
        } 
        
        Pass
        {
            Blend One One
            Cull back
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

            HLSLPROGRAM 
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
   
            struct VertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION; 
                float3 normal : NORMAL;
                float3 WorldSpaceViewDirection : TEXCOORD0;
                float3 Local : TEXCOORD1;
            };
            
            float _RimPower, _RimIntensity,_RippleScale;
            float4 _RimColor;
            
            // 버텍스 셰이더
            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz * _RippleScale);
                o.normal = TransformObjectToWorldNormal(v.normal);
                o.WorldSpaceViewDirection = normalize(_WorldSpaceCameraPos.xyz - TransformObjectToWorld(v.vertex.xyz));
                o.Local = v.vertex.xyz;
                return o;
            }

            // 픽셀 셰이더
            half4 frag(VertexOutput i) : SV_Target
            {
                float face = saturate(dot(i.WorldSpaceViewDirection,i.normal)); // 가운데 0 테두리 일수록 1 
                float rim = 1.0 - (pow(face,_RimPower));
                float4 color = rim * _RimColor * _RimIntensity * i.Local.y;

                //color.rgb += rim * _RimIntensity * _RimColor; 
                clip(color.a);
                return color;
            }
            ENDHLSL // HLSL 끝
        }
    }
}