Shader "Custom/SM64WarpPainting"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1,1,1,1)
        _Strength("Distortion Strength", Float) = 0.03
        _Frequency("Wave Frequency", Float) = 20.0
        _Speed("Wave Speed", Float) = 1.5
    }

    SubShader
    {
        Tags { 
            "RenderType"="Transparent"
            "Queue"="Transparent" 
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _TintColor;

            float _Strength;
            float _Frequency;
            float _Speed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float2 center = float2(0.5, 0.5);
                float2 delta = uv - center;
                float dist = length(delta);

                float wave = sin(dist * _Frequency - _Time.y * _Speed);

                uv += normalize(delta) * wave * _Strength;

                fixed4 col = tex2D(_MainTex, uv);
                col *= _TintColor;

                // Preserve alpha from tint * texture alpha
                return col;
            }
            ENDCG
        }
    }
}
