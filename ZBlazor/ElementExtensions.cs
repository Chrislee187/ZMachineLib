using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ZBlazor
{
    public static class ElementExtensions
    {
        public static void Focus(this ElementReference elementRef, IJSRuntime jsRuntime)
        {
            jsRuntime.InvokeAsync<object>(
                "interopElement.focus", elementRef);
        }

        public static void ScrollToBottom(this ElementReference elementRef, IJSRuntime jsRuntime)
        {
            jsRuntime.InvokeAsync<object>(
                "interopElement.scrollToBottom", elementRef);
        }
        public static void EnsureVisible(this ElementReference elementRef, IJSRuntime jsRuntime)
        {
            jsRuntime.InvokeAsync<object>(
                "interopElement.ensureVisible", elementRef);
        }
        public static void Click(this ElementReference elementRef, IJSRuntime jsRuntime)
        {
            jsRuntime.InvokeAsync<object>(
                "interopElement.click", elementRef);
        }
    }
}