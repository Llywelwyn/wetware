using System.Numerics;
using Raylib_cs;

namespace Wetware.Assets;

public enum Sprite
{
    Player = 205,
    WallTop = 301,
    Wall = 302,
}

public class Atlas(string path)
{
    private Texture2D m_texture = Raylib.LoadTexture(path);
    public readonly Vector2 TileSize = new(16, 24);
    private const int m_columns = 102;

    private Rectangle GetSpriteSourceRect(Sprite s)
    {
        int index = (int)s;
        int col = index % m_columns;
        int row = index / m_columns;
        return new Rectangle(col * TileSize.X, row * TileSize.Y, TileSize.X, TileSize.Y);
    }

    public void Draw(Sprite s, Vector2 position, Color colour)
    {
        Raylib.DrawTextureRec(m_texture, GetSpriteSourceRect(s), position * TileSize, colour);
    }
}
