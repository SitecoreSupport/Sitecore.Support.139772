
namespace Sitecore.Support.Social.IoC.Modules
{
    using ISearchProvider = Sitecore.Social.Search.ISearchProvider;
    using Ninject.Modules;

    public class SupportModule139772 : NinjectModule
    {
        public override void Load()
        {
            this.Rebind<ISearchProvider>().To<Sitecore.Support.Social.Search.SearchProvider>();
        }
    }
}