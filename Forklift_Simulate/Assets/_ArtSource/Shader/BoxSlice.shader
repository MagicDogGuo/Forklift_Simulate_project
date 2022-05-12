Shader "Custom/BoxSlice" {

	Properties{

	_CubePos("Cube World Position", Vector) = (0, 0, 0)

	_CubeSize("Cube Size", Vector) = (1, 1, 1)

	_MainTex("Texture", 2D) = "white" {}

	_BumpMap("Bumpmap", 2D) = "bump" {}

	}

		SubShader{

		Tags { "RenderType" = "Opaque" }



		CGPROGRAM

		#pragma surface surf Lambert

		struct Input {

		float2 uv_MainTex;

		float2 uv_BumpMap;

		float3 worldPos;

		};



		sampler2D _MainTex;

		sampler2D _BumpMap;

		float3 _CubePos;

		float3 _CubeSize;



		void surf(Input IN, inout SurfaceOutput o) {

		float minX = _CubePos.x - (_CubeSize / 2);

		float maxX = _CubePos.x + (_CubeSize / 2);

		float minY = _CubePos.y - (_CubeSize / 2);

		float maxY = _CubePos.y + (_CubeSize / 2);

		float minZ = _CubePos.z - (_CubeSize / 2);

		float maxZ = _CubePos.z + (_CubeSize / 2);

		if (IN.worldPos.x >= minX && IN.worldPos.x <= maxX &&

		IN.worldPos.y >= minY && IN.worldPos.y <= maxY &&

		IN.worldPos.z >= minZ && IN.worldPos.z <= maxZ) {

		discard;

		}

		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

		}

		ENDCG

	}

		Fallback "Diffuse"

}