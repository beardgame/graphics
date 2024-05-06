using System;
using Evergine.Bindings.RenderDoc;
using static Evergine.Bindings.RenderDoc.RENDERDOC_OverlayBits;
using Api = Evergine.Bindings.RenderDoc.RenderDoc;

namespace Bearded.Graphics.Windowing;

public interface IRenderDoc
{
    string? Version => null;
    void ShowOverlay(bool visible) { }
    void ClearCaptureKeys() { }
    RenderDoc.FrameCapture? CaptureFrame(bool openReplayUIOnDispose = true) => null;
}

static partial class RenderDoc
{
    private sealed class DummyImplementation : IRenderDoc;

    private sealed class Implementation : IRenderDoc
    {
        // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
        private const uint defaultOverlayMask = (uint)(
            eRENDERDOC_Overlay_Enabled |
            eRENDERDOC_Overlay_FrameRate |
            eRENDERDOC_Overlay_FrameNumber
        );
        // ReSharper restore BitwiseOperatorOnEnumWithoutFlags

        private readonly Api api;

        public Implementation(Api api)
        {
            this.api = api;

            api.API.MaskOverlayBits(0, defaultOverlayMask);
        }

        string IRenderDoc.Version
        {
            get
            {
                int major, minor, patch;
                unsafe
                {
                    api.API.GetAPIVersion(&major, &minor, &patch);
                }
                return $"{major}.{minor}.{patch}";
            }
        }

        void IRenderDoc.ShowOverlay(bool visible)
        {
            var (keep, add) = visible
                ? (eRENDERDOC_Overlay_All, eRENDERDOC_Overlay_Enabled)
                : (~eRENDERDOC_Overlay_Enabled, eRENDERDOC_Overlay_None);

            // ReSharper disable once IntVariableOverflowInUncheckedContext
            api.API.MaskOverlayBits((uint)keep, (uint)add);
        }

        void IRenderDoc.ClearCaptureKeys()
        {
            unsafe
            {
                api.API.SetCaptureKeys((RENDERDOC_InputButton*)0, 0);
            }
        }

        FrameCapture? IRenderDoc.CaptureFrame(bool openReplayUIOnDispose)
            => new FrameCapture(api, openReplayUIOnDispose);
    }

    public readonly struct FrameCapture : IDisposable
    {
        private readonly Api api;
        private readonly bool openUIOnDispose;

        internal FrameCapture(Api api, bool openUIOnDispose)
        {
            this.api = api;
            this.openUIOnDispose = openUIOnDispose;
            api.API.StartFrameCapture(IntPtr.Zero, IntPtr.Zero);
        }

        void IDisposable.Dispose()
        {
            EndCapture(openUIOnDispose);
        }

        public bool EndCapture(bool openReplayUI = true)
        {
            var success = api.API.EndFrameCapture(IntPtr.Zero, IntPtr.Zero) == 1;

            if (success && openReplayUI)
            {
                openReplayUi();
            }

            return success;
        }

        private void openReplayUi()
        {
            var uiNotYetLaunched = api.API.ShowReplayUI() == 0;
            if (uiNotYetLaunched)
            {
                api.API.LaunchReplayUI(1, "");
            }
        }
    }
}
