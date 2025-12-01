using tl2_tp8_2025_dan1dlm.Interfaces;

namespace tl2_tp8_2025_dan1dlm.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUsuariosRepository _usuariosRepository;
    private readonly IHttpContextAccessor _httpContextAccesor;
    private readonly HttpContext context;
    public AuthenticationService(IUsuariosRepository usuariosRepository, IHttpContextAccessor httpContextAccessor)
    {
        _usuariosRepository = usuariosRepository;
        _httpContextAccesor = httpContextAccessor;
        context = _httpContextAccesor.HttpContext;
    }
    public bool Login(string username, string password)
    {

        var user = _usuariosRepository.GetUser(username, password);

        if (user != null)
        {
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no esta disponible.");
            }

            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", user.User);
            context.Session.SetString("UserNombre", user.Nombre);
            context.Session.SetString("Rol", user.Rol);

            return true;
        }

        return false;
    }

    public void Logout()
    {
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext no está disponible.");
        }

        context.Session.Clear();
    }

    public bool IsAuthenticated()
    {
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext no está disponible.");
        }

        return context.Session.GetString("IsAuthenticated") == "true";
    }

    public bool HasAccessLevel(string requiredAccessLevel)
    {
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext no está disponible.");
        }

        return context.Session.GetString("Rol") == requiredAccessLevel;
    }
}

