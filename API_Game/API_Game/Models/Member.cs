using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Demo_ASP_MVC_Modele.WebApp.Models
{
    //Ne pas utiliser de displayName dans Une Api
    public class Member
    {
       
        public int Id { get; set; }

     
        public string Pseudo { get; set; }


        public string Email { get; set; }

  
        public string Pwd { get; set; }

   public string Token { get; set; }
        public bool IsAdmin { get; set; }   
    }

    public class MemberForm
    {


        [Required(ErrorMessage = "Votre login est requis")]
        
        public string Pseudo { get; set; }

    
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Votre Email est invalide")]
        public string Email { get; set; }

  
        [Required]
        [DataType(DataType.Password, ErrorMessage = "votre mot de passe est invalide")]
        public string Pwd { get; set; }

 
        [Required]
        [DataType(DataType.Password, ErrorMessage = "votre mot de passe est invalide")]
        [Compare("Pwd", ErrorMessage = "Les mots de passes ne correspondent pas")]
        public string PwdConfirm { get; set; }

    }

    public class MemberFormLogin
    {



        [Required(ErrorMessage = "Votre login est requis")]

        public string Pseudo { get; set; }


        [Required]
        [DataType(DataType.Password, ErrorMessage = "votre mot de passe est invalide")]
        public string Pwd { get; set; }


    }

    // sert à faire Profil utilisateur + liste des favoris
    public class MemberProfilView : Member
    {
        public IEnumerable<Game> FavoriteList { get; set; }
    }

}

