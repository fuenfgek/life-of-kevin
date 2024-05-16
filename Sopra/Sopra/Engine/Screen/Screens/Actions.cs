
namespace Sopra.Engine.Screen.Screens
{
    /// <summary>
    /// The class, that manages the action with menu and options
    /// </summary>
    /// <author>Anushe Glushik</author>
    class Actions
    {
        public ScreenManager mScreenManager;
        public Actions(ScreenManager screenManager)
        {
            mScreenManager = screenManager;
        }

        /// <summary>
        /// Change the order of drawing menu, map and game screen.
        /// The method is called when "Spiel Starten" button is clicked.
        /// Menu goes under the map and game screen.
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="newMap"></param>
        /// <param name="gameScreen"></param>
        public void StartGameAction(Menu menu, LoadMap newMap, TestlevelScreen gameScreen)
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(newMap);
            mScreenManager.AddScreen(gameScreen);
        }

        /// <summary>
        /// Remove menu screen when "Optionen" is clicked and show options screen
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="newMap"></param>
        /// <param name="gameScreen"></param>
        /// <param name="options"></param>
        public void OpenOptionsAction(Menu menu, Options options)
        {
            mScreenManager.RemoveScreen(menu);
            mScreenManager.AddScreen(options);
        }

        /// <summary>
        /// Remove options screen and show menu screen
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="newMap"></param>
        /// <param name="gameScreen"></param>
        /// <param name="options"></param>
        public void OptionsZurueckAction(Menu menu, LoadMap newMap, TestlevelScreen gameScreen, Options options)
        {
            mScreenManager.RemoveScreen(options);
            mScreenManager.AddScreen(menu);
        }

        public void PauseTheGameAction(TestlevelScreen testlevelS, LoadMap testMapS, PauseScreen pauseMenuS)
        {
            mScreenManager.AddScreen(pauseMenuS);
        }
        public void ReturnToTheGameAction(TestlevelScreen testlevelS, LoadMap testMapS, PauseScreen pauseMenuS)
        {
            mScreenManager.RemoveScreen(pauseMenuS);
        }

        public void ReturnToMenuAction(Menu mainMenuS, TestlevelScreen testlevelS, LoadMap testMapS, PauseScreen pauseMenuS)
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(mainMenuS);
        }

        public void OpenOptionsPauseAction(Options options, PauseScreen pauseMenuS)
        {
            mScreenManager.RemoveScreen(pauseMenuS);
            mScreenManager.AddScreen(options);
        }
    }
}
