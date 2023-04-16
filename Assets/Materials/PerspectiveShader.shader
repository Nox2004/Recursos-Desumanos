// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MyShaders/SpritePerspective"
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

            //Edges offset
            float upright_offset = 0;
            float upleft_offset = 0;
            
            float right_offset = 0;
            float left_offset = 0;

            //Color overlay settings
            bool color_overlay_on;
            float4 color_overlay;

            //Outline settings
            int outline_on = 0;
            float outline_size;
            
            //Color blend settings
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

                //Lower edges
                if (_vertex.y < 0)
                {
                    if (_vertex.x < 0) //Left
                    {
                        _vertex.x+=left_offset;
                    }
                    else if (_vertex.x > 0) //Right
                    {
                        _vertex.x+=right_offset;
                    }
                }
                else //Upper edges
                {
                    if (_vertex.x < 0) //Left
                    {
                        _vertex.x+=upleft_offset;
                    }
                    else if (_vertex.x > 0) //Right
                    {
                        _vertex.x+=upright_offset;
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

                //Applies a overlay to the pixel color
                if (color_overlay_on == 1)
                {
                    c = color_overlay;
                }

                //Blends the pixel color with another
                if (blend_color_on == 1) 
                {
                    c*=blend_color;
                }

                //Multiplies by alpha
		        c.rgb *= c.a;
		        return c;
            }
        ENDCG
        }
    }
}