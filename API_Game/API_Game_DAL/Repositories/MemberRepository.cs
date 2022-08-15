using Demo_ASP_MVC_Modele.DAL.Entities;
using Demo_ASP_MVC_Modele.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_ASP_MVC_Modele.DAL.Repositories
{
    public class MemberRepository : RepositoryBase<int, MemberEntity>, IMemberRepository
    {
        public MemberRepository(IDbConnection connection)
          : base(connection, "Member", "Id") // crée le construsteur de la classe parent
        {
        }

        protected override MemberEntity Convert(IDataRecord record)
        {
            return new MemberEntity
            {
                Id = (int)record["Id"],
                Pseudo = (string)record["Pseudo"],
                Email = (string)record["Email"],
                PwdHash = null,
                IsAdmin = (bool)record["IsAdmin"]
            };
        }
        public override int Insert(MemberEntity entity)
        {
            // Créer la commande
            using (IDbCommand cmd = _Connection.CreateCommand())
            {

         

                // On défini la requete SQL
                cmd.CommandText = "INSERT INTO [Member]([Pseudo], [Email], [Pwd_Hash], [IsAdmin])" +
                    " OUTPUT inserted.[Id]" +
                    " VALUES (@Pseudo, @Email, @PwdHash, @IsAdmin)";

            // On ajoute les parametres SQL
            AddParameter(cmd, "@Pseudo", entity.Pseudo);
            AddParameter(cmd, "@Email", entity.Email);
            AddParameter(cmd, "@PwdHash", entity.PwdHash);
                AddParameter(cmd, "@IsAdmin", entity.IsAdmin);


                _Connection.Open();
            int id = (int)cmd.ExecuteScalar();
            _Connection.Close();

            return id;  
            }
        }

        public override bool Update(MemberEntity entity)
        {
            throw new NotImplementedException();
        }

        public MemberEntity GetByPseudo(string pseudo)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT * FROM {TableName} WHERE pseudo = @pseudo";

                AddParameter(cmd, "@pseudo", pseudo);


                _Connection.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return Convert(reader);
                    return null;
                }
            }
        }

        public string GetHashByPseudo(string pseudo)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT Pwd_Hash FROM {TableName} WHERE pseudo = @pseudo";

                AddParameter(cmd, "@pseudo", pseudo);

                _Connection.Open();

                object result = cmd.ExecuteScalar();
                _Connection.Close();
                return result is DBNull ? null : (string)result;

            }
        }


        // Méthodes pour la gestion des favoris
        public override IEnumerable<MemberEntity> GetByMemberId(int id)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "Select * FROM Game G Join Favorite F on F.IdGame = G.Id " +
                    "Where F.IdMember = @id";
                AddParameter(cmd, "id", id);
                _Connection.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return Convert(reader);
                    }
                }
            }
        }


        public override bool AddFavoriteGame(int memberId, int gameId)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "Insert into Favorite (IdMember, IdGame) Values (@mid, @gid)";
                AddParameter(cmd, "mid", memberId);
                AddParameter(cmd, "gid", gameId);
                _Connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool SetAdmin(int Id)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE Member SET IsAdmin = 1 WHERE Id = @Id";
                AddParameter(cmd, "@Id", Id);

                ConnectionOpen();
                return cmd.ExecuteNonQuery() == 1;

            }
        }
    }
}
