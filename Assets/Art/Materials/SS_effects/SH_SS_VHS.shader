Shader "Unlit/SH_SS_VHS"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _CA ("CA", Range(0.01, 0.0001)) = 0.001
        _Sharpen ("Sharpen", Range(0.01, 0.0001)) = 0.001
        _Blur ("Blur", Range(0.01, 0.0001)) = 0.001  
        _Discoloration ("Discoloration", Range(0.1, 0.001)) = 0.03  
        _Distortion ("Distortion", Range(-1, 2)) = 2
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
            #define VHSRES float2(120.0,120.0)
            #define V float2(0.,1.)
            
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Noise;
            float4 _MainTex_ST;
            float _CA;
            float _Sharpen;
            float _Blur;
            float _Discoloration;
            float _Distortion;

            float v2random( float2 uv ) {
            return tex2D( _Noise, fmod( uv, 1 ) ).x;
            }



            float3 CA(sampler2D tex, float2 uv)
            {
                float strength = _CA;
                float r = tex2D(tex, uv + float2(0.0, strength)).r;
                float g = tex2D(tex, uv).g;
                float b = tex2D(tex, uv + float2(0.0, -strength)).b;
                float3 final = float3(r,g,b);
                return final;
            }

            float3 Sharpen(sampler2D tex, float2 uv)
            {
                float strength = _Sharpen;
                float3 tl = CA(tex, uv + float2(-strength, strength)).rgb;
                float3 tm = CA(tex, uv + float2(0.0, strength)).rgb;
                float3 tr = CA(tex, uv + float2(strength, strength)).rgb;
                float3 ml = CA(tex, uv + float2(-strength, 0.0)).rgb;
                float3 mr = CA(tex, uv + float2(strength, 0.0)).rgb;
                float3 bl = CA(tex, uv + float2(-strength, -strength)).rgb;
                float3 bm = CA(tex, uv + float2(0.0, -strength)).rgb;
                float3 br = CA(tex, uv + float2(strength, -strength)).rgb;
                float3 final = (tl+tm+tr+ml+mr+bl+bm+br)/8.0;
                final = lerp(tex2D(tex, uv).rgb, final, -2.0);
                return final;
            }

            float3 Blur(sampler2D tex, float2 uv)
            {
                float strength = _Blur;
                float3 tl = Sharpen(tex, uv + float2(-strength, strength)).rgb;
                float3 tm = Sharpen(tex, uv + float2(0.0, strength)).rgb;
                float3 tr = Sharpen(tex, uv + float2(strength, strength)).rgb;
                float3 ml = Sharpen(tex, uv + float2(-strength, 0.0)).rgb;
                float3 mr = Sharpen(tex, uv + float2(strength, 0.0)).rgb;
                float3 bl = Sharpen(tex, uv + float2(-strength, -strength)).rgb;
                float3 bm = Sharpen(tex, uv + float2(0.0, -strength)).rgb;
                float3 br = Sharpen(tex, uv + float2(strength, -strength)).rgb;
                float3 final = (tl+tm+tr+ml+mr+bl+bm+br)/8.0;
                return final;
            }

            float2 DistortUV(float2 uv)
            {
                float offs = tex2D(_Noise, float2(_Time.x, uv.y * 0.5)).r;
                offs -= 0.5;
                offs *= 2.0;
                offs *= 0.0025;
                uv.x += offs;
                return uv;
            }

            float2 crt(float2 coord, float bend)
            {
                // put in symmetrical coords
                coord = (coord - 0.5) * 2.0;

                coord *= 0.5;	
                
                // deform coords
                coord.x *= 1.0 + pow((abs(coord.y) / bend), 2.0);
                coord.y *= 1.0 + pow((abs(coord.x) / bend), 2.0);

                // transform back to 0.0 - 1.0 space
                coord  = (coord / 1.0) + 0.5;

                return coord;
            }

            float3 ColourCorrect(float3 col)
            {
                col += _Discoloration;
                col = clamp(col, float3(0.0,0.0,0.0), float3(1.0,1.0,1.0));
                col = pow(col, float3(0.8,0.8,0.8));
                col /= 1.5;
                col = clamp(col, float3(0.0,0.0,0.0), float3(1.0,1.0,1.0));
                return col;
            }

            float3 Finalize(float3 col, float2 uv)
            {
                float bars = tex2D(_Noise, float2(_Time.x, uv.y * 0.1)).g;
                col += float3(bars,bars,bars) * float3(0.05, 0.01, 0.005);
                float n = tex2D(_Noise, uv * 0.45 + (_Time.x * tex2D(_Noise, float2(_Time.x,_Time.x)).r)).b;
                n *= 0.05;
                return (col * n) + col / 1.1;
            }

            float4 mainImage( float2 uv )
            {
                // uv *= 1 - _Distortion*0.1;
                // uv += _Distortion/2*0.1; 
                uv = DistortUV(uv);

                uv = crt(uv, _Distortion);
                
                float3 colour = Blur(_MainTex, uv);
                colour = ColourCorrect(colour);
                colour = Finalize(colour, uv);
                
                return float4(colour, 1.0);
            }

            float vignette(float2 uv) {
                uv = (uv - 0.5) * 0.98;
                return clamp(pow(cos(uv.x * 3.1415), 2) * pow(cos(uv.y * 3.1415), 2) * 50.0, 0.0, .95);
            }
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the tex2D
                // fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = mainImage(i.uv);
                // apply fog

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col * vignette(i.uv);
            }
            ENDCG
        }
    }
}
