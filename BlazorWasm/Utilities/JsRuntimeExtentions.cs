using Microsoft.JSInterop;

namespace BlazorWasm.Utilities
{
    public static class JsRuntimeExtentions
    {
        //For Success Message of an opration
        public static ValueTask ToastrSuccess(this IJSRuntime jSRuntime, string message)
        {
            return jSRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }
        //for error message of an opration
        public static ValueTask ToastrError(this IJSRuntime jsSRuntime, string message)
        {
            return jsSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }

        //SweetAlert2
        public static ValueTask SweetAlert2Success(this IJSRuntime jSRuntime, string message)
        {
            return jSRuntime.InvokeVoidAsync("ShowSweetAlert2", "success", message);
        }
        public static ValueTask SweetAlert2Error(this IJSRuntime jsSRuntime, string message)
        {
            return jsSRuntime.InvokeVoidAsync("ShowSweetAlert2", "error", message);
        }

    }
}
