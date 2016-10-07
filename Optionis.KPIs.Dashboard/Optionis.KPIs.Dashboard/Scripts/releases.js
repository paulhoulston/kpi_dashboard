var Releases = {
    
    settings: {
        allReleasesUri: '/releases?top=10000',
        applicationsUri: '/releases/applications',
        baseUrl: '',
        dateFormat: 'dd/mm/yy',
        deploymentStatuses: '/deployments/statuses',
        releasesUri: '/releases',
        usersUri: '/users'
    },

    templates: {
        addDeployment: '#add-deployment-template',
        addIssue: '#add-issue-template',
        createRelease: '#add-release-template',
        createUser: '#create-user-template',
        deploymentDetails: '#deployment-template',
        errors: '#errors-template',
        issueDetails: '#issue-template',
        releases: '#releases-template',
        releaseDetails: '#release-details-template',
        statuses: '#update-status-template'
    },

    formatString: function (str, args) {
        args = typeof args === 'object' ? args : Array.prototype.slice.call(arguments, 1);
        return str.replace(/\{([^}]+)\}/gm, function () {
            return args[arguments[1]];
        });
    },

    datePickerSubmitDate: function(el) {
        function toString(date) {
            return Releases.formatString(
                '{0}-{1}-{2}T00:00:00',
                date.getFullYear(),
                (date.getMonth() < 9 ? '0' : '') + (1 + date.getMonth()),
                (date.getDate() < 10 ? '0' : '') + date.getDate());
        }
        return toString($(el).datepicker('getDate'));
    },

    closeDialog: function() {
        $('#popup').dialog('close');
    },

    getView: function(opts) {
        var theTemplateScript = $(opts.template).html(),
            compiledTemplate = Handlebars.compile(theTemplateScript);
        //console.log('Releases.getView called with data: ' + JSON.stringify(opts.data));
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

    tomorrow: function() {
        var today = new Date();
        var tomorrow = new Date();
        tomorrow.setDate(today.getDate() + 1);
        return tomorrow;
    },

    init: function() {

        function bindReleases(releasesUri) {

            function getRelease(uri) {
                function onGetReleaseDetails(d) {
                    var releaseDiv = $('div[data-uri="' + uri + '"]').empty().html(Releases.getView({ template: Releases.templates.releaseDetails, data: d }));

                    function getIssue(uri) {
                        $.getJSON(Releases.settings.baseUrl + uri, function (d) {
                            $('div[data-issue-uri="' + uri + '"]').empty().html(
                                Releases.getView({ template: Releases.templates.issueDetails, data: d })
                            );
                        });
                    }
                    
                    function getDeployment(uri) {
                        function onDeleteDeployment(e) {
                            function deleteDeployment() {
                                Releases.closeDialog();
                                $.ajax({ 
                                    url: Releases.settings.baseUrl + $(e.currentTarget).attr('data-delete-deployment-uri'),
                                    type: 'DELETE',
                                    success: function () { bindReleases(); }
                                });
                            }

                            $('#popup').empty().html($('<p/>', { 
                                text: 'Are you sure wish to delete the deployment' 
                            })).dialog({
                                title: 'Delete deployment?',
                                width: 350,
                                height: 175,
                                buttons: [
                                    { text: 'OK', click: deleteDeployment },
                                    { text: 'Cancel', click: Releases.closeDialog }
                                ]
                            });
                        }

                        function onChangeStatus(e) {
                            var trg = $(e.currentTarget),
                                deploymentUri = trg.attr('data-deployment-status-uri'),
                                row = trg.parents('.row');

                            function onChangeStatusACtion(ev) {
                                var link = $(ev.currentTarget);

                                function updateStatus() {
                                    $.ajax({
                                        url: Releases.settings.baseUrl + deploymentUri,
                                        type: 'PATCH',
                                        data: {
                                            propertyName: 'status',
                                            propertyValue: row.find('select option:selected').val()
                                        },
                                        success: function () { bindReleases(); }
                                    });
                                }

                                if (link.attr('data-action') === 'cancel') {
                                    bindReleases();
                                } else {
                                    updateStatus();
                                }
                            }

                            $.getJSON(Releases.settings.baseUrl + deploymentUri, function (deployment) {
                                $.getJSON(Releases.settings.baseUrl + Releases.settings.deploymentStatuses, function (statuses) {
                                    row.empty().html(
                                        Releases.getView({
                                            template: Releases.templates.statuses,
                                            data: {
                                                version: deployment.version,
                                                deploymentDate: deployment.deploymentDate,
                                                status: deployment.status,
                                                statuses: statuses.statuses
                                            }
                                        })).find('a[data-action]').on('click', onChangeStatusACtion);
                                });
                            });
                        }

                        $.getJSON(Releases.settings.baseUrl + uri, function (d) {
                            var deployment = $('div[data-deployment-uri="' + uri + '"]').empty().html(
                                Releases.getView({ template: Releases.templates.deploymentDetails, data: d })
                            );
                            deployment.find('a[data-delete-deployment-uri]').on('click', onDeleteDeployment);
                            deployment.find('a[data-deployment-status-uri]').on('click', onChangeStatus);
                        });
                    }

                    function addIssue(e) {
                        var popup = $('#popup');

                        function createIssue() {
                            $.ajax({
                                url: Releases.settings.baseUrl + $(e.currentTarget).attr('data-add-issue-uri'),
                                type: 'POST',
                                data: {
                                    issueId: popup.find('#issueId').val(),
                                    title: popup.find('#title').val(),
                                    link: popup.find('#link').val()
                                },
                                success: function () {
                                    bindReleases();
                                    Releases.closeDialog();
                                },
                                error: function (jqXHR, _, __) {
                                    Releases.handlePostError(popup.find('#errors'), jqXHR);
                                }
                            });
                        }

                        popup.empty().html(Releases.getView({
                            template: Releases.templates.addIssue,
                            data: {
                                uri: $(e.currentTarget).attr('data-add-issue-uri')
                            }
                        })).dialog({
                            width: 500,
                            height: 325,
                            position: { my: 'center', at: 'center', of: window },
                            title: 'Create Issue',
                            closeOnEscape: true,
                            buttons: [
                                { text: 'Create', click: createIssue },
                                { text: 'Cancel', click: Releases.closeDialog }
                            ]
                        });
                    }

                    function addDeployment(e) {
                        var popup = $('#popup');

                        function createDeployment() {
                            $.ajax({
                                url: Releases.settings.baseUrl + $(e.currentTarget).attr('data-add-deployment-uri'),
                                type: 'POST',
                                data: {
                                    deploymentDate: popup.find('#deploymentDate').val(),
                                    version: popup.find('#version').val(),
                                    status: popup.find('#status').val(),
                                    comments: popup.find('#comments').val()
                                },
                                success: function () {
                                    bindReleases();
                                    Releases.closeDialog();
                                },
                                error: function (jqXHR, _, __) {
                                    Releases.handlePostError(popup.find('#errors'), jqXHR);
                                }
                            });
                        }

                        $.getJSON(Releases.settings.baseUrl + Releases.settings.deploymentStatuses, function (statuses) {
                            popup.empty().html(
                            Releases.getView({
                                template: Releases.templates.addDeployment,
                                data: statuses
                            })).dialog({
                                width: 500,
                                height: 375,
                                position: { my: 'center', at: 'center', of: window },
                                title: 'Create Issue',
                                closeOnEscape: true,
                                buttons: [
                                    { text: 'Create', click: createDeployment },
                                    { text: 'Cancel', click: Releases.closeDialog }
                                ]
                            }).find('#deploymentDate').datepicker({
                                showOn: 'both',
                                dateFormat: Releases.settings.dateFormat
                            }).datepicker('setDate', Releases.tomorrow());
                        });
                    }

                    releaseDiv.find('div[data-issue-uri]').each(function(_, o) {
                        getIssue($(o).attr('data-issue-uri'));
                    });
                    releaseDiv.find('div[data-deployment-uri]').each(function(_, o) {
                        getDeployment($(o).attr('data-deployment-uri'));
                    });
                    releaseDiv.find('a[data-add-deployment-uri]').on('click', addDeployment);
                    releaseDiv.find('a[data-add-issue-uri]').on('click', addIssue);
                }

                $.getJSON(Releases.settings.baseUrl + uri, onGetReleaseDetails);
            }

            $.getJSON(Releases.settings.baseUrl + (releasesUri || Releases.settings.releasesUri), function (d) {
                var releasesDiv = $('#releases').empty().html(
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
                    return {
                        'title': form.find('#title').val(),
                        'createdBy': form.find('#createdBy').val(),
                        'comments': form.find('#comments').val(),
                        'issues': [],
                        'application': form.find('#application').val(),
                        'version': form.find('#version').val(),
                        'deploymentDate': Releases.datePickerSubmitDate(form.find('#deploymentDate'))
                    };
                }

                function onSuccess(data) {
                    Releases.closeDialog();
                    bindReleases();
                }

                $.ajax({
                    url: Releases.settings.baseUrl + Releases.settings.releasesUri,
                    type: 'POST',
                    data: getData(),
                    success: onSuccess,
                    error: function (jqXHR, _, __) { Releases.handlePostError(form.find('#errors'), jqXHR); }
                });
            }

            function bindUsers() {
                $.getJSON(Releases.settings.baseUrl + Releases.settings.usersUri, function (d) {
                    if (d.users) {
                        for (ind in d.users)
                            $.getJSON(Releases.settings.baseUrl + d.users[ind], function (usr) {
                                $('#createdBy').append($('<option/>', { 'value': usr.id, 'text': usr.userName }));
                            });
                    }
                });
            }

            $.getJSON(Releases.settings.baseUrl + Releases.settings.applicationsUri, function (applications) {
                $('#popup').empty().html(Releases.getView({
                    template: Releases.templates.createRelease,
                    data: applications
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
                    dateFormat: Releases.settings.dateFormat
                }).datepicker('setDate', Releases.tomorrow());

                bindUsers();
            });
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
                    url: Releases.settings.baseUrl + Releases.settings.usersUri,
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

        function getPath() {
            var index = window.location.pathname.lastIndexOf('/');
            return index >= 0 ?
                window.location.pathname.substring(0, index) :
                window.location.pathname;
        }

        Releases.settings.baseUrl = getPath();
        bindReleases();

        $('#btnCreateRelease').on('click', onCreateRelease);
        $('#btnCreateUser').on('click', onCreateUser);

        Handlebars.registerHelper("formatDate", function (datetime, format) {
            return $.datepicker.formatDate("dd MM yy", new Date(datetime));
        });
    }
};