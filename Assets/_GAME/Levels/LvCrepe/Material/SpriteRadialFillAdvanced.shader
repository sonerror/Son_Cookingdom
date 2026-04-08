Shader "Sprites/AdvancedRadialFill"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)       // <- SpriteRenderer.color sẽ truyền giá trị vào đây
        _FillAmount ("Fill Amount", Range(0,1)) = 1
        _StartAngle ("Start Angle (°)", Range(0,360)) = 0
        _Clockwise ("Clockwise (1=true)", Range(0,1)) = 1
    }

    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"     // <- cần thiết để SpriteRenderer truyền color
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            float _FillAmount;
            float _StartAngle;
            float _Clockwise;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv - 0.5;
                float dist = length(uv);

                float angle = atan2(uv.y, uv.x);
                angle = degrees(angle);
                if (angle < 0) angle += 360;

                float startAngle = _StartAngle % 360;
                float relAngle = angle - startAngle;
                if (relAngle < 0) relAngle += 360;

                if (_Clockwise < 0.5) relAngle = 360 - relAngle;

                float normalizedAngle = relAngle / 360.0;
                float mask = step(normalizedAngle, _FillAmount);

                fixed4 texCol = tex2D(_MainTex, i.uv);
                texCol *= _Color;
                texCol.a *= mask;

                return texCol;
            }
            ENDCG
        }
    }
}
