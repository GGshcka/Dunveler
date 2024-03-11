using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using System.ComponentModel;
using System.Reflection;

namespace Dunveler;

public static unsafe class Labyrinth
{
    public static Model model;
    public static Texture2D cubicmap, texture;
    public static unsafe Color* mapPixels;
    public static Vector3 mapPosition;

    public static void Start()
    {
        Image imMap = LoadImageFromMemory(".png", Resources.Resources.cubemap_lvlMaze);
        Image imAtlasMap = LoadImageFromMemory(".png", Resources.Resources.cubemap_atlas);

        cubicmap = LoadTextureFromImage(imMap);
        Mesh mesh = GenMeshCubicmap(imMap, new Vector3( 1.0f, 1.0f, 1.0f ));
        model = LoadModelFromMesh(mesh);

        texture = LoadTextureFromImage(imAtlasMap);

        SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

        mapPixels = LoadImageColors(imMap);
        UnloadImage(imMap);

        mapPosition = new Vector3(-1f, 0.0f, -3f);
    }

    public static void Draw()
    {
        ClearBackground(Color.White);
        BeginMode3D(Player.Camera);

        DrawModel(model, mapPosition, 1.0f, Color.White);

        EndMode3D();
    }

    public static void Unloading()
    {
        UnloadImageColors(mapPixels);
        UnloadTexture(cubicmap);
        UnloadTexture(texture);
        UnloadModel(model);
    }
}