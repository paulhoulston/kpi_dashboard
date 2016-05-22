var Releases = {
    
    settings: {
        uri: '/releases'
    },

    templates: {
        releases: '#releases-template',
        releaseDetails: '#release-details-template'
    },

    bindView: function(opts) {
        var theTemplateScript = $(opts.template).html(),
            compiledTemplate = Handlebars.compile (theTemplateScript);
        return $(opts.target).append (compiledTemplate (opts.data));
    },

    getRelease: function(uri) {
        $.getJSON(uri, function(d) {
            Releases.bindView({ 
                template: Releases.templates.releaseDetails, 
                target: 'div[data-uri="' + uri + '"]',
                 data: d
            });
        });
    },

    init: function() {
        $.getJSON(Releases.settings.uri, function(d){
            Releases.bindView({ 
                template: Releases.templates.releases,
                target: '#releases',
                data: d 
            }).children('div[data-uri]').each(function(i, o) {
                Releases.getRelease($(o).attr('data-uri'));
            });
        });
    }
};