namespace Contact.API.Controllers;

public static class ErrorController
{
    public static RouteGroupBuilder MapErrorApi(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}/throw", ThrowInvalidOpException)
            .DocumentGetRequest<ContactRecord>("ThrowError", "Throws demo exception");

        return group;
    }

    internal static Task<IResult> ThrowInvalidOpException(int id) => throw new InvalidOperationException(
        $"This is a sample request that throws exception with id: {id}"
    );
}
