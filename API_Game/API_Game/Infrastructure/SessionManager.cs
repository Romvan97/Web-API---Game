using Demo_ASP_MVC_Modele.BLL.Entities;
using Demo_ASP_MVC_Modele.WebApp.Models;
using System.Text.Json;


public class SessionManager
{
    private ISession _session;
    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _session = httpContextAccessor.HttpContext.Session;
    }
   
    public MemberModel CurrentMember
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_session.GetString("user")))
            {  
                return null;  
            }
            else
            {
                return JsonSerializer.Deserialize<MemberModel>(_session.GetString("user"));
            }
        }

        set
        {
            _session.SetString(("user"), JsonSerializer.Serialize(value));
        }

    }

    
}