var Releases = {
    
    settings: {
        uri: '/releases'
    },

    templates: {
        releases: '#releases-template',
        releaseDetails: '#release-details-template'
    },

    getView: function(opts) {
        var theTemplateScript = $(opts.template).html(),
            compiledTemplate = Handlebars.compile (theTemplateScript);
        return compiledTemplate (opts.data);
    },

    getRelease: function(uri) {
        $.getJSON(uri, function(d) {
            $('div[data-uri="' + uri + '"]').empty().html(
                Releases.getView({ template: Releases.templates.releaseDetails, data: d })
            )
        });
    },

    init: function() {

        function bindReleases() {
            $.getJSON(Releases.settings.uri, function(d){
                $('#releases').empty().html(
                    Releases.getView({ template: Releases.templates.releases, data: d })
                ).children('div[data-uri]').each(function(i, o) {
                    Releases.getRelease($(o).attr('data-uri'));
                });
            });
        }

        function createRelease(e) {
            var form = $(e.currentTarget);
            function getData() {
                return {
                    'title': form.find('#title').val(),
                    'createdBy': form.find('#createdBy').val(),
                    'comments': form.find('#comments').val(),
                    'issues': [],
                    'application': form.find('#application').val(),
                    'version': form.find('#version').val(),
                    'deploymentDate': '2016-05-24T12:00:00'
                };
            }

            e.preventDefault();
            $.post(Releases.settings.uri, getData(), bindReleases);
        }

        bindReleases();
        $('#createRelease').on('submit', createRelease);
    }
};