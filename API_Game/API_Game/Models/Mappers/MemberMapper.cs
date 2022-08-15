using Demo_ASP_MVC_Modele.BLL.Entities;
using Demo_ASP_MVC_Modele.BLL.Interfaces;

namespace Demo_ASP_MVC_Modele.WebApp.Models.Mappers
{
    public static class MemberMapper
    {

        // BLL -> ViewModel
        public static IEnumerable<Member> ToViewModel(this IEnumerable<MemberModel> datas)
        {
            foreach (MemberModel data in datas)
            {
                yield return data.ToViewModel();
            }

            
        }

        // BLL -> ViewModel
        public static Member ToViewModel(this MemberModel data)
        {
            return new Member
            {
                Id = data.Id,
                Pseudo = data.Pseudo,
                Email = data.Email,
                Pwd = data.Pwd,
                
            };
        }

        // Form -> BLL
        public static MemberModel ToModel(this MemberForm form)
        {
            return new MemberModel()
            {
                Pseudo = form.Pseudo,
                Email = form.Email,
                Pwd = form.Pwd
                
            };
        }

        public static MemberModel ToModel(this MemberFormLogin form)
        {
            return new MemberModel()
            {
                Pseudo = form.Pseudo,
                
                Pwd = form.Pwd

            };
        }

        public static MemberProfilView ToView(this MemberModel m, IGameService gameService)
        {
            return new MemberProfilView
            {
                Id = m.Id,
                Email = m.Email,
                Pseudo = m.Pseudo,
                IsAdmin = m.IsAdmin,
                FavoriteList = gameService.GetByMemberId(m.Id).Select(x => x.ToViewModel())
                
            };
        }
        public static Member ToWeb(this MemberModel m)
        {
            return new Member
            {
                Id = m.Id,
                Email = m.Email,
                Pseudo = m.Pseudo,
                IsAdmin = m.IsAdmin
                
            };
        }
    }
}
