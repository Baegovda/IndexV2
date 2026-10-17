using Suckdex.Domain.Database.Entities;

namespace Suckdex.Domain.Database.Repositories
{

  public interface IGamePathRepository : IRepository<GamePath>
  {

    GamePath? GetByPath( string gamePath );
    bool CheckIfGamePathAlreadyExists( string gamePath );

  }

}
