Shader "Unlit/WaterShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DistortTex ("Distort", 2D) = "white" {}
        _Colour ("Colour", Color) = (1, 1, 1, 0.2)
        _Intensity ("Intensity", Float) = 1
        _Ramp ("Ramp", Float) = 1
        
        _GlowThickness ("Glow Thickness", Float) = 1
        _GlowIntensity ("Glow Intensity", Float) = 1
        
        _DistortThickness ("Thickness", Range(0.01,0.99)) = 0.5
    }
    SubShader
    {
        Tags { 
            "PreviewType" = "Plane"
            "RenderType"="Transparent"
            "Queue"="Overlay"
        }
        LOD 100
        //Blend SrcAlpha OneMinusSrcAlpha
        Blend SrcAlpha One
        Zwrite On
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _DistortTex;
            float4 _MainTex_ST;

            float4 _Colour;
            float _Intensity, _Ramp, _DistortThickness, _GlowIntensity, _GlowThickness;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 mainTex = tex2D(_MainTex, i.uv);
                float luminance = Luminance(mainTex);
                float3 mainColour = luminance * _Colour;
                mainColour = pow(mainColour, _Ramp) * _Intensity;

                float3 distortTex = tex2D(_DistortTex, i.uv);
                float distortMask = abs(sin(distortTex.r * 30 + _Time.y));
                float distortStep = step(distortMask, _DistortThickness);

                float glow =  smoothstep(distortMask - _GlowThickness, distortMask + _GlowThickness, _DistortThickness);
                glow *= _GlowIntensity;
                return fixed4(mainColour + glow, distortStep);
                return fixed4(mainColour, distortStep);
            }
                
            ENDCG
        }
    }
}
