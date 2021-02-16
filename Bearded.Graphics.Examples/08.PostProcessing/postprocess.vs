#version 150

in vec2 v_position;

out vec2 uv;

void main()
{
    uv = v_position * 0.5 + 0.5;
	gl_Position = vec4(v_position, 0.0, 1.0);
}
