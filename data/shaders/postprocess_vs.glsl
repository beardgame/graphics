#version 130

in vec2 v_position;
in vec2 v_texCoord;

out vec2 p_texCoord;

void main()
{
	gl_Position = vec4(v_position, 0, 1);
	p_texCoord = v_texCoord;
}