using Volo.Abp.Application.Services;
using FYPTourneyPro.Localization;

namespace FYPTourneyPro.Services;

/* Inherit your application services from this class. */
public abstract class FYPTourneyProAppService : ApplicationService
{
    protected FYPTourneyProAppService()
    {
        LocalizationResource = typeof(FYPTourneyProResource);
    }
}