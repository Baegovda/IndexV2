using Suckdex.Domain.Database.Entities;

namespace Suckdex.Domain.Database.Repositories
{

  public interface ISavedSettingsRepository : IRepository<SavedSettings>
  {

    SavedSettings GetByKey( string key );
    void AddOrUpdate( SavedSettings savedSettings );
    SavedSettings New( string key );

  }

}
