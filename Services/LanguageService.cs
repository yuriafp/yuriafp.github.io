using Microsoft.JSInterop;
using Portfolio.Content;

namespace Portfolio.Services;

public sealed class LanguageService(IJSRuntime js)
{
    private const string StorageKey = "yuriafp.lang";

    public Lang Current { get; private set; } = Lang.Pt;

    public event Action? Changed;

    public async Task InitializeAsync()
    {
        var stored = Parse(await js.InvokeAsync<string?>("site.getLang", StorageKey));

        if (stored is null)
        {
            var browser = await js.InvokeAsync<string?>("site.browserLang");
            stored = browser?.StartsWith("pt", StringComparison.OrdinalIgnoreCase) == true
                ? Lang.Pt
                : Lang.En;
        }

        await SetAsync(stored.Value, persist: false);
    }

    public Task ToggleAsync() => SetAsync(Current == Lang.Pt ? Lang.En : Lang.Pt);

    public async Task SetAsync(Lang lang, bool persist = true)
    {
        Current = lang;
        var code = lang == Lang.Pt ? "pt" : "en";

        if (persist)
        {
            await js.InvokeVoidAsync("site.setLang", StorageKey, code);
        }

        await js.InvokeVoidAsync("site.setHtmlLang", lang == Lang.Pt ? "pt-BR" : "en");
        Changed?.Invoke();
    }

    private static Lang? Parse(string? value) => value switch
    {
        "pt" => Lang.Pt,
        "en" => Lang.En,
        _ => null,
    };
}
