﻿// Owlchemy Labs

Shader "Unlit/AlphaSelfIllumWithFade"
{
	Properties
	{
		_Color ("Color Tint", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white"
	}

	Category
	{
		Lighting Off
		ZWrite Off
		//ZWrite On
		Cull back
		Blend SrcAlpha OneMinusSrcAlpha
		//AlphaTest Greater 0.001
		Tags {Queue=Transparent}

		SubShader
		{
			Pass
			{
				SetTexture [_MainTex]
				{
					
					ConstantColor [_Color]
					Combine Texture * constant
				}
			}
		}
	}
}
