Shader "Custom/AlphaBlendingEffect"
{
    Properties
    {
        _Speed("Speed", float) = 1

        _MainTex("Albedo (RGB)", 2D) = "white" {}
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" }

            CGPROGRAM
            #pragma surface surf LambertBlinnphong

            sampler2D _MainTex;

            float _Speed;

            struct Input
            {
                float2 uv_MainTex;
            };


            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y + _Time.y * _Speed));//UV y축으로 기본속도(_Time.y) 만큼 이동
                o.Albedo = lerp(c.rgb, float4(0,0,0,0), c.a);

                o.Alpha = c.a;
            }

            float4 LightingLambertBlinnphong(SurfaceOutput s, float3 lightDir, float viewDir, float atten)
            {
                //Diffuse구현, Lambert공식사용
                float3 Diffuse;
                float ndotl = saturate(dot(s.Normal, lightDir));//0~1범위에서 벗어나는 결과값 조정
                Diffuse = ndotl * s.Albedo * _LightColor0.rgb * atten;

                float4 final;//최종적으로 return할 값
                final.rgb = Diffuse.rgb;
                final.a = s.Alpha;
                return final;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
