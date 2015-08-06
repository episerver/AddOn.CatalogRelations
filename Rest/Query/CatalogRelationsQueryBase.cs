using System.Collections.Generic;
using AddOn.CatalogRelations.Components;
using EPiServer;
using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Core;
using EPiServer.Framework;

namespace AddOn.CatalogRelations.Rest.Query
{
    /// <summary>
    /// Abstract base class for queries used in the Catalog Relations Gadget.
    /// </summary>
    public abstract class CatalogRelationsQueryBase<T> : ContentQueryBase where T: IContent
    {
        private static readonly string[] DefaultPlugInAreas = { CatalogRelationsComponent.CatalogRelationsQueryPlugInArea};
        private readonly IContentRepository _contentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogRelationsQueryBase{T}"/> class.
        /// </summary>
        /// <param name="queryHelper">The query helper.</param>
        /// <param name="contentRepository">The content repository.</param>
        protected CatalogRelationsQueryBase(
            IContentRepository contentRepository,
            IContentQueryHelper queryHelper)
            : base(contentRepository, queryHelper)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Returns <see cref="CatalogRelationsComponent.CatalogRelationsQueryPlugInArea"/>.
        /// </summary>
        public override IEnumerable<string> PlugInAreas
        {
            get { return DefaultPlugInAreas; }
        }

        /// <summary>
        /// Gets links to the related content which should be returned by the query.
        /// </summary>
        /// <param name="referenceContent">The content to get related content references for.</param>
        /// <returns><see cref="ContentReference"/>s to the related content.</returns>
        protected abstract IEnumerable<ContentReference> GetRelatedContent(T referenceContent);

        /// <summary>
        /// Gets related content.
        /// </summary>
        /// <param name="parameters">Parameters to the query, containing the ReferenceId to get related content for.</param>
        /// <returns>The related content.</returns>
        protected override IEnumerable<IContent> GetContent(ContentQueryParameters parameters)
        {
            Validator.ThrowIfNull("parameters", parameters);

            var referenceId = parameters.ReferenceId;

            if (ContentReference.IsNullOrEmpty(referenceId))
            {
                return null;
            }

            T referenceContent;

            if (!_contentRepository.TryGet(referenceId, out referenceContent))
            {
                return null;
            }

            var links = GetRelatedContent(referenceContent);
            return _contentRepository.GetItems(links, new LoaderOptions {LanguageLoaderOption.FallbackWithMaster()});
        }
    }
}