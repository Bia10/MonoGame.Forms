﻿using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MonoGame.Forms.Controls
{
    /// <summary>
    /// This class mainly creates the <see cref="GraphicsDeviceControl.GraphicsDevice"/>, the <see cref="GraphicsDeviceControl.SwapChainRenderTarget"/> and the game loop.
    /// It inherits from <see cref="GraphicsDeviceControl"/>, which makes its childs available as a tool box control.
    /// </summary>
    public abstract class GameControl : GraphicsDeviceControl
    {
        GameTime gameTime;
        Stopwatch timer;
        TimeSpan elapsed;

        /// <summary>
        /// Basic initializing of the game control.
        /// It starts a <see cref="Stopwatch"/> and creates the mouse events.
        /// </summary>
        protected override void Initialize()
        {
            if (timer == null) timer = Stopwatch.StartNew();

            Application.Idle -= GameLoop;
            Application.Idle += GameLoop;
        }

        /// <summary>
        /// Basic drawing logic.
        /// </summary>
        protected override void Draw()
        {
            Draw();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (Visible && AutomaticInvalidation)
            {
                gameTime = new GameTime(timer.Elapsed, timer.Elapsed - elapsed);
                elapsed = timer.Elapsed;

                UpdateMousePositions();

                Update(gameTime);
                Invalidate();
            }
        }

        /// <summary>
        /// Basic update logic.
        /// You must implement this to your custom class, so you can write your own update logic for the game loop.
        /// </summary>
        /// <param name="gameTime">The <see cref="GameTime"/> reflects the current time of the game loop</param>
        protected abstract void Update(GameTime gameTime);
    }
}