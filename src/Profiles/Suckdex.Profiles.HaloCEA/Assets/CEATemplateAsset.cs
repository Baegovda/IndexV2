using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Meshes;

namespace Suckdex.Profiles.HaloCEA.Assets
{

  public class CEATemplateAsset : MeshAsset
  {

    #region Properties

    public override string TypeName => "Template (Model)";

    #endregion

    #region Constructor

    public CEATemplateAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

}
