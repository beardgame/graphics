#version 150

// Here are the uniforms set by the IRenderSettings
uniform vec3 lightPosition;
uniform float lightRadius;
uniform vec4 lightColor;
uniform vec4 ambientLightColor;

in vec3 p_position;
in vec4 p_color;

out vec4 fragColor;

void main()
{
    vec3 vectorToLight = lightPosition - p_position;
    float distanceToLight = length(vectorToLight);

    vec3 unitX = dFdx(p_position);
    vec3 unitY = dFdy(p_position);
    vec3 normal = cross(unitX, unitY);

    float lightAngleScalar = max(0, dot(
        normalize(normal), normalize(vectorToLight)
    ));

    float lightDistanceScalar = max(0, 1 - (distanceToLight - lightRadius));

    lightDistanceScalar *= lightDistanceScalar * lightDistanceScalar;

    vec4 totalLight = ambientLightColor + lightColor * lightDistanceScalar * lightAngleScalar;

    fragColor = p_color * totalLight;
}
