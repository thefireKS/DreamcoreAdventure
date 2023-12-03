Shader "Unlit/SH_SS_VHS_optim"
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
            #define VHSRES fixed2(120.0,120.0)
            #define V fixed2(0.,1.)
            
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                fixed4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
            };

            struct v2f
            {
                fixed2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                fixed4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Noise;
            fixed4 _MainTex_ST;
            fixed _CA;
            fixed _Sharpen;
            fixed _Blur;
            fixed _Discoloration;
            fixed _Distortion;

            fixed3 CA(sampler2D tex, fixed2 uv)
            {
                fixed strength = _CA;
                fixed r = tex2D(tex, uv + fixed2(0.0, strength)).r;
                fixed g = tex2D(tex, uv).g;
                fixed b = tex2D(tex, uv + fixed2(0.0, -strength)).b;
                fixed3 final = fixed3(r,g,b);
                return final;
            }

fixed3 Sharpen(sampler2D tex, fixed2 uv)
{
    fixed strength = _Sharpen * 0.5;  // Reduce strength by half
    fixed3 tl = CA(tex, uv + fixed2(-strength, strength)).rgb;
    fixed3 tr = CA(tex, uv + fixed2(strength, strength)).rgb;
    fixed3 ml = CA(tex, uv + fixed2(-strength, 0.0)).rgb;
    fixed3 mr = CA(tex, uv + fixed2(strength, 0.0)).rgb;
    fixed3 bl = CA(tex, uv + fixed2(-strength, -strength)).rgb;
    fixed3 br = CA(tex, uv + fixed2(strength, -strength)).rgb;
    fixed3 final = (tl + tr + ml + mr + bl + br) / 6.0;  // Reduce divisor to 6
    final = lerp(tex2D(tex, uv).rgb, final, -1.0);  // Adjust lerping
    return final;
}

fixed3 Blur(sampler2D tex, fixed2 uv)
{
    fixed strength = _Blur * 0.5;  // Reduce strength by half
    fixed3 tl = Sharpen(tex, uv + fixed2(-strength, strength)).rgb;
    fixed3 tr = Sharpen(tex, uv + fixed2(strength, strength)).rgb;
    fixed3 ml = Sharpen(tex, uv + fixed2(-strength, 0.0)).rgb;
    fixed3 mr = Sharpen(tex, uv + fixed2(strength, 0.0)).rgb;
    fixed3 bl = Sharpen(tex, uv + fixed2(-strength, -strength)).rgb;
    fixed3 br = Sharpen(tex, uv + fixed2(strength, -strength)).rgb;
    fixed3 final = (tl + tr + ml + mr + bl + br) / 6.0;  // Reduce divisor to 6
    return final;
}

            fixed2 DistortUV(fixed2 uv)
            {
                fixed offs = tex2D(_Noise, fixed2(_Time.x, uv.y * 0.5)).r;
                offs -= 0.5;
                offs *= 2.0;
                offs *= 0.0025;
                uv.x += offs;
                return uv;
            }

            fixed2 crt(fixed2 coord, fixed bend)
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

            fixed3 ColourCorrect(fixed3 col)
            {
                col += _Discoloration;
                col = clamp(col, fixed3(0.0,0.0,0.0), fixed3(1.0,1.0,1.0));
                col = pow(col, fixed3(0.8,0.8,0.8));
                col /= 1.5;
                col = clamp(col, fixed3(0.0,0.0,0.0), fixed3(1.0,1.0,1.0));
                return col;
            }

            fixed3 Finalize(fixed3 col, fixed2 uv)
            {
                fixed bars = tex2D(_Noise, fixed2(_Time.x, uv.y * 0.1)).g;
                col += fixed3(bars,bars,bars) * fixed3(0.05, 0.01, 0.005);
                fixed n = tex2D(_Noise, uv * 0.45 + (_Time.x * tex2D(_Noise, fixed2(_Time.x,_Time.x)).r)).b;
                n *= 0.05;
                return (col * n) + col / 1.1;
            }

            fixed4 mainImage( fixed2 uv )
            {
                uv = DistortUV(uv);

                uv = crt(uv, _Distortion);
                fixed3 colour;
                colour = Blur(_MainTex, uv);
                colour = ColourCorrect(colour);
                colour = Finalize(colour, uv);
                
                return fixed4(colour, 1.0);
            }

            fixed vignette(fixed2 uv) {
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
                fixed4 col = mainImage(i.uv);
                // float4 col = tex2D(_MainTex, i.uv);
                return col * vignette(i.uv);
            }
            ENDCG
        }
    }
}
