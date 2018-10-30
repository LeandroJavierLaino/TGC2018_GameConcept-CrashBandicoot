//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

//Textura para DiffuseMap
texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
	Texture = (texDiffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

float screen_dx;					// tamaño de la pantalla en pixels
float screen_dy;
float time;

//Input del Vertex Shader
struct VS_INPUT
{
	float4 Position : POSITION0;
	float3 Normal :   NORMAL0;
	float4 Color : COLOR;
	float2 Texcoord : TEXCOORD0;
};

//Input del Pixel Shader
struct PS_INPUT_DEFAULT
{
    float2 ScreenPos : TEXCOORD0;
};

//Offsets y weights de Gaussian Blur
static const int MAX_SAMPLES = 15;
float2 gauss_offsets[MAX_SAMPLES];
float gauss_weights[MAX_SAMPLES];

texture g_RenderTarget;
sampler RenderTarget =
sampler_state
{
	Texture = <g_RenderTarget>;
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

//Textura de Bloom
texture texBloomRT;
sampler BloomRT = sampler_state
{
    Texture = (texBloomRT);
    MipFilter = POINT;
    MinFilter = POINT;
    MagFilter = POINT;
};

//Output del Vertex Shader
struct VS_OUTPUT
{
	float4 Position :        POSITION0;
	float2 Texcoord :        TEXCOORD0;
	float3 Norm :          TEXCOORD1;			// Normales
	float3 Pos :   		TEXCOORD2;		// Posicion real 3d
};

//Vertex Shader
VS_OUTPUT vs_main(VS_INPUT Input)
{
	VS_OUTPUT Output;

	//Proyectar posicion
	Output.Position = mul(Input.Position, matWorldViewProj);

	//Las Texcoord quedan igual
	Output.Texcoord = Input.Texcoord;

	// Calculo la posicion real
	float4 pos_real = mul(Input.Position, matWorld);
	Output.Pos = float3(pos_real.x, pos_real.y, pos_real.z);

	// Transformo la normal y la normalizo
	//Output.Norm = normalize(mul(Input.Normal,matInverseTransposeWorld));
	Output.Norm = normalize(mul(Input.Normal, matWorld));
	return(Output);
}

//Numero random
float rand(float2 co){
      return 0.5+(frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453))*0.5;
}

//Pixel Shader
float4 ps_main(float3 Texcoord: TEXCOORD0, float3 N : TEXCOORD1,
	float3 Pos : TEXCOORD2) : COLOR0
{
	//Obtener el texel de textura
	float4 fvBaseColor = tex2D(diffuseMap, Texcoord);
	return fvBaseColor;
}

technique DefaultTechnique
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 vs_main();
		PixelShader = compile ps_3_0 ps_main();
	}
}

void VSCopy(float4 vPos : POSITION, float2 vTex : TEXCOORD0, out float4 oPos : POSITION, out float2 oScreenPos : TEXCOORD0)
{
	oPos = vPos;
	oScreenPos = vTex;
	oPos.w = 1;
}

float4 PSPostProcess(in float2 Tex : TEXCOORD0, in float2 vpos : VPOS) : COLOR0
{
	//PostProceso basico
	float4 ColorBase = tex2D(RenderTarget, Tex);
	
	
	//PostProceso de ondas
	//float4 ColorBase = tex2D(RenderTarget, Tex + float2(34*sin(time+Tex.x*15)/screen_dx,25*cos(time+Tex.y*15)/screen_dy));
	//return ColorBase;
	
	//PostProceso de desfase en el tiempo(embarrado)
	//float2 desf = float2(0, sin(time*50*(5/screen_dy)));
	//float4 ColorBase = tex2D(RenderTarget,Tex + desf);

	//Escala de grices
	// Y = 0.2126 R + 0.7152 G + 0.0722 B
	//float Y = 0.2126*ColorBase.r + 0.7152*ColorBase.g + 0.0722*ColorBase.b;
	//return float4(Y,Y,Y,1);
	
	//Efecto ojo de pez
	//float2 q = float2( Tex.x * 2 - 1 , 1 - Tex.y * 2 );
	//float d = length(q);
	//float4 color = 0;
	//if(d<=1)
	//{
	//	float k = d*d;
	//	float2 v = normalize(q) * k * 0.5;
	//	float2 uv = float2(0.5 + v.x, 0.5 - v.y);
	//	color = tex2D(RenderTarget,uv);
	//}
	//return color;
	
	//Barras interpoladas
	//float4 Color = tex2D(RenderTarget, Tex);
	//if ((fmod( (Tex.y)*(screen_dy),10)) < 4 ) return Color;
	//else return Color/3;

	//Vignetting
    	//float lensRadius = 0.55;
    	//uv /= lensRadius;
    	//float sin2 = uv.x*uv.x+uv.y*uv.y;
    	//float cos2 = 1.0-min(sin2*sin2,1.0);
    	//float cos4 = cos2*cos2;
    	//c *= cos4;
	float lensRadius = 800;
	float2 pos = vpos - 0.5*float2(screen_dx,screen_dy);
	pos /= lensRadius;

	float sin2 = pos.x*pos.x + pos.y*pos.y;
	float cos2 = 1 - min(sin2*sin2,1);
	float cos4 = cos2*cos2;
	//ColorBase *= cos4 * float4(0,0.9,0,1);
	ColorBase *= cos4 *rand(vpos *time);

	return ColorBase * 1.25;
	
}

float4 PSPostProcessRain(in float2 Tex : TEXCOORD0, in float2 vpos : VPOS) : COLOR0
{
	//PostProceso basico
	float4 ColorBase = tex2D(RenderTarget, Tex);

	ColorBase *= rand(vpos *time);

	return ColorBase;
}


float4 PSPostProcessWaves(in float2 Tex : TEXCOORD0, in float2 vpos : VPOS) : COLOR0
{
	//PostProceso de ondas
	float4 ColorBase = tex2D(RenderTarget, Tex + float2(34*sin(time+Tex.x*15)/screen_dx,25*cos(time+Tex.y*15)/screen_dy));
	return ColorBase * 1.5;
}

static const int kernel_r = 6;
static const int kernel_size = 13;
static const float Kernel[kernel_size] =
{
	0.002216,    0.008764,    0.026995,    0.064759,    0.120985,    0.176033,    0.199471,    0.176033,    0.120985,    0.064759,    0.026995,    0.008764,    0.002216,
};

void Blur(float2 screen_pos  : TEXCOORD0, out float4 Color : COLOR)
{
	Color = 0;
	for (int i = 0; i < kernel_size; ++i)
		for (int j = 0; j < kernel_size; ++j)
			Color += tex2D(RenderTarget, screen_pos + float2((float)(i - kernel_r) / screen_dx, (float)(j - kernel_r) / screen_dy)) * Kernel[i] * Kernel[j];
	Color.a = 1;
}

technique PostProcess
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 VSCopy();
		PixelShader = compile ps_3_0 PSPostProcess();
	}
}

technique Waves
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 VSCopy();
		PixelShader = compile ps_3_0 PSPostProcessWaves();
	}
}

/**************************************************************************************/
/* BloomPass */
/**************************************************************************************/

//Pasada de GaussianBlur horizontal o vertical para generar efecto de Bloom
float4 ps_bloom_pass(PS_INPUT_DEFAULT Input) : COLOR0
{
    float4 vSample = 0.0f;
    float4 vColor = 0.0f;

    float2 vSamplePosition;

	// Perform a one-directional gaussian blur
    for (int iSample = 0; iSample < MAX_SAMPLES; iSample++)
    {
        vSamplePosition = Input.ScreenPos + gauss_offsets[iSample];
        vColor = tex2D(RenderTarget, vSamplePosition);
        vSample += gauss_weights[iSample] * vColor;
    }

    return vSample;
}

technique BloomPass
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 vs_main();
        PixelShader = compile ps_3_0 ps_bloom_pass();
    }
}