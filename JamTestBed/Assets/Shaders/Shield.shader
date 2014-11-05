Shader "Custom/Shield" {
	Properties
	 {
		_Color ("Main Color", Color) = (1,1,1,0)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}		
		_effectTex ("Base (RGB) Alpha (A)", 2D) = "white" {}		
		_shieldAplha  ("Shield Aplha", float) = 0
		_hitPoint ("Hit Point", Vector) = (0,0,0,0) 
		_diameter  ("Shield Diameter", float) = 0

	}


    SubShader
 {
    Tags {Queue=Transparent}
    Blend SrcAlpha OneMinusSrcAlpha
    ColorMask RGB
    Cull Off

        Pass 
{
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag 
			//#pragma exclude_renderers d3d11 xbox360
			#include "UnityCG.cginc"
        
            uniform sampler2D _MainTex;
            uniform sampler2D _effectTex;
            
			float _shieldAplha;
			Vector _hitPoint;
 			float _diameter;
	      
	        
  			struct vertexInput 
  			{
                float4 vert : POSITION;
                
                float2 texcoord0 : TEXCOORD0; 
            };
	        
	         struct fragmentInput 
	         {
                float4 pos : SV_POSITION;
                float alpha : alpha;
                float2 texcoord0 : TEXCOORD0; 
            };
            
            
			
            fragmentInput  vert(vertexInput v)  
            {
                float dist = (v.vert.r -_hitPoint.r)*(v.vert.r -_hitPoint.r);
                dist += (v.vert.y - _hitPoint.y )*( v.vert.y - _hitPoint.y);
                dist += (v.vert.z -_hitPoint.z)*(v.vert.z -_hitPoint.z);
                
              	fragmentInput o;  
                o.alpha = ( 1-(dist  / _diameter))*_shieldAplha;
                o.alpha = clamp(o.alpha,0,1);
                o.pos = mul (UNITY_MATRIX_MVP, v.vert); 
                o.texcoord0 = v.texcoord0;

                return o;
            }

            float4 frag(fragmentInput  i) : COLOR 
            {
				float4 colour;                 
				float4 c = tex2D(_MainTex, i.texcoord0); 
				colour.rgb =  c.rgb;
				colour.a = i.alpha;
				return colour;
            }

            ENDCG
        }
    }
}