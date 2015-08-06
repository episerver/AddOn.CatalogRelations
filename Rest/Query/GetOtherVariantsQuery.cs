using System.Collections.Generic;
using System.Linq;
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
    /// Gets the siblings variants of a variant, i.e. other variants of a variant's parent product.
    /// </summary>
    [ServiceConfiguration(typeof(IContentQuery))]
    public class GetOtherVariantsQuery : CatalogRelationsQueryBase<EntryContentBase>
    {
        private readonly IContentRepository _contentRepository;
        private readonly IRelationRepository _relationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOtherVariantsQuery"/> class.
        /// </summary>
        /// <param name="queryHelper">Helper used for filter, sort and range.</param>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="relationRepository">The relation repository.</param>
        public GetOtherVariantsQuery(
            IContentQueryHelper queryHelper,
            IContentRepository contentRepository,
            IRelationRepository relationRepository)
            : base(contentRepository, queryHelper)
        {
            _contentRepository = contentRepository;
            _relationRepository = relationRepository;
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return "othervariants"; }
        }

        /// <summary>
        /// Gets the content of the related.
        /// </summary>
        /// <param name="referenceContent">Content of the reference.</param>
        /// <returns></returns>
        protected override IEnumerable<ContentReference> GetRelatedContent(EntryContentBase referenceContent)
        {
            var parentProducts = referenceContent.GetParentProducts();
            return
                parentProducts.SelectMany(_relationRepository.GetRelationsBySource<ProductVariation>)
                    .Select(r => r.Target)
                    .Where(l => !l.CompareToIgnoreWorkID(referenceContent.ContentLink))
                    .Distinct();
        }
    }
}