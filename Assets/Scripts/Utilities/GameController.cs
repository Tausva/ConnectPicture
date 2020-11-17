public static class GameController
{
    static UIControls uiControls;

    public static void InjectUiControls(UIControls uIControls)
    {
        uiControls = uIControls;
    }

    public static void StartEndSequence()
    {
        if (uiControls != null)
        {
            uiControls.EnableButtonsPanel();
        }
    }
}