using System.Collections.Generic;

public class QueueScreenManager
{
    private Queue<Popup> _popupQueue = new();

    public void ShowPopup(Popup popup)
    {
        _popupQueue.Enqueue(popup);
        ShowNextPopup();
    }

    public void ShowNextPopup()
    {
        if (_popupQueue.Count == 0)
        {
            return;
        }

        var popup = _popupQueue.Dequeue();
        popup.Show(null, ShowNextPopup);
    }
}