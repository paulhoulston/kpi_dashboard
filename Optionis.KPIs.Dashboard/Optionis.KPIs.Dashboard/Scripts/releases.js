var Releases = {
    
    settings: {
        dateFormat: 'dd/mm/yy',
        releasesUri: '/releases',
        usersUri: '/users'
    },

    templates: {
        createRelease: '#create-release-template',
        createUser: '#create-user-template',
        deploymentDetails: '#deployment-template',
        errors: '#errors-template',
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

    handlePostError: function(errorsEl, jqXHR) {
        if(jqXHR && jqXHR.responseJSON && jqXHR.responseJSON.error && jqXHR.responseJSON.error.message) {
            errorsEl.removeClass('hidden').empty().html(
                Releases.getView({
                    template: Releases.templates.errors,
                    data: { errors: [{ error: jqXHR.responseJSON.error.message }] }
                }));
        }
    },

    init: function() {

        function bindReleases() {
            $.getJSON(Releases.settings.releasesUri, function(d){
                $('#releases').empty().html(
                    Releases.getView({ template: Releases.templates.releases, data: d })
                ).children('div[data-uri]').each(function(_, o) {
                    Releases.getRelease($(o).attr('data-uri'));
                });
            });
        }

        function closeDialog() {
            $('#popup').dialog('close');
        }

        function onCreateRelease() {
            function createRelease(e) {
                var form = $('#divReleaseToCreate');

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

                function onSuccess(data) {
                    closeDialog();
                    bindReleases();
                }

                $.ajax({
                    url: Releases.settings.releasesUri,
                    type: 'POST',
                    data: getData(),
                    success: onSuccess,
                    error: function(jqXHR, _, __) { Releases.handlePostError(form.find('#errors'), jqXHR); }
                });
            }

            function tomorrow() {
                var today = new Date();
                var tomorrow = new Date();
                tomorrow.setDate(today.getDate() + 1);
                return tomorrow;
            }

            $('#popup').empty().html(Releases.getView({
                template: Releases.templates.createRelease
            })).dialog({
                width: 650,
                height: 400,
                position: { my: 'center', at: 'center', of: window },
                title: 'Create Release',
                closeOnEscape: true,
                buttons: [
                    { text: 'Create', click: createRelease },
                    { text: 'Cancel', click: closeDialog }
                ]
            }).find('#deploymentDate').datepicker({
                showOn: 'both',
                setDate: tomorrow,
                dateFormat: Releases.settings.dateFormat
            }).datepicker('setDate', tomorrow());
        }

        function onCreateUser() {
            var popup = $('#popup');

            function onSuccess () {
                popup.empty().html($('<p/>', { 'text': 'User added successfully' })).dialog('option', 'buttons', [
                    { 'text': 'Close', click: closeDialog }
                ]);
            }

            function createUser() {
                $.ajax({
                    url: Releases.settings.usersUri,
                    type: 'POST',
                    data: { username: $('#username').val() },
                    success: onSuccess,
                    error: function(jqXHR, _, __) { 
                        Releases.handlePostError(popup.find('#errors'), jqXHR);
                    }
                });
            }

            popup.empty().html(Releases.getView({
                template: Releases.templates.createUser
            })).dialog({
                width: 500,
                height: 250,
                position: { my: 'center', at: 'center', of: window },
                title: 'Create User',
                closeOnEscape: true,
                buttons: [
                    { text: 'Create', click: createUser },
                    { text: 'Cancel', click: closeDialog }
                ]
            });
        }

        bindReleases();

        $('#btnCreateRelease').on('click', onCreateRelease);
        $('#btnCreateUser').on('click', onCreateUser);

        // Format the date
        Handlebars.registerHelper("formatDate", function(datetime, format) {
            return new Date(datetime).toLocaleString();
        });
    }
};