namespace Wetware.Screens;

public class Stack
{
    private readonly List<Screen> m_stack = new();

    public void Push(Screen s)
    {
        s.OnEnter();
        m_stack.Add(s);
    }

    public void Pop()
    {
        if (m_stack.Count == 0) return;
        var top = m_stack[^1];
        top.OnExit();
        m_stack.RemoveAt(m_stack.Count - 1);
    }

    public void Render()
    {
        int start = 0;
        // Finds the first blocking screen, if one exists, and starts there,
        // neglecting to render any screens below it in the m_stack.
        for (int i = m_stack.Count - 1; i >= 0; i--)
        {
            if (m_stack[i].BlocksRender)
            {
                start = i;
                break;
            }
        }
        for (int i = start; i < m_stack.Count; i++) m_stack[i].Render();
    }
}
