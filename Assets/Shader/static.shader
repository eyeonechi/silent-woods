Shader "Unlit/static"
{
	Properties
	{
		_Color ("color", Color) = (1,1,1,1)
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
