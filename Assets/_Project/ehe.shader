Shader "Custom/AlwaysOnTop"
{
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }

        Pass
        {
            ZTest Always     // luôn pass depth test
            ZWrite Off       // không ghi depth để không phá scene

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return fixed4(1,1,1,1); // màu trắng (custom lại)
            }
            ENDCG
        }
    }
}