// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sector12/SpriteSkewShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        
        left_offset ("Left Offset", Float) = 0
        right_offset ("Right Offset", Float) = 0
    }
    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend One OneMinusSrcAlpha
        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
		        uint vertexId : SV_VertexID;
            };
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            fixed4 _Color;
            float left_offset;
            float right_offset;

            bool color_overlay_on;
            float4 color_overlay;

            int outline_on = 0;
            float outline_size;

            int blend_color_on = 1;
            float4 blend_color = float4(1,1,1,1);

	        
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                
                // Create a skew transformation matrix
                float4x4 transformMatrix = float4x4(
                    1,0,0,0,
                    0,1,0,0,
                    0,0,1,0,
                    0,0,0,1);
                
                //float4 skewedVertex = mul(transformMatrix, IN.vertex);
                float4 _vertex = IN.vertex;
                if (_vertex.y < 0)
                {
                    if (_vertex.x < 0)
                    {
                        _vertex.x+=left_offset;
                    }
                    else if (_vertex.x > 0)
                    {
                        _vertex.x+=right_offset;
                    }
                }
                OUT.vertex = UnityObjectToClipPos(_vertex);
                

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
                return OUT;
            }
		
            fixed4 frag(v2f IN) : SV_Target
            {	
		        fixed4 c = tex2D(_MainTex,IN.texcoord) * IN.color;
                if (color_overlay_on == 1)
                {
                    c = color_overlay;
                }
                if (blend_color_on == 1) 
                {
                    c*=blend_color;
                }
		        c.rgb *= c.a;
		        return c;
            }
        ENDCG
        }
    }
}