using UnityEngine.UIElements;

public abstract class UIPanel
{
    protected VisualElement RootElement;
    protected UIManager Manager;

    protected UIPanel(VisualElement root, UIManager manager, string elementName)
    {
        RootElement = root.Q<VisualElement>(elementName);
        Manager = manager;
    }

    public virtual void Show()
    {
        if (RootElement != null)
        {
            // Bỏ class "hidden" đi để UI hiển thị lại theo mặc định (hoặc Flex)
            RootElement.RemoveFromClassList("hidden");
        }
    }

    public virtual void Hide()
    {
        if (RootElement != null)
        {
            // Thêm class "hidden" vào để áp dụng display: none từ USS
            RootElement.AddToClassList("hidden");
        }
    }
}