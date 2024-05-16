using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sopra.ECS;
using Sopra.Input;
using Sopra.Logic.UserInput;
using Sopra.Maps;

namespace Sopra.Screens
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GameScreen : AbstractScreen
    {
        private Level mLevel;
        private readonly MenuActions mMenuActions;
        public bool IsTechDemo { private get; set; }

        public GameScreen(MenuActions menuActions)
            : base(false, false)
        {
            mMenuActions = menuActions;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mLevel.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            mLevel.Update(gameTime);
        }

        public void AssignLevel(Level level)
        {
            mLevel = level;
        }

        public override bool HandleKeyEvent(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Keys.Escape && keyEvent.State == KeyState.Up)
            {
                if (IsTechDemo)
                {
                    mMenuActions.OpenSimplePauseScreen();
                }
                else
                {
                    mMenuActions.OpenPauseScreen();
                }

                return true;
            }

            return true;
        }

        public override bool HandleMouseEvent(MouseEvent mouseEvent)
        {
            var events = Events.Instance;

            if (mouseEvent.PositionReleased.HasValue)
            {
                switch (mouseEvent.Button)
                {
                    case MouseButton.Left:
                        events.Fire(new LeftClickReleasedE());
                        break;

                    case MouseButton.Middle:
                        break;

                    case MouseButton.Right:
                        events.Fire(new RightClickReleasedE());
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (mouseEvent.Button)
                {
                    case MouseButton.Left:
                        events.Fire(new LeftClickPressedE());
                        break;

                    case MouseButton.Middle:
                        break;

                    case MouseButton.Right:
                        events.Fire(new RightClickPressedeE());
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return true;
        }
    }
}