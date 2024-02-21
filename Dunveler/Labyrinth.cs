using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Dunveler;

public class Labyrinth
{
    const int GLSL_VERSION = 330;

    static Model cube = LoadModelFromMesh(GenMeshCube(-10.0f, -10.0f, -10.0f));
    static Shader lightShader = LoadShader(
        "resources/shaders/glsl330/lighting.vs",
        "resources/shaders/glsl330/lighting.fs"
    );
    static Light[] lights = new Light[1];

    public static unsafe void StartLabyrinth()
    {
        // Get some required shader loactions
        lightShader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(lightShader, "viewPos");

        // ambient light level
        int ambientLoc = GetShaderLocation(lightShader, "ambient");
        float[] ambient = new[] { 0.1f, 0.1f, 0.1f, 1.0f };
        SetShaderValue(lightShader, ambientLoc, ambient, ShaderUniformDataType.Vec4);

        cube.Materials[0].Shader = lightShader;

        lights[0] = Rlights.CreateLight(
            0,
            LightType.Point,
            new Vector3(5, 9, 5),
            new Vector3(5, 9, 5),
            Color.Yellow,
            lightShader
        );
    }

    public static unsafe void DrawLabyrinth()
    {
        Rlights.UpdateLightValues(lightShader, lights[0]);

        // Update the light shader with the camera view position
        SetShaderValue(
            lightShader,
            lightShader.Locs[(int)ShaderLocationIndex.VectorView],
            Player.camera.Position,
            ShaderUniformDataType.Vec3
        );

        ClearBackground(Color.Black);

        BeginMode3D(Player.camera);

        DrawSphereEx(lights[0].position, 0.2f, 8, 8, lights[0].color);
        DrawModel(cube, new Vector3(5, 9, 5), 1.0f, Color.White);

        EndMode3D();
    }

    public static void UnloadLabrinth()
    {
        UnloadModel(cube);
        UnloadShader(lightShader);
    }
}