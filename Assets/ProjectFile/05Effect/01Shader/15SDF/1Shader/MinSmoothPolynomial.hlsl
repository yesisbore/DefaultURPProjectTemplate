#ifndef MYCUSTOM_HLSL
#define MYCUSTOM_HLSL

void MinSmoothPolynomial_float(float a, float b, float k,out float Out)
{
    float h = max(k - abs(a-b), 0.0) / k;
    Out = min(a,b) - h * h * k * (1.0/4.0);
}
#endif