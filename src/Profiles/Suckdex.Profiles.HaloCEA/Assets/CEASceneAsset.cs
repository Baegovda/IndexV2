using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Meshes;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEASceneAsset : MeshAsset
  {

    #region Properties

    public override string TypeName => "Scene";

    #endregion

    #region Constructor

    public CEASceneAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

}
