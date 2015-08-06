define([
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/string",
    "dojo/when",

    "dojo/aspect",

    // epi shell
    "epi/shell/dgrid/util/misc",

    // epi-cms
    "epi-cms/contentediting/ContentActionSupport",
    "epi-cms/dgrid/formatters",
    "epi-cms/widget/_GridWidgetBase"
],

function (
    declare,
    lang,
    dojoString,
    when,

    aspect,

    // epi shell
    misc,

    // epi-cms
    ContentActionSupport,
    formatters,
    _GridWidgetBase
) {

    // TODO: Is there a better generic grid we can use and just mix in the formatter?
    return declare([_GridWidgetBase], {
        queryName: null,

        queryParameters: null,

        dndTypes: ['epi.cms.contentreference'],

        postMixInProperties: function () {
            // summary:
            //
            // tags:
            //    protected
            this.storeKeyName = "epi.cms.content.light";

            this.inherited(arguments);
        },

        buildRendering: function () {
            // summary:
            //		Construct the UI for this widget with this.domNode initialized as a dgrid.
            // tags:
            //		protected

            this.inherited(arguments);

            var gridSettings = lang.mixin({
                columns: {
                    name: {
                        renderCell: lang.hitch(this, this.catalogItemFormatter)
                    }
                },
                store: this.store,
                dndSourceType: this.dndTypes
            }, this.defaultGridMixin);

            this.grid = new this._gridClass(gridSettings, this.domNode);

            this.grid.set("showHeader", false);
        },

        catalogItemFormatter: function (item, value, node, options) {
            // summary:
            //      Formatter for catalog list to display both thumbnail and icon type identifier.
            // tags:
            //      public

            var text = misc.htmlEncode(item.name);
            var title = misc.attributeEncode(this.getTitleSelector(item) || text);
            var returnValue = dojoString.substitute("${thumbnail} ${icon} ${text}", {
                thumbnail: formatters.thumbnail(this.getThumbnailSelector(item)),
                icon: formatters.contentIcon(item.typeIdentifier),
                text: misc.ellipsis(text, title)
            });

            node.innerHTML = returnValue;
            return returnValue;
        },

        getThumbnailSelector: function (item) {
            // summary:
            //      Get thumbnail url from content item.
            // tags:
            //      public

            if (item && item.properties) {
                return item.properties.thumbnail;
            }
            return '';
        },

        getTitleSelector: function (item) {
            // summary:
            //      Get title information from content item.
            // tags:
            //      public

            if (item && item.properties) {
                return item.properties.customToolTip;
            }
            return '';
        },

        _onChangeContext: function (e) {
            this.inherited(arguments);
            this.fetchData();
        },

        fetchData: function () {

            when(this._getCurrentItem(), lang.hitch(this, function (currentItem) {
                this.set("currentItem", currentItem);
            }));

            when(this.getCurrentContext(), lang.hitch(this, function (context) {
                this.grid.set("queryOptions", { ignore: ["query", "referenceId"], sort: [{ attribute: "name", descending: true }] });
                var queryParameters = this.queryParameters || {};
                queryParameters.query = this.queryName;
                queryParameters.referenceId = context.id;
                this.grid.set("query", queryParameters);
            }));
        },

        _setQueryNameAttr: function (value) {
            this.queryName = value;
            this.fetchData();
        }
    });
});