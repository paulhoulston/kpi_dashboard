var Releases = {
    
    settings: {
        allReleasesUri: '/releases?top=10000',
        dateFormat: 'dd/mm/yy',
        deploymentStatuses: '/deployments/statuses',
        releasesUri: '/releases',
        usersUri: '/users'
    },

    templates: {
        createRelease: '#create-release-template',
        createUser: '#create-user-template',
        deploymentDetails: '#deployment-template',
        deploymentStatus: '#update-status-template',
        errors: '#errors-template',
        issueDetails: '#issue-template',
        releases: '#releases-template',
        releaseDetails: '#release-details-template'
    },

    formatString: function (str, args) {
        args = typeof args === 'object' ? args : Array.prototype.slice.call(arguments, 1);
        return str.replace(/\{([^}]+)\}/gm, function () {
            return args[arguments[1]];
        });
    },

    closeDialog: function() {
        $('#popup').dialog('close');
    },

    getView: function(opts) {
        var theTemplateScript = $(opts.template).html(),
            compiledTemplate = Handlebars.compile (theTemplateScript);
        return compiledTemplate (opts.data || { });
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

        function bindReleases(releasesUri) {

            function getRelease(uri) {

                function getIssue(uri) {
                    $.getJSON(uri, function(d) {
                        $('div[data-issue-uri="' + uri + '"]').empty().html(
                            Releases.getView({ template: Releases.templates.issueDetails, data: d })
                        );
                    });
                }

                function getDeployment(uri) {
                    function onUpdateStatus(e) {
                        function onGetStatuses(d) {
                            var trg = $(e.currentTarget);
    
                            function getStatusOptions() {
                                var currentStatus = trg.attr('data-status'),
                                    opts = [];
                                $(d.statuses).each(function(_, val) {
                                    opts.push({ value: val, selected: val === currentStatus }); 
                                });
                                return { deploymentUri: uri, options: opts };
                            }

                            function onUpdateDeployent() { 
                                bindReleases();
                            }

                            var trg = $(e.currentTarget),
                                div = trg.parents('div[name="statusDiv"]');
                            
                            div.empty().html(Releases.getView({ 
                                template: Releases.templates.deploymentStatus,
                                data: getStatusOptions()
                            }));

                            div.find('a[name="cancel"]').on('click', bindReleases);
                            div.find('a[name="save"]').on('click', onUpdateDeployent);

                        }

                        $.getJSON(Releases.settings.deploymentStatuses, onGetStatuses);
                    }

                    function onDeleteDeployment(e) {

                        function deleteDeployment() {
                            Releases.closeDialog();
                            $.ajax({ 
                                url: $(e.currentTarget).attr('data-deployment-uri'),
                                type: 'DELETE',
                                success: bindReleases
                            });
                        }

                        $('#popup').empty().html($('<p/>', { 
                            text: 'Are you sure wish to delete the deployment' 
                        })).dialog({
                            title: 'Delete deployment?',
                            buttons: [
                                { text: 'OK', click: deleteDeployment },
                                { text: 'Cancel', click: Releases.closeDialog }
                            ]
                        });
                    }

                    $.getJSON(uri, function(d) {
                        var deployment = $('div[data-deployment-uri="' + uri + '"]').empty().html(
                            Releases.getView({ template: Releases.templates.deploymentDetails, data: d })
                        );
                        deployment.find('a[data-action="update"][data-deployment-uri]').on('click', onUpdateStatus);
                        deployment.find('a[data-action="delete"][data-deployment-uri]').on('click', onDeleteDeployment);
                    });
                }

                $.getJSON(uri, function(d) {
                    var releaseDiv = 
                        $('div[data-uri="' + uri + '"]').empty().html(
                            Releases.getView({ template: Releases.templates.releaseDetails, data: d })
                        );
                    releaseDiv.find('div[data-issue-uri]').each(function(_, o) {
                        getIssue($(o).attr('data-issue-uri'));
                    });
                    releaseDiv.find('div[data-deployment-uri]').each(function(_, o) {
                        getDeployment($(o).attr('data-deployment-uri'));
                    });
                });
            }

            $.getJSON(releasesUri || Releases.settings.releasesUri, function(d){
                $('#releases').empty().html(
                    Releases.getView({ template: Releases.templates.releases, data: d })
                ).children('div[data-uri]').each(function(_, o) {
                    getRelease($(o).attr('data-uri'));
                });

                $('a.showAll').on('click', function() {
                    bindReleases(Releases.settings.allReleasesUri);
                });
            });
        }

        function onCreateRelease() {
            function createRelease(e) {
                var form = $('#divReleaseToCreate');

                function getData() {
                    function toString(date) {
                        return Releases.formatString(
                            '{0}-{1}-{2}T00:00:00',
                            date.getFullYear(),
                            (date.getMonth() < 9 ? '0' : '') + (1 + date.getMonth()),
                            (date.getDate() < 10 ? '0' : '') + date.getDate());
                    }
                    
                    return {
                        'title': form.find('#title').val(),
                        'createdBy': form.find('#createdBy').val(),
                        'comments': form.find('#comments').val(),
                        'issues': [],
                        'application': form.find('#application').val(),
                        'version': form.find('#version').val(),
                        'deploymentDate': toString(form.find('#deploymentDate').datepicker('getDate'))
                    };
                }

                function onSuccess(data) {
                    Releases.closeDialog();
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

            function bindUsers() {
                $.getJSON(Releases.settings.usersUri, function(d) {
                    if(d.users) {
                        for(ind in d.users)
                            $.getJSON(d.users[ind].uri, function(usr) {
                                $('#createdBy').append($('<option/>', { 'value': usr.id, 'text': usr.userName }));
                            });
                    }
                });
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
                    { text: 'Cancel', click: Releases.closeDialog }
                ]
            }).find('#deploymentDate').datepicker({
                showOn: 'both',
                setDate: tomorrow,
                dateFormat: Releases.settings.dateFormat
            }).datepicker('setDate', tomorrow());

            bindUsers();
        }

        function onCreateUser() {
            var popup = $('#popup');

            function onSuccess () {
                popup.empty().html($('<p/>', { 'text': 'User added successfully' })).dialog('option', 'buttons', [
                    { 'text': 'Close', click: Releases.closeDialog }
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
                    { text: 'Cancel', click: Releases.closeDialog }
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