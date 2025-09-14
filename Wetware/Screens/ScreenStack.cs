namespace Wetware.Screens;

public class Stack
{
    private readonly List<Screen> stack = new();

    public void Push(Screen s)
    {
        s.OnEnter();
        stack.Add(s);
    }

    public void Pop()
    {
        if (stack.Count == 0) return;
        var top = stack[^1];
        top.OnExit();
        stack.RemoveAt(stack.Count - 1);
    }

    public void Render()
    {
        int start = 0;
        // Finds the first blocking screen, if one exists, and starts there,
        // neglecting to render any screens below it in the stack.
        for (int i = stack.Count - 1; i >= 0; i--)
        {
            if (stack[i].BlocksRender)
            {
                start = i;
                break;
            }
        }
        for (int i = start; i < stack.Count; i++) stack[i].Render();
    }
}
