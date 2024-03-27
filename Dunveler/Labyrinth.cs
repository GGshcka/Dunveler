using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Dunveler.Resources.Resources;

namespace Dunveler;

public static unsafe class Labyrinth
{
    public static Model model;
    public static Texture2D cubicmap, texture;
    public static unsafe Color* mapPixels;
    public static Vector3 mapPosition, exitPosition;
    public static float exitSize = 0.5f;
    public static int currentTimeSec = 0, frameCount = 0, currentTimeMins = 0;

    public static void Start(Image imMap)
    {
        Random randEnv = new();
        Byte[][] mapStyle =
        {
            dungeon_atlas, 
            mossy_atlas,
            interworld_atlas,
            midasplace_atlas,
            backrooms_atlas,
        };
        Image imAtlasMap = LoadImageFromMemory(".png", mapStyle[randEnv.Next(5)]);

        cubicmap = LoadTextureFromImage(imMap);
        Mesh mesh = GenMeshCubicmap(imMap, new Vector3( 1.0f, 1.0f, 1.0f ));
        model = LoadModelFromMesh(mesh);

        texture = LoadTextureFromImage(imAtlasMap);
        UnloadImage(imAtlasMap);

        SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

        mapPixels = LoadImageColors(imMap);
        UnloadImage(imMap);

        mapPosition = new Vector3(0f, 0.2f, -2f);
        exitPosition = new Vector3(
            LabyrinthGenerator.WIDTH - 3 + mapPosition.X,
            mapPosition.Y,
            LabyrinthGenerator.HEIGHT - 3 + mapPosition.Z);
    }

    public static void Draw()
    {
        ClearBackground(Color.Black);
        BeginMode3D(Player.Camera);

        DrawModel(model, mapPosition, 1.0f, Color.White);

        DrawCube(exitPosition, exitSize, 2f, exitSize, ColorAlpha(Color.Yellow, 0.5f));

        EndMode3D();
    }

    public static string TimerText = "00:00";

    public static void Timer()
    {
        if (frameCount > GetFPS())
        {
            frameCount = 0;
            currentTimeSec++;
        }

        frameCount++;

        if (currentTimeSec == 60)
        {
            currentTimeMins++;
            currentTimeSec = 0;
        }

        if (currentTimeMins < 10) { TimerText = $"0{currentTimeMins}"; }
        else TimerText = $"{currentTimeMins}";
        if (currentTimeSec < 10) { TimerText += $":0{currentTimeSec}"; }
        else TimerText += $":{currentTimeSec}";
    }
    public static void TimerClear() { frameCount = currentTimeMins = currentTimeSec = 0; }

    public static void Unloading()
    {
        UnloadImageColors(mapPixels);
        UnloadTexture(cubicmap);
        UnloadTexture(texture);
        UnloadModel(model);
    }
}