using Crosscutting;

namespace Properties.Api.HttpResponse;

public static class ApiResponseExtension
{
    /// <summary>
    /// Convierte un <see cref="Result{T}"/> en un <see cref="IResult"/> 
    /// utilizando una función personalizada para formatear la respuesta en caso de éxito.
    /// </summary>
    /// <typeparam name="TIn">Tipo de dato contenido en el resultado.</typeparam>
    /// <param name="result">Resultado de la operación en el dominio.</param>
    /// <param name="onSuccess">
    /// Función que recibe un objeto <see cref="ApiResponseSuccessful{T}"/> 
    /// y devuelve la respuesta HTTP correspondiente en caso de éxito.
    /// </param>
    /// <returns>
    /// - Si <see cref="Result{T}.IsSuccess"/> es <c>true</c>, devuelve el <see cref="IResult"/> generado por <paramref name="onSuccess"/>.  
    /// - En caso contrario, devuelve un error HTTP usando <see cref="CustomResults.Problem(Result)"/>.
    /// </returns>
    public static IResult ToHttpResponse<TIn>(this Result<TIn> result, Func<ApiResponseSuccessful<TIn>, IResult> onSuccess)
    {
        var resultSuccess = ApiResponseSuccessful<TIn>.Create(result.Value, result.TotalData);
        return result.IsSuccess ? onSuccess(resultSuccess) :
            CustomResults.Problem(result);
    }

    /// <summary>
    /// Convierte un <see cref="Result{T}"/> en un <see cref="IResult"/> 
    /// devolviendo una respuesta <c>200 OK</c> con el contenido exitoso por defecto.
    /// </summary>
    /// <typeparam name="TIn">Tipo de dato contenido en el resultado.</typeparam>
    /// <param name="result">Resultado de la operación en el dominio.</param>
    /// <returns>
    /// - Si <see cref="Result{T}.IsSuccess"/> es <c>true</c>, devuelve <c>200 OK</c> con un <see cref="ApiResponseSuccessful{T}"/> en el body.  
    /// - En caso contrario, devuelve un error HTTP usando <see cref="CustomResults.Problem(Result)"/>.
    /// </returns>
    public static IResult ToHttpResponse<TIn>(this Result<TIn> result)
    {
        var resultSuccess = ApiResponseSuccessful<TIn>.Create(result.Value, result.TotalData);
        return result.IsSuccess ? Results.Ok(resultSuccess) :
            CustomResults.Problem(result);
    }

    /// <summary>
    /// Convierte un <see cref="Result{T}"/> en un <see cref="IResult"/> 
    /// utilizando una función personalizada que no recibe parámetros adicionales.
    /// </summary>
    /// <typeparam name="TIn">Tipo de dato contenido en el resultado.</typeparam>
    /// <param name="result">Resultado de la operación en el dominio.</param>
    /// <param name="onSuccess">
    /// Función que devuelve la respuesta HTTP en caso de éxito.
    /// </param>
    /// <returns>
    /// - Si <see cref="Result{T}.IsSuccess"/> es <c>true</c>, devuelve el <see cref="IResult"/> generado por <paramref name="onSuccess"/>.  
    /// - En caso contrario, devuelve un error HTTP usando <see cref="CustomResults.Problem(Result)"/>.
    /// </returns>
    public static IResult ToHttpResponse<TIn>(this Result<TIn> result, Func<IResult> onSuccess)
    {
        return result.IsSuccess ? onSuccess() :
            CustomResults.Problem(result);
    }
}
