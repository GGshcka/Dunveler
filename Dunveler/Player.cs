using System.Diagnostics;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Dunveler;

public static unsafe class Player
{
    private static bool cur = false;

    public static Camera3D Camera = new Camera3D();

    internal static float Sens = 0.15f; //Сенса для мыши от 0.01 до 1;
    internal static float Speed; //Скорость перемещения;

    private static readonly Vector2 CenterScreen = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
    private static readonly Vector2 CenterScreenPlus7 = new Vector2(GetScreenWidth() / 2 + 7, GetScreenHeight() / 2 + 7);
    private static readonly Vector2 CenterScreenPlusMinus7 = new Vector2(GetScreenWidth() / 2 - 7, GetScreenHeight() / 2 + 7);
    private static readonly int ThickCur = 3;

    public static void CameraStart()
    {
        Camera.Position = new Vector3(0f, -.2f, 0f); // Camera position
        Camera.Target = new Vector3(1.0f, -.2f, 0.0f);      // Camera looking at point
        Camera.Up = new Vector3(.0f, 1.0f, .0f);          // Camera up vector (rotation towards target)
        Camera.FovY = 65.0f;                                // Camera field-of-view Y
        Camera.Projection = CameraProjection.Perspective;

        CameraYaw(ref Camera, -135 * DEG2RAD, true);
        CameraPitch(ref Camera, -45 * DEG2RAD, true, true, false);
    }

    public static void CameraUpdater()
    {
        Vector3 oldCamPos = Camera.Position;

        if (cur == false)
        {
            UpdateCameraPro(
                ref Camera,
                new Vector3(
                    IsKeyDown(KeyboardKey.W)*Speed - IsKeyDown(KeyboardKey.S) *Speed,
                    IsKeyDown(KeyboardKey.D)*Speed - IsKeyDown(KeyboardKey.A) *Speed,
                    0.0f),
                new Vector3(
                    GetMouseDelta().X*Sens,
                    GetMouseDelta().Y*Sens,
                    0.0f),
                0.0f);

            DrawLineEx(CenterScreenPlus7, CenterScreen, ThickCur, Color.White);
            DrawLineEx(CenterScreenPlusMinus7, CenterScreen, ThickCur, Color.White);
        }

        // Check player collision (we simplify to 2D collision detection)
        Vector2 playerPos = new Vector2(Camera.Position.X, Camera.Position.Z);
        float playerRadius = 0.1f;  // Collision radius (player is modelled as a cilinder for collision)

        int playerCellX = (int)(playerPos.X - Labyrinth.mapPosition.X + 0.5f);
        int playerCellY = (int)(playerPos.Y - Labyrinth.mapPosition.Z + 0.5f);

        // Out-of-limits security check
        if (playerCellX < 0) playerCellX = 0;
        else if (playerCellX >= Labyrinth.cubicmap.Width) playerCellX = Labyrinth.cubicmap.Width - 1;

        if (playerCellY < 0) playerCellY = 0;
        else if (playerCellY >= Labyrinth.cubicmap.Height) playerCellY = Labyrinth.cubicmap.Height - 1;

        // Check map collisions using image data and player position
        // TODO: Improvement: Just check player surrounding cells for collision
        for (int y = 0; y < Labyrinth.cubicmap.Height; y++)
        {
            for (int x = 0; x < Labyrinth.cubicmap.Height; x++)
            {
                if ((Labyrinth.mapPixels[y * Labyrinth.cubicmap.Width + x].R == 255) &&
                    (Labyrinth.mapPixels[y * Labyrinth.cubicmap.Width + x].G == 255) &&
                    (Labyrinth.mapPixels[y * Labyrinth.cubicmap.Width + x].B == 255) &&
                    (CheckCollisionCircleRec(playerPos, playerRadius,
                    new Rectangle(Labyrinth.mapPosition.X - 0.5f + x * 1.0f, Labyrinth.mapPosition.Z - 0.5f + y * 1.0f, 1.0f, 1.0f ))))
                {
                    // Collision detected, reset camera position
                    Camera.Position = oldCamPos;
                }
            }
        }
    }

    internal static bool InfoDraw = false;

    public static void Controls()
    {
        if (IsKeyPressed(KeyboardKey.Tab))
        {
            if (cur == false)
            {
                EnableCursor();
                cur = true;
            }
            else
            {
                DisableCursor();
                cur = false;
            }
        }

        if (IsKeyPressed(KeyboardKey.U))
        {
            Camera.Position.Y += 0.1f;
            Camera.Target.Y += 0.1f;
        }

        if (IsKeyPressed(KeyboardKey.I))
        {
            Camera.Position.Y -= 0.1f;
            Camera.Target.Y -= 0.1f;
        }

        if (IsKeyPressed(KeyboardKey.F3)) InfoDraw = !InfoDraw;

        Speed = IsKeyDown(KeyboardKey.LeftShift) ? 0.08f : 0.04f;
    }
}