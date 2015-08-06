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
    /// Gets the parent packages of an entry.
    /// </summary>
    [ServiceConfiguration(typeof(IContentQuery))]
    public class GetParentPackagesQuery : CatalogRelationsQueryBase<EntryContentBase>
    {
        private readonly IRelationRepository _relationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetParentPackagesQuery"/> class.
        /// </summary>
        /// <param name="queryHelper">Helper used for filter, sort and range.</param>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="relationRepository">The relation repository.</param>
        public GetParentPackagesQuery(
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
            get { return "parentpackages"; }
        }

        /// <summary>
        /// Gets parent packages.
        /// </summary>
        /// <param name="referenceContent">The content to get parent packages for.</param>
        /// <returns>The parent packages.</returns>
        protected override IEnumerable<ContentReference> GetRelatedContent(EntryContentBase referenceContent)
        {
            return referenceContent.GetParentPackages(_relationRepository);
        }
    }
}