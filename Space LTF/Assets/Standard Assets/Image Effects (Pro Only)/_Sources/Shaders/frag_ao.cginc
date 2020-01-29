
//_Params - x=radius, y=minz, z=attenuation power, w=SSAO power

half frag_ao (v2f_ao i, int sampleCount, float3 samples[INPUT_SAMPLE_COUNT])
{
	// read random normal from noise texture
    half3 randN = tex2D (_RandomTexture, i.uvr).xyz * 2.0 - 1.0;    
    
    // read scene depth/normal
    float4 depthnormal = tex2D (_CameraDepthNormalsTexture, i.uv);
    float3 viewNorm;
    float depth;
    DecodeDepthNormal (depthnormal, depth, viewNorm);
    depth *= _ProjectionParams.z;
    float scale = _Params.x / depth;

    half3 normOffset = viewNorm * _Bias;
    
    // accumulated occlusion factor
    float occ = 0.0;
    for (int s = 0; s < sampleCount; ++s)
    {


        half3 randomDir = samples[s];        
        // Make it point to the upper hemisphere
        randomDir *=  (dot(viewNorm, randomDir) < 0) ? -1.0 : 1.0;

        // Add a bit of normal to reduce self shadowing
        randomDir += normOffset;
        
        float2 offset = randomDir.xy * scale;
        float sD = depth - (randomDir.z * _Params.x);

		// Sample depth at offset location
        float4 sampleND = tex2D (_CameraDepthNormalsTexture, i.uv + offset);
        float sampleD = DecodeFloatRG (sampleND.zw);

        sampleD *= _ProjectionParams.z;
        float zd = saturate(sD - sampleD);

        occ += (zd > _Params.y) * pow(1 - zd, _Params.z);
      
    }
    occ /= sampleCount;
    occ *= _Params.w;
    return 1 - occ;
}

