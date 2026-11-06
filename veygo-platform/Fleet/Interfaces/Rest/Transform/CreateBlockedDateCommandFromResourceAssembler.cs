using Veygo.Platform.Fleet.Domain.Model.Commands;
using Veygo.Platform.Fleet.Interfaces.Rest.Resources;
using Veygo.Platform.Shared.Interfaces.Rest.Transform;

namespace Veygo.Platform.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a <see cref="CreateBlockedDateCommand" /> from a <see cref="CreateBlockedDateResource" />.
/// </summary>
public static class CreateBlockedDateCommandFromResourceAssembler
{
    public static CreateBlockedDateCommand ToCommandFromResource(CreateBlockedDateResource resource) =>
        new(resource.VehicleId, DateValueAssembler.ToDate(resource.Date, "date"));
}
