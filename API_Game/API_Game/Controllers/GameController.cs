using Demo_ASP_MVC_Modele.BLL.Entities;
using Demo_ASP_MVC_Modele.BLL.Interfaces;
using Demo_ASP_MVC_Modele.WebApp.Infrastructure;
using Demo_ASP_MVC_Modele.WebApp.Models;
using Demo_ASP_MVC_Modele.WebApp.Models.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
       

        private IGameService _service;

        public GameController(IGameService service)
        {
            _service = service;
        }

        [HttpGet("liste")]
        public IActionResult Index()
        {
            return Ok(_service.GetAll().ToViewModel());
        }




        [HttpPost("ajouter")]
        public IActionResult Add(GameForm gameForm)
        {


            try
            {
                int id = _service.Insert(gameForm.ToModel()); return Ok();
            }
            catch (Exception e)
            {

                return BadRequest();
            }

        }

        [HttpGet("Detail/{id}")]
        public IActionResult Details(int id)
        {
            try
            {
                Game game = _service.GetById(id).ToViewModel();
                return Ok(game);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest();
            }
        }

        [HttpDelete("supprimer")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }


        [HttpPut("modifier/{id}")]
        public IActionResult Update([FromRoute] int id, GameForm gameForm)
        {

          
            try
            {
                GameModel data = gameForm.ToModel();
                data.Id = id;

                bool isOk = _service.Update(data);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }





        [HttpPost("favoris/{gameId}/member/{memberId}")]
        public IActionResult AddFav(int memberId, int gameId)
        {
            try
            {

                _service.AddFavoriteGame(memberId, gameId);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest();
            }

        }

    }
}
