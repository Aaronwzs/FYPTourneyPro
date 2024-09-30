using Microsoft.Extensions.Localization;
using FYPTourneyPro.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace FYPTourneyPro;

[Dependency(ReplaceServices = true)]
public class FYPTourneyProBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<FYPTourneyProResource> _localizer;

    public FYPTourneyProBrandingProvider(IStringLocalizer<FYPTourneyProResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}