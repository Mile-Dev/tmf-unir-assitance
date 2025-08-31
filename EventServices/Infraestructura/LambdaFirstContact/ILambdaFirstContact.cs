namespace EventServices.Infraestructura.LambdaFirstContact
{
    public interface ILambdaFirstContact
    {
        Task<string> InvokeLambdaGetByIdAsync(string functionArn, string resourcePath, string path, string id);
    }
}
