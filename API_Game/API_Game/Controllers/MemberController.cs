using API_Game.Infrastructure;
using Demo_ASP_MVC_Modele.BLL.Entities;
using Demo_ASP_MVC_Modele.BLL.Interfaces;
using Demo_ASP_MVC_Modele.WebApp.Infrastructure;
using Demo_ASP_MVC_Modele.WebApp.Models;
using Demo_ASP_MVC_Modele.WebApp.Models.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Game.Controllers
{
 

    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {


        private IMemberService _service;

        private IGameService _gameService;


        // Token Manager est comme session manager
        private TokenManager _tokenManager;

        public MemberController(IMemberService service, IGameService gameService, TokenManager tokenManager)
        {
            _service = service;
            _gameService = gameService;
            _tokenManager = tokenManager;
        }


        [HttpPost("register")] // Si on a deux pot/get/... les mm on aura une erreur
        public IActionResult Register(MemberForm memberForm)
        {

            

            MemberModel memberReceive = new();

            try
            {
                _service.Register(memberForm.ToModel());
                return Ok();
            }

            // Récupération des deux exceptions
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


        [HttpPost("login")]

        public IActionResult Login(MemberFormLogin memberFormLogin)
        {


            MemberModel memberReceiveLog;

            try
            {
                memberReceiveLog = _service.Login(memberFormLogin.Pseudo, memberFormLogin.Pwd);
                memberReceiveLog.Token = _tokenManager.GenerateToken(memberReceiveLog);

                if (memberReceiveLog != null)
                {
                    return Ok(memberReceiveLog.Token); 
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception e)
            {
                return BadRequest();
            }



        }




        [HttpPost("logout")]

        public IActionResult Logout(MemberModel member)
        {
            member = null;

            return Ok();
        }

        [Authorize("Auth")] // Seul les utilisateurs peuvent récupérer le profil
        [Authorize("Admin")]  // Seul les admins peuvent récupérer le profil
        [HttpGet("favoris/{id}/game")]
        public IActionResult Profil(int id)
        {
            IEnumerable<GameModel> m = _gameService.GetByMemberId(id);
           
                return Ok(m);
           

        }
    }


}

