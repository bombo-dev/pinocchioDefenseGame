Shader "Custom/WaterFall"
{
    Properties
    {
        _Speed("Speed", float) = 1

        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("NormalMap", 2D) = "bump"{}
        _NormalPower("Normal Power", float) = 1.0

        _SpecCol("Specular Color", Color) = (1,1,1,1)
        _SpecPower("Specular Power", Range(10,200)) = 100
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            CGPROGRAM
            #pragma surface surf LambertBlinnphong

            sampler2D _MainTex;
            sampler2D _BumpMap;

            float _Speed;
            float4 _DiffuseCol;
            float4 _SpecCol;
            float _NormalPower;
            float _SpecPower;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_BumpMap;
            };


            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y + _Time.y * _Speed));//UV y축으로 기본속도(_Time.y) 만큼 이동
                o.Albedo = c.rgb;

                //노말 적용
                float3 n = UnpackNormal(tex2D(_BumpMap, float2(IN.uv_BumpMap.x, IN.uv_BumpMap.y + _Time.y * _Speed)));//UV y축으로 기본속도(_Time.y) 만큼 이동
                o.Normal = float3(n.x * _NormalPower, n.y * _NormalPower, n.z);

                o.Alpha = c.a;
            }

            float4 LightingLambertBlinnphong(SurfaceOutput s, float3 lightDir, float viewDir, float atten)
            {
                //Diffuse구현, Lambert공식사용
                float3 Diffuse;
                float ndotl = saturate(dot(s.Normal, lightDir));//0~1범위에서 벗어나는 결과값 조정
                Diffuse = ndotl * s.Albedo * _LightColor0.rgb * atten;

                //Specular구현, Blinnphong공식사용
                float3 Specular;
                float3 H = normalize(lightDir + viewDir);
                float spec = saturate(dot(H, s.Normal));
                spec = pow(spec, _SpecPower);//Specular 크기조절
                Specular = spec * _SpecCol.rgb;//Specular 색

                float4 final;//최종적으로 return할 값
                final.rgb = Diffuse.rgb + Specular.rgb;
                final.a = s.Alpha;
                return final;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
