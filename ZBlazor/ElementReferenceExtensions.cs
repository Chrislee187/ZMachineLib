using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ZBlazor
{
    public static class ElementReferenceExtensions
    {
        private static string ElementMethod(string methodName) => $"interopElement.{methodName}";
        public static void Focus(this ElementReference elementRef, IJSRuntime jsRuntime) 
            => jsRuntime.InvokeAsync<object>(ElementMethod("focus"), elementRef);

        public static void ScrollToBottom(this ElementReference elementRef, IJSRuntime jsRuntime) 
            => jsRuntime.InvokeAsync<object>(ElementMethod("scrollToBottom"), elementRef);

        public static void EnsureVisible(this ElementReference elementRef, IJSRuntime jsRuntime) 
            => jsRuntime.InvokeAsync<object>(ElementMethod("ensureVisible"), elementRef);

        public static void Click(this ElementReference elementRef, IJSRuntime jsRuntime) 
            => jsRuntime.InvokeAsync<object>(ElementMethod("click"), elementRef);
        public static void SetSelectionRange(this ElementReference elementRef, int start, int end, IJSRuntime jsRuntime) 
                    => jsRuntime.InvokeAsync<object>(ElementMethod("setSelectionRange"), elementRef, start, end);
    }
}