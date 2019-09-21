﻿Shader "Custom/TextShader"
{
	Properties
	{
		_MainTex("Texture", any) = "" {}
	}

	CGINCLUDE
	#pragma vertex vert
	#pragma fragment frag

	#include "UnityCG.cginc"

	struct appdata_t
	{
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D _MainTex;
	uniform float4 _MainTex_ST;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.color = v.color;
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		return tex2D(_MainTex, i.texcoord).a * i.color;
	}
	ENDCG

	SubShader
	{

		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		//Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha
		//Cull Off
		ZWrite Off
		//ZTest Always

		Pass
		{
			CGPROGRAM
			ENDCG
		}
	}
	
	Fallback off
}