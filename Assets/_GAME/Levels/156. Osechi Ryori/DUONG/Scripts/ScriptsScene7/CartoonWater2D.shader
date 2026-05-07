Shader "Custom/2DWaterDuong_TimeBased"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)

        // Các biến điều khiển chuyển động (để Public để C# set)
        _StartVal ("Start Value", Float) = 0
        _EndVal ("End Value", Float) = 1
        _StartTime ("Start Time", Float) = 0
        _Duration ("Duration", Float) = 1

        _WaveStrength ("Wave Strength", Range(0, 0.05)) = 0.01
        _WaveSpeed ("Wave Speed", Range(0, 5)) = 1
        _WaveFrequency ("Wave Frequency", Range(0.1, 50)) = 10 
        _LightStrength ("Light Strength", Range(0, 1)) = 0.3
        _EdgeFade ("Edge Fade", Range(0, 0.5)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            // Biến logic chuyển đổi
            float _StartVal;
            float _EndVal;
            float _StartTime;
            float _Duration;

            float _WaveStrength;
            float _WaveSpeed;
            float _WaveFrequency;
            float _LightStrength;
            float _EdgeFade;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 originalUV : TEXCOORD1; 
                float2 localPos : TEXCOORD3;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.originalUV = o.uv; 
                o.color = v.color * _Color;
                o.localPos = v.vertex.xy; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ---- LOGIC TÍNH TOÁN SMOOTH TRONG SHADER ----
                
                // 1. Tính xem đã trôi qua bao lâu từ lúc bắt đầu lệnh
                float timeElapsed = _Time.y - _StartTime;
                
                // 2. Tính tiến độ (0 đến 1). saturate giúp kẹp giá trị không vượt quá 0-1
                float progress = saturate(timeElapsed / _Duration);

                // 3. Làm mượt tiến độ (Optional: Smoothstep giúp chạy nhanh ở giữa, chậm ở 2 đầu)
                progress = progress * progress * (3.0 - 2.0 * progress);

                // 4. Nội suy từ giá trị cũ sang giá trị mới
                float activeMul = lerp(_StartVal, _EndVal, progress);

                // ---------------------------------------------

                float time = _Time.y * _WaveSpeed;

                // Áp dụng activeMul vào cường độ sóng
                float waveX = sin(i.localPos.y * _WaveFrequency + time) * _WaveStrength * activeMul;
                float waveY = cos(i.localPos.x * _WaveFrequency + time) * _WaveStrength * activeMul;

                float2 distortedUV = i.uv + float2(waveX, waveY);

                fixed4 col = tex2D(_MainTex, distortedUV);
                col *= i.color;

                // Áp dụng activeMul vào ánh sáng
                float lightParams = sin(i.localPos.y * _WaveFrequency + time);
                col.rgb += lightParams * _LightStrength * col.a * activeMul;

                // Edge fade giữ nguyên
                float edge = 
                    smoothstep(0.0, _EdgeFade, i.originalUV.x) *
                    smoothstep(0.0, _EdgeFade, i.originalUV.y) *
                    smoothstep(0.0, _EdgeFade, 1.0 - i.originalUV.x) *
                    smoothstep(0.0, _EdgeFade, 1.0 - i.originalUV.y);

                col.a *= edge;

                return col;
            }
            ENDCG
        }
    }
}