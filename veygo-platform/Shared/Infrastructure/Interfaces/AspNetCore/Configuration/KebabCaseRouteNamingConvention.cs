using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Veygo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

namespace Veygo.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;

/// <summary>
///     Replaces the <c>[controller]</c> route token with its kebab-case equivalent
///     (e.g. <c>BlockedDates</c> → <c>blocked-dates</c>).
/// </summary>
public class KebabCaseRouteNamingConvention : IControllerModelConvention
{
    /// <summary>
    ///     Applies the kebab-case naming convention to the controller and its actions.
    /// </summary>
    /// <param name="controller">The <see cref="ControllerModel" />.</param>
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);

        foreach (var selector in controller.Actions.SelectMany(action => action.Selectors))
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);
    }

    private static AttributeRouteModel? ReplaceControllerTemplate(SelectorModel selector, string name)
    {
        return selector.AttributeRouteModel != null
            ? new AttributeRouteModel
            {
                Template = selector.AttributeRouteModel.Template?.Replace("[controller]", name.ToKebabCase())
            }
            : null;
    }
}
