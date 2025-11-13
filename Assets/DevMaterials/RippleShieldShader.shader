Shader "Custom/RippleShield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Float) = 5.0
        _WaveIntensity ("Wave Intensity", Float) = 0.02
        _Color ("Color Tint", Color) = (0.1,0.6,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _WaveSpeed;
            float _WaveIntensity;
            fixed4 _Color;

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

            float _TimeValue;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                _TimeValue = _Time.y; // Unity shader time.y is time in seconds
                return o;
            }

           fixed4 frag(v2f i) : SV_Target
            {
                float wave = sin(i.uv.x * 10 + _Time.y * _WaveSpeed) * _WaveIntensity;
                float2 rippleUV = float2(i.uv.x, i.uv.y + wave);

                fixed4 col = tex2D(_MainTex, rippleUV) * _Color;
                col.a = col.a * _Color.a;

                return col;
            }

            ENDCG
        }
    }
}
