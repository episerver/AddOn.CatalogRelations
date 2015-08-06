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
    /// Gets the variants of a product.
    /// </summary>
    [ServiceConfiguration(typeof(IContentQuery))]
    public class GetVariantsQuery : CatalogRelationsQueryBase<ProductContent>
    {
        private readonly IRelationRepository _relationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetVariantsQuery"/> class.
        /// </summary>
        /// <param name="queryHelper">Helper used for filter, sort and range.</param>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="relationRepository">The relation repository.</param>
        public GetVariantsQuery(
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
            get { return "variants"; }
        }

        /// <summary>
        /// Gets variants of a product.
        /// </summary>
        /// <param name="referenceContent">The product to get variants for.</param>
        /// <returns>The product variants.</returns>
        protected override IEnumerable<ContentReference> GetRelatedContent(ProductContent referenceContent)
        {
            return referenceContent.GetVariants(_relationRepository);
        }
    }
}