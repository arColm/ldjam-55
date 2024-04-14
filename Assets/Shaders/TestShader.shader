Shader "TestShader"
{
	Properties
	{
		_MaskTexture("MaskTexture", 2D) = "white" {}
		_MaskTextureColor("MaskTextureColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord("", 2D) = "white" {}

	}

		SubShader
	{


		Tags { "RenderType" = "Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0


		GrabPass{ }

		Pass
		{
			Name "Unlit"
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM

			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
		//only defining to not throw compilation error over Unity 5.5
		#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
		#endif
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_instancing
		#include "UnityCG.cginc"


		struct appdata
		{
			float4 vertex : POSITION;
			float4 color : COLOR;
			float4 ase_texcoord : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
			float3 worldPos : TEXCOORD0;
			#endif
			float4 ase_texcoord1 : TEXCOORD1;
			float4 ase_texcoord2 : TEXCOORD2;
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
		};

		uniform float4 _MaskTextureColor;
		uniform sampler2D _MaskTexture;
		uniform float4 _MaskTexture_ST;
		ASE_DECLARE_SCREENSPACE_TEXTURE(_GrabTexture)
		inline float4 ASE_ComputeGrabScreenPos(float4 pos)
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = (pos.y - o.y) * _ProjectionParams.x * scale + o.y;
			return o;
		}



		v2f vert(appdata v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			UNITY_TRANSFER_INSTANCE_ID(v, o);

			float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
			float4 screenPos = ComputeScreenPos(ase_clipPos);
			o.ase_texcoord2 = screenPos;

			o.ase_texcoord1.xy = v.ase_texcoord.xy;

			//setting value to unused interpolator channels and avoid initialization warnings
			o.ase_texcoord1.zw = 0;
			float3 vertexValue = float3(0, 0, 0);
			#if ASE_ABSOLUTE_VERTEX_POS
			vertexValue = v.vertex.xyz;
			#endif
			vertexValue = vertexValue;
			#if ASE_ABSOLUTE_VERTEX_POS
			v.vertex.xyz = vertexValue;
			#else
			v.vertex.xyz += vertexValue;
			#endif
			o.vertex = UnityObjectToClipPos(v.vertex);

			#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			#endif
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			UNITY_SETUP_INSTANCE_ID(i);
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
			fixed4 finalColor;
			#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
			float3 WorldPosition = i.worldPos;
			#endif
			float2 uv_MaskTexture = i.ase_texcoord1.xy * _MaskTexture_ST.xy + _MaskTexture_ST.zw;
			float4 tex2DNode41 = tex2D(_MaskTexture, uv_MaskTexture);
			float4 screenPos = i.ase_texcoord2;
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos(screenPos);
			float4 screenColor44 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_grabScreenPos.xy / ase_grabScreenPos.w);
			float4 temp_cast_0 = (tex2DNode41.a).xxxx;


			finalColor = ((_MaskTextureColor * ((tex2DNode41.a * (1.0 - screenColor44)) * 2.0)) + (screenColor44 - temp_cast_0));
			return finalColor;
		}
		ENDCG
	}
	}
		CustomEditor "ASEMaterialInspector"


}