using Microsoft.JSInterop;

namespace BlazorEcommerce.Client.Services
{
    public class BrowserService
    {
        private IJSRuntime JS = null;

        public event EventHandler<int> Resize;

        private int browserWidth;
        private int browserHeight;

        public async void Init(IJSRuntime js)
        {
            // enforce single invocation
            if (JS == null)
            {
                this.JS = js;
                await JS.InvokeAsync<string>("resizeListener", DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public void SetBrowserDimensions(int jsBrowserWidth, int jsBrowserHeight)
        {
            browserWidth = jsBrowserWidth;
            browserHeight = jsBrowserHeight;
            // For simplicity, we're just using the new width
            this.Resize?.Invoke(this, jsBrowserWidth);
        }
    }
}
