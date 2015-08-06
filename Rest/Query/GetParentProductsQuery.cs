using System.Collections.Generic;
using EPiServer;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;

namespace AddOn.CatalogRelations.Rest.Query
{
    /// <summary>
    /// Gets the parent products of an entry.
    /// </summary>
    [ServiceConfiguration(typeof(IContentQuery))]
    public class GetParentProductsQuery : CatalogRelationsQueryBase<EntryContentBase>
    {
        private readonly IRelationRepository _relationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetParentProductsQuery"/> class.
        /// </summary>
        /// <param name="queryHelper">Helper used for filter, sort and range.</param>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="relationRepository">The relation repository.</param>
        public GetParentProductsQuery(
            IContentQueryHelper queryHelper,
            IContentRepository contentRepository,
            IRelationRepository relationRepository)
            : base(contentRepository, queryHelper)
        {
            _relationRepository = relationRepository;
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return "parentproducts"; }
        }

        /// <summary>
        /// Gets the parent products.
        /// </summary>
        /// <param name="referenceContent">The content to get parent products for.</param>
        /// <returns>The parent products.</returns>
        protected override IEnumerable<ContentReference> GetRelatedContent(EntryContentBase referenceContent)
        {
            return referenceContent.GetParentProducts(_relationRepository);
        }
    }
}