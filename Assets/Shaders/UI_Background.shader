Shader "Unlit/UIBackground"
{
    Properties // properties can be adjusted in Unity inspector
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SpriteOpacity ("Sprite Opacity", Range(0, 1)) = 1
        _Background ("Background", Color) = (1, 1, 1, 1)
        _AnimateXY("Animate X", Float) = 0
        _AnimateYY("Animate Y", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            float _SpriteOpacity;
            float4 _MainTex_ST;
            float4 _Background;
            float _AnimateXY;
            float _AnimateYY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv += frac(float2(_AnimateXY, _AnimateYY) * _MainTex_ST * _Time.y);
                // frac clamps betwen 0 and 1, unity time runs from progam start so this is useful for stopping it from going nuts

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvs = i.uv;
                fixed4 textureColour = tex2D(_MainTex, uvs);
                // apply user specified opacity
                textureColour *= _SpriteOpacity;

                // Blend the texture color with the background color based on the alpha channel
                // (put the background colour wherever the main texture isn't)
                fixed4 finalColour = lerp(_Background, textureColour, textureColour.a);

                return finalColour;
            }
            ENDCG
        }
    }
}
