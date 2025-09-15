using Wetware.Components;

namespace Wetware.Screens;

public abstract class Screen(Position origin, Position size)
{
    protected Position m_origin = origin;
    protected Position m_size = size;

    /// <summary>
    /// Whether this screen has processed an input, and should consume it.
    /// </summary>
    public bool HandledInput { get; protected set; } = false;

    /// <summary>
    /// Whether this screen should prevent rendering screens below it.
    /// </summary>
    public bool BlocksRender { get; protected set; } = false;

    public virtual void OnEnter() { }
    public virtual void OnExit() { }

    public abstract void HandleInput();
    public abstract void Render();
}
