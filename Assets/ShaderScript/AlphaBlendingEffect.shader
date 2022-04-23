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
                fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y + _Time.y * _Speed));//UV y������ �⺻�ӵ�(_Time.y) ��ŭ �̵�
                o.Albedo = lerp(c.rgb, float4(0,0,0,0), c.a);

                o.Alpha = c.a;
            }

            float4 LightingLambertBlinnphong(SurfaceOutput s, float3 lightDir, float viewDir, float atten)
            {
                //Diffuse����, Lambert���Ļ��
                float3 Diffuse;
                float ndotl = saturate(dot(s.Normal, lightDir));//0~1�������� ����� ����� ����
                Diffuse = ndotl * s.Albedo * _LightColor0.rgb * atten;

                float4 final;//���������� return�� ��
                final.rgb = Diffuse.rgb;
                final.a = s.Alpha;
                return final;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
