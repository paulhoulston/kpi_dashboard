﻿var Releases = {
    
    settings: {
        dateFormat: 'dd/mm/yy',
        uri: '/releases'
    },

    templates: {
        createRelease: '#create-release-template',
        deploymentDetails: '#deployment-template',
        issueDetails: '#issue-template',
        releases: '#releases-template',
        releaseDetails: '#release-details-template'
    },

    getView: function(opts) {
        var theTemplateScript = $(opts.template).html(),
            compiledTemplate = Handlebars.compile (theTemplateScript);
        return compiledTemplate (opts.data || { });
    },

    getIssue: function(uri) {
        $.getJSON(uri, function(d) {
            $('div[data-issue-uri="' + uri + '"]').empty().html(
                Releases.getView({ template: Releases.templates.issueDetails, data: d })
            );
        });
    },


    getDeployment: function(uri) {
        $.getJSON(uri, function(d) {
            $('div[data-deployment-uri="' + uri + '"]').empty().html(
                Releases.getView({ template: Releases.templates.deploymentDetails, data: d })
            );
        });
    },

    getRelease: function(uri) {
        $.getJSON(uri, function(d) {
            var releaseDiv = 
                $('div[data-uri="' + uri + '"]').empty().html(
                    Releases.getView({ template: Releases.templates.releaseDetails, data: d })
                );
            releaseDiv.find('div[data-issue-uri]').each(function(_, o) {
                Releases.getIssue($(o).attr('data-issue-uri'));
            });
            releaseDiv.find('div[data-deployment-uri]').each(function(_, o) {
                Releases.getDeployment($(o).attr('data-deployment-uri'));
            });
        });
    },

    init: function() {

        function bindReleases() {
            $.getJSON(Releases.settings.uri, function(d){
                $('#releases').empty().html(
                    Releases.getView({ template: Releases.templates.releases, data: d })
                ).children('div[data-uri]').each(function(_, o) {
                    Releases.getRelease($(o).attr('data-uri'));
                });
            });
        }

        function onCreateRelease() {

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
                        'deploymentDate': form.find('#deploymentDate').datepicker('getDate')
                    };
                }

                function onSuccess() {
                    $('#popup').dialog('close');
                    bindReleases();
                }

                e.preventDefault();
                $.post(Releases.settings.uri, getData(), onSuccess);
            }

            function getView() {
                return Releases.getView({ template: Releases.templates.createRelease });
            }

            function tomorrow() {
                var today = new Date();
                var tomorrow = new Date();
                tomorrow.setDate(today.getDate() + 1);
                return tomorrow;
            }

            var form = $('#popup').empty().html(getView()).dialog({
                width: 650,
                height: 400,
                position: { my: 'center', at: 'center', of: window },
                title: 'Create Release'
            }).find('form');

            form.find('#deploymentDate').datepicker({
                showOn: 'both',
                setDate: tomorrow,
                dateFormat: Releases.settings.dateFormat
            }).datepicker('setDate', tomorrow());
            form.on('submit', createRelease  );
        }

        bindReleases();

        $('#btnCreateRelease').on('click', onCreateRelease);

        // Format the date
        Handlebars.registerHelper("formatDate", function(datetime, format) {
            return new Date(datetime).toLocaleString();
        });
    }
};