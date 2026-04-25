Shader "Unlit/DarknessCircle"
{
    Properties
    {
        _Color("Dark Color", Color) = (0,0,0,1)
        _Center("Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius("Radius", Float) = 0.25
        _Smooth("Smooth", Float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            float4 _Center;
            float _Radius;
            float _Smooth;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // プレイヤー中心との距離
                float dist = distance(uv, _Center.xy);

                // 0 = 透明, 1 = 黒
                float edge = smoothstep(_Radius, _Radius + _Smooth, dist);

                fixed4 col = _Color;
                col.a *= edge;

                return col;
            }
            ENDCG
        }
    }
}
