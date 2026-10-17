using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Suckdex.Domain.Assets;
using Suckdex.Domain.Assets.Meshes;
using Suckdex.Jobs;
using Suckdex.Profiles.Halo2A.Jobs;
using Prism.Ioc;

namespace Suckdex.Profiles.Halo2A.Assets
{

  public class H2ATemplateAsset : MeshAsset
  {

    #region Properties

    public override string TypeName => "Template (Model)";

    #endregion

    #region Constructor

    public H2ATemplateAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

  public class H2ATemplateAssetFactory : AssetFactoryBase<H2ATemplateAsset>
  {

    #region Constructor

    public H2ATemplateAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<H2ATemplateAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );
      parameters.Set( assetLoadContext );

      return JobManager.StartJob<LoadTemplateJob>( parameters );
    }

    #endregion

  }

}
