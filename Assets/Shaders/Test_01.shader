Shader "Unlit/Test_01"
{   
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Period ("Period", float) = 0.5
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
            float _Period;
            float4 _MainTex_ST;
            
            fixed Blend(float normalizedY, float offset, float period)
            {
            // Calculate blend by height
                fixed blendNegToPosOne = cos((2 * M_PI / period) * (normalizedY + offset));
                return blendNegToPosOne + 1;
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
                 
                
                //o.vertex += fixed4(blend * 1, 0, 0, 0);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed blend = Blend(i.uv.y, _Time[3], _Period);
                fixed wetDelta = stepNegPos(0.5, i.uv.x) * sinTime01() * 0.1;
                
                i.uv.x += blend * wetDelta; // Wet blend
            
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                col.rgb *= col.a;
               
                return col;
            }
            ENDCG
        }
    }
}
