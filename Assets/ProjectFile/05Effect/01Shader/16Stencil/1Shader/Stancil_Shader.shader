Shader "Custom/Stancil"
{
    Properties
    {
        [IntRange] _StencilID("Stencil ID", Range(0,255)) = 0
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry-1" "RenderType"="Opaque" }
        
        Pass
        {
            Blend Zero One
            Zwrite off
            ColorMask 0
            Cull front
            
            Stencil
            {
                Ref[_StencilID]
                Comp always
                Pass replace   
            }
        }
    }
}
