using Suckdex.Domain.FileSystem;

namespace Suckdex.Profiles.HaloCEA.FileSystem.Files
{

  public class CEACacheBlockFileNode : CEAFileNode
  {

    #region Constructor

    public CEACacheBlockFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

    #endregion

  }

}
