var Releases = {
    init: function() {

        function bindView(opts) {
            var theTemplateScript = $(opts.template).html(),
                compiledTemplate = Handlebars.compile (theTemplateScript);
            $(opts.target).append (compiledTemplate (opts.data));
        }

        $.getJSON('/releases', function(d){
            bindView({ template: '#releases-template', target: 'body', data: d });
        });
    }
};