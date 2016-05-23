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

        var form = $('#createRelease');
        form.on('submit', function (e) {
            e.preventDefault();
            var data = {
                'title': form.find('#title').val(),
                'createdBy': form.find('#createdBy').val(),
                'comments': form.find('#comments').val(),
                'issues': [],
                'application': form.find('#application').val(),
                'version': form.find('#version').val(),
                'deploymentDate': null
            };
            console.log(data);
            $.ajax({
                url: Releases.settings.uri,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: data,
                success: function(d) {
                    console.log(d);
                    alert('success');
                },
                error: function() {
                    alert('error');
                }
            });
        });
    }
};