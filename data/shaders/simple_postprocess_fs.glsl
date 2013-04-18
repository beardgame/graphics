#version 130

uniform sampler2D diffuseTexture;

uniform vec2 pixelSize;

in vec2 p_texCoord;

out vec4 fragColor;

void main()
{
	vec2 uv = p_texCoord;

	vec4 c = texture(diffuseTexture, uv) * 3;

	uv.x = p_texCoord.x + pixelSize.x * 1.5;
	uv.y = p_texCoord.y;
	c += texture(diffuseTexture, uv) * 2;

	uv.x = p_texCoord.x - pixelSize.x * 1.5;
	uv.y = p_texCoord.y;
	c += texture(diffuseTexture, uv) * 2;

	uv.x = p_texCoord.x;
	uv.y = p_texCoord.y + pixelSize.y * 1.5;
	c += texture(diffuseTexture, uv) * 2;

	uv.x = p_texCoord.x;
	uv.y = p_texCoord.y - pixelSize.y * 1.5;
	c += texture(diffuseTexture, uv) * 2;


	uv.x = p_texCoord.x + pixelSize.x * 3.5;
	uv.y = p_texCoord.y;
	c += texture(diffuseTexture, uv);

	uv.x = p_texCoord.x - pixelSize.x * 3.5;
	uv.y = p_texCoord.y;
	c += texture(diffuseTexture, uv);

	uv.x = p_texCoord.x;
	uv.y = p_texCoord.y + pixelSize.y * 3.5;
	c += texture(diffuseTexture, uv);

	uv.x = p_texCoord.x;
	uv.y = p_texCoord.y - pixelSize.y * 3.5;
	c += texture(diffuseTexture, uv);


	uv.x = p_texCoord.x + pixelSize.x * 5.5;
	uv.y = p_texCoord.y;
	c += texture(diffuseTexture, uv);

	uv.x = p_texCoord.x - pixelSize.x * 5.5;
	uv.y = p_texCoord.y;
	c += texture(diffuseTexture, uv);

	uv.x = p_texCoord.x;
	uv.y = p_texCoord.y + pixelSize.y * 5.5;
	c += texture(diffuseTexture, uv);

	uv.x = p_texCoord.x;
	uv.y = p_texCoord.y - pixelSize.y * 5.5;
	c += texture(diffuseTexture, uv);

	c /= 19;

	fragColor = c;
}