Shader "usim/BlitCopyDepth"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM

            #pragma multi_compile CHANNELS1 CHANNELS2 CHANNELS3 CHANNELS4
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            uniform sampler2D _CameraDepthTexture;
            float4 _CameraDepthTexture_ST;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord.xy, _CameraDepthTexture);
                return o;
            }

#if CHANNELS1
            float frag(v2f i) : COLOR
            {
                float d = tex2D(_CameraDepthTexture, i.uv);
                d = Linear01Depth(d);
                return d;
            }
#endif
#if CHANNELS2
            float2 frag(v2f i) : COLOR
            {
                float d = tex2D(_CameraDepthTexture, i.uv);
                d = Linear01Depth(d);
                return float2(d, d);
            }
#endif
#if CHANNELS3
            float3 frag(v2f i) : COLOR
            {
                float d = tex2D(_CameraDepthTexture, i.uv);
                d = Linear01Depth(d);
                return float3(d, d, d);
            }
#endif
#if CHANNELS4
            float4 frag(v2f i) : COLOR
            {
                float d = tex2D(_CameraDepthTexture, i.uv);
                d = Linear01Depth(d);
                return float4(d, d, d, 1);
            }
#endif
            ENDCG
        }
    }
}