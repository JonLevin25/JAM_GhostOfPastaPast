Shader "Unlit/Test_01"
{   
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RippleScale ("Ripple Scale", float) = 0.1
        _RipplePeriod ("Ripple Period", float) = 0.5
        _RippleSpeed ("Ripple Speed", float) = 0.5
        _HorCenter ("Horizontal Center", Float) = 0.5
    }
    SubShader
    {
        Tags
        {
             "IgnoreProjector" = "True"
             "RenderType" = "Transparent"
             "PreviewType" = "Plane"
             "CanUseSpriteAtlas" = "True" 
        }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #define M_PI 3.1415926535897932384626433832795
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
            
            float _RippleScale;
            float _RipplePeriod;
            float _RippleSpeed;
            float4 _MainTex_ST;
            
            fixed SinBlendGoingUp(float value, float offset, float period)
            {
                // Calculate blend by height
                fixed blendNegToPosOne = cos((2 * M_PI / period) * -value + offset);
                return (blendNegToPosOne + 1) / 2;
            }
            
            fixed stepNegPos(float reference, float variable)
            {
                return step(reference, variable) * 2 -1;
            }
            
            fixed sinTime01()
            {
                return (_SinTime[3] + 1) * 0.5;
            }

            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed blend = SinBlendGoingUp(i.uv.y, _RippleSpeed * _Time[3], _RipplePeriod);
                
                float scaleCenter = 0.5f;
                float wet = (i.uv.x - scaleCenter) * _RippleScale + scaleCenter;

                //float wetDelta = stepNegPos(0.5, i.uv.x) * _RippleScale;
                
                i.uv.x = i.uv.x * (1-blend) + wet * blend;
                
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
               // test wetDelta
               col.gb = 0;
               col.r = wet;
               return col   ;
               // test blend
               col.gb = 0;
               col.r = blend;
               
               
                return col;
            }
            ENDCG
        }
    }
}
