Shader "UI/Fluid"
{
    Properties
    {
        _MainTex("Background Texture", 2D) = "white" {}
        _Speed("Speed", Float) = 1
        _Amplitude("Amplitude", Range(0, 1)) = 0.1
        _Distortion("Distortion", Range(0, 1)) = 0.1
        _Color("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            float _Speed;
            float _Amplitude;
            float _Distortion;
            float4 _Color;

            v2f vert (appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;
                return OUT;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                float2 uv = IN.uv;
                uv.x += sin(uv.y * _Speed + _Time.y) * _Amplitude;
                uv.y += sin(uv.x * _Speed + _Time.y) * _Amplitude;
                uv += _Time.y * _Distortion;
                fixed4 col = tex2D(_MainTex, uv);
                col.rgb *= _Color.rgb;
                return col;
            }
            ENDCG
        }
    }
}