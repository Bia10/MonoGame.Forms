﻿using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System;
using System.ComponentModel;

namespace MonoGame.Forms.Controls
{
    /// <summary>
    /// Inherit from this class in your custom class to create a draw control, which is selectable from the ToolBox during design time.
    /// It provides 'NO' game loop, but it's updated through invalidation (<see cref="System.Windows.Forms.Control.Invalidate()"/>).
    /// <remarks>This draw control is useful as a simple window, which doesn't need a classical game loop like a preview window for textures.</remarks>
    /// </summary>
    public abstract class DrawWindow : GraphicsDeviceControl
    {
        /// <summary>
        /// The <see cref="DrawService"/> of the <see cref="DrawWindow"/> draws the actual content of the draw control.
        /// </summary>
        [Browsable(false)]
        public DrawService Editor { get { return _Editor; } }
        private DrawService _Editor;

        /// <summary>
        /// Basic initializing.
        /// </summary>
        protected override void Initialize()
        {
            _Editor = new DrawService(_graphicsDeviceService, SwapChainRenderTarget);
            _Editor.Initialize();

            SwapChainRenderTargetRefreshed -= DrawWindow_UpdateSwapChainRenderTarget;
            MultiSampleCountRefreshed -= DrawWindow_UpdateMultiSampleCount;
            SwapChainRenderTargetRefreshed += DrawWindow_UpdateSwapChainRenderTarget;
            MultiSampleCountRefreshed += DrawWindow_UpdateMultiSampleCount;
        }

        private void DrawWindow_UpdateSwapChainRenderTarget(SwapChainRenderTarget obj)
        {
            _Editor.SwapChainRenderTarget = obj;
        }

        private void DrawWindow_UpdateMultiSampleCount(int obj)
        {
            _Editor.GetCurrentMultiSampleCount = obj;
        }

        /// <summary>
        /// Basic drawing.
        /// The draw control becomes updated though invalidation: <see cref="System.Windows.Forms.Control.Invalidate()"/>
        /// </summary>
        protected override void Draw()
        {
            if (_Editor != null)
            {
                UpdateMousePositions();
                Editor.UpdateMousePositions(GetRelativeMousePosition, GetAbsoluteMousePosition);
                Editor.IsMouseInsideControl = IsMouseInsideControl;

                _Editor.Draw();

                if (AutomaticInvalidation) Invalidate();
            }
        }

        /// <summary>
        /// Updates related Editor services when the <see cref="System.Windows.Forms.Control.ClientSize"/> changes.
        /// </summary>
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);

            if (Editor != null)
            {
                Editor.DisableRenderTargets();
                Editor.CamHoldPosition(ClientSize);
            }
        }

        /// <summary>
        /// In case the ClientSize was changed before activating the window, the cam position gets updated according to this changes.
        /// </summary>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Editor != null) Editor.CamHoldPosition(ClientSize);
        }

        /// <summary>
        /// Disposes the contents of the attached Editor.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _Editor?.Dispose();
            SwapChainRenderTargetRefreshed -= DrawWindow_UpdateSwapChainRenderTarget;
            MultiSampleCountRefreshed -= DrawWindow_UpdateMultiSampleCount;
        }
    }
}
