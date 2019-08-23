/// <reference path="builder.js" />

Vvveb.ComponentsGroup["Element UI"] = ['el-container'];

Vvveb.Components.extend('_base', 'el-container', {
    classes: ["el-container"],
    image: "icons/container.svg",
    html: '<section class="el-container" style="min-height:150px;">el-container</section>',
    name: 'el-container',
    properties: [
        {
            name: "Type",
            key: "type",
            htmlAttr: "style",
            sort: base_sort++,
            section: style_section,
            col: 6,
            inline: true,
            inputtype: SelectInput,
            validValues: ["el-container", "el-header", "el-aside", "el-main"],
            data: {
                options: [{
                    value: "el-container",
                    text: "el-container"
                }, {
                    value: "el-header",
                    text: "el-header"
                }, {
                    value: "el-aside",
                    text: "el-aside"
                }, {
                    value: "el-main",
                    text: "el-main"
                }, {
                    value: "el-footer",
                    text: "el-footer"
                }]
            }
        }
    ]
});