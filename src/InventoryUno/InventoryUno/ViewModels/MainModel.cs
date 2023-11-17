using System.Diagnostics;
using Dotnet.Cblite.Inventory.Shared.Services;
using Uno.Extensions.Reactive.Bindings;

namespace InventoryUno.ViewModels;

public partial record MainModel 
{
    private readonly IAuthenticationService _authenticationService;
    public IState<string> Username => State<string>.Empty(this);
    public IState<string> Password => State<string>.Empty(this);

    public MainModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    public void Authenticate()
    {
        Console.WriteLine("Hello World");   
        Debug.WriteLine("Hello World");   
    }


}
