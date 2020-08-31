#version 150

uniform sampler2D inTexture;
uniform vec2 pixelSize;

in vec2 uv;

out vec4 fragColor;

void main()
{
    vec4 left = texture(inTexture, uv + vec2(pixelSize.x, 0));
    vec4 right = texture(inTexture, uv + vec2(-pixelSize.x, 0));
    vec4 up = texture(inTexture, uv + vec2(0, pixelSize.y));
    vec4 down = texture(inTexture, uv + vec2(0, -pixelSize.y));

    fragColor = vec4(max(abs(left - right), abs(up - down)).rgb, 1);
}
