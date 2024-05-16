using Sopra.Screens.MenuScreens;

namespace Sopra.Screens
{
    
    /// <summary>
    /// Factory for screens.
    /// The actuall implementation is provided by NInject.
    /// </summary>
    public interface IScreenFactory
    {
        MainMenu CreateMainScreen();

        OptionMenu CreateOptionScreen();

        GameScreen CreateGameScreen();

        HudScreen CreateHudScreen();

        PauseMenu CreatePauseScreen();

        SimplePauseMenu CreateSimplePauseScreen();

        GameOverMenu CreateGameOverScreen();

        LoadingScreen CreateLoadingScreen();

        LoadingScreenDeath CreateLoadingScreenDeath();

        YouWonMenu CreateYouWonScreen();

        StatMenu CreateStatScreen();

        AchievementMenu CreateAchievementScreen();

        CreditsMenu CreateCreditsScreen();

        QuadScreen CreateQuadScreen();

        TechDemoStatsScreen CreatTechDemoStatsScreen();

    }
}