using System.Collections.Generic;
using System.Linq;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using EPiServer.Shell.ViewComposition;

namespace AddOn.CatalogRelations.Components
{
    /// <summary>
    /// Component showing variants of a product, parent products for a variant etc.
    /// </summary>
    [Component]
    public class CatalogRelationsComponent : ComponentDefinitionBase
    {
        /// <summary>
        /// The plugin area to use for queries that show up in the catalog relations gadget.
        /// </summary>
        public static readonly string CatalogRelationsQueryPlugInArea = "relatedcontent";

        private readonly IEnumerable<IContentQuery> _queries;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogRelationsComponent"/> class.
        /// </summary>
        public CatalogRelationsComponent()
            : this(ServiceLocator.Current.GetAllInstances<IContentQuery>())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogRelationsComponent"/> class.
        /// </summary>
        /// <param name="queries">The queries.</param>
        public CatalogRelationsComponent(IEnumerable<IContentQuery> queries)
            : base("addon/component/CatalogRelations")
        {
            LanguagePath = "/addon/catalogrelations";
            Categories = new[] { "commerce" };
            _queries = queries;
        }

        /// <summary>
        /// Creates the component.
        /// </summary>
        /// <returns>The component</returns>
        public override IComponent CreateComponent()
        {
            var component = base.CreateComponent();

            var querySettings = _queries.Where(q => q.PlugInAreas != null && q.PlugInAreas.Contains(CatalogRelationsQueryPlugInArea))
                .GroupBy(q => q.Name)
                .Select(g => g.OrderByDescending(q => q.Rank).First())
                .OrderBy(q => q.SortOrder)
                .Select(q => new { q.Name });

            component.Settings.Add(new Setting("queries", querySettings.ToList(), false));
            return component;
        }
    }
}

