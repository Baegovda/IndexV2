using Suckdex.Modules.JobManager.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Suckdex.Modules.JobManager
{

  public class JobManagerModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<JobsView>();
    }

  }

}
