﻿Shader "Custom/RenderOverTerrain" {
	Properties {
		_Color ("Main Color", Color )
		=(1,1,1,1)
		_MainTex ("Base (RGB)", 2D)="White" {}
	}
	
	Category { 
	Offset -2,-2
	    Zwrite Off
	    
	SubShader {
		Tags { "RenderType"="Opaque"
		"Queue" = "Geometry-1" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed4 Color ;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	}
	FallBack "Diffuse"
}
