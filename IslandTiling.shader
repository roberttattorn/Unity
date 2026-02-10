Shader "Custom/IslandTiling" {
    Properties {
        _MainTex ("Texture Atlas", 2D) = "white" {}
        _IslandRect ("Island Rect (X, Y, Width, Height)", Vector) = (0, 0, 0.5, 0.5)
        _TileScale ("Tile Scale", Float) = 4.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float4 _IslandRect; // (X-offset, Y-offset, Width, Height)
        float _TileScale;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            // 1. Convert standard 0-1 UVs into a repeating pattern
            float2 tiledUV = frac(IN.uv_MainTex * _TileScale);
            
            // 2. Remap that repeating pattern into the 'island' sub-region
            // New UV = IslandStart + (RepeatingUV * IslandSize)
            float2 finalUV = _IslandRect.xy + (tiledUV * _IslandRect.zw);
            
            fixed4 c = tex2D(_MainTex, finalUV);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
