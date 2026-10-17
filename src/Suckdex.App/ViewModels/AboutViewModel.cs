using System.Reflection;
using Suckdex.Common;
using Suckdex.UI.ViewModels;
using Prism.Ioc;

namespace Suckdex.App.ViewModels
{

  public class AboutViewModel : DialogWindowViewModel
  {

    public string VersionString
    {
      get
      {
        return GetBuildVersion();
      }
    }

    public AboutViewModel( IContainerProvider container )
      : base( container )
    {
      Title = "About";
    }

    private static string GetBuildVersion()
    {
      var assembly = Assembly.GetExecutingAssembly();
      return AssemblyHelpers.GetBuildString( assembly );
    }

  }

}
