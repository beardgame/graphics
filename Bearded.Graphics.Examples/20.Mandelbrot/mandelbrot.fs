#version 400

uniform vec2 scale;
uniform vec2 offset;

in vec2 fragment_z;

out vec4 fragColor;

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main()
{
    const int maxIterations = 100;

    vec2 z = fragment_z * scale + offset;

    float a = z.x;
    float b = z.y;

    int i = 0;
    for(; i <= maxIterations; i++)
    {
        float a_ = a * a - b * b + z.x;
        float b_ = 2 * a * b + z.y;

        a = a_;
        b = b_;

        if (a * a + b * b > 4)
            break;

    }

    float h = float(i) / maxIterations;

    vec3 hsv = vec3(h, 1, 1);

    fragColor = vec4(hsv2rgb(hsv), 1);
}
