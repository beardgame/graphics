#version 400

in vec2 v_position;

out vec2 fragment_z;

void main()
{
	gl_Position = vec4(v_position, 0.0, 1.0);

	fragment_z = v_position;
}
