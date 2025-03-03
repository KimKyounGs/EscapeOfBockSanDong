using UnityEngine;
using UnityEngine.UI;

public class UIResolutionHandler : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    void Start()
    {

        int width = Screen.currentResolution.width;
        int height = Screen.currentResolution.height;
        Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);

        canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
    }
}
