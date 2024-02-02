Shader "Unlit/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 0.7)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        float _WaterTime;
        float _RippleAmount;

        struct VertexInput
        {
            float4 position : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct VertexOutput
        {
            float4 position : POSITION;
            float2 uv : TEXCOORD0;
            float rippleOffset : TEXCOORD1; 
        };
        
        ENDHLSL
        
        Pass
        {
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            VertexOutput vert(VertexInput input)
            {
                VertexOutput output;

                // Add time-based ripple effect to the UV coordinates
                float rippleStrength = 12;
                float rippleSpeed = 0.5;
                float frequencyScaleX = 1.0;
                float frequencyScaleY = 1.0;

                float timeFactor = _WaterTime * rippleSpeed;
                
                // Add ripple effect to both x and y axes
                output.uv = 0;
                output.uv.x += cos(input.uv.x * frequencyScaleX + timeFactor) * rippleStrength;
                output.uv.y += cos(input.uv.y * frequencyScaleY + timeFactor) * rippleStrength;

                output.position = TransformObjectToHClip(input.position.xyz);
                output.rippleOffset = output.uv.x;
                
                return output;
            }

            half4 frag(VertexOutput output) : COLOR
            {
                float wave = sin(output.rippleOffset * 1.5) * _RippleAmount;
                float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, output.uv * 4 * wave);

                // Apply smoothstep to alpha channel
                baseTex.a = smoothstep(0.45, 0.55, baseTex.a);

                // Color variance based on wave value
                float colorVariance = 0.05; 
                float3 color = baseTex.r + wave * colorVariance;

                return float4(color, baseTex.b) * _BaseColor;
            }
                
            ENDHLSL
        }
    }
}
