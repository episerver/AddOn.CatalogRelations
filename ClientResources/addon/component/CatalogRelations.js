define([
// Dojo
    "dojo",
    "dojo/_base/declare",
    "dojo/_base/array",
    "dojo/_base/lang",
    "dojo/dom-construct",
    "dojo/dom-geometry",

// Dijit
    "dijit/_TemplatedMixin",
    "dijit/_Container",
    "dijit/layout/_LayoutWidget",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/Select",

// Template
    "dojo/text!./templates/CatalogRelations.html",

// Resources
    "epi/i18n!epi/cms/nls/addon.catalogrelations",

// Widgets in template
    "../widget/CatalogRelationsGrid"
], function (
// Dojo
    dojo,
    declare,
    array,
    lang,
    domConstruct,
    domGeometry,

// Dijit
    _TemplatedMixin,
    _Container,
    _LayoutWidget,
    _WidgetsInTemplateMixin,
    Select,

// Template
    template,
// Resources
    resources
) {

    return declare([_Container, _LayoutWidget, _TemplatedMixin, _WidgetsInTemplateMixin], {
        // summary: 
        //      This component will list variants of a product, parent products of a variant etc.
        //
        // tags:
        //      Public

        templateString: template,

        resources: resources,

        querySelection: null,

        postCreate: function () {

            this.inherited(arguments);

            var options = array.map(this.queries, function (item) {
                return {
                    label: this.resources.queries[item.name],
                    value: item.name
                };
            }, this);

            this.querySelection = new Select({
                name: "QuerySelection",
                options: options
            });

            domConstruct.place(this.querySelection.domNode, this.reloadButton.domNode, 'before');

            this.own(
                this.querySelection.on("change", lang.hitch(this, this._reloadQuery))
            );
        },

        startup: function () {
            this.inherited(arguments);

            // Set the initial query after the grid has been initialized
            this._reloadQuery();
        },

        resize: function (newSize) {
            // summary:
            //      Customize default resize method
            // newSize: object
            //      The new size of Task component
            // tags:
            //      Public

            this.inherited(arguments);

            this.contentQuery.resize(this._caculateContentQuerySize(newSize));
        },

        _caculateContentQuerySize: function (newSize) {
            // summary:
            //      Calculate the new Size of the Content Query
            // newSize: object
            //      The new size of Task component
            // tags:
            //      Private

            var toolbarSize = domGeometry.getMarginBox(this.toolbar);

            return { w: newSize.w, h: newSize.h - toolbarSize.h };
        },

        _reloadQuery: function () {
            var query = this.querySelection.get("value");
            this.contentQuery.set("queryName", query);
        }
    });
});
