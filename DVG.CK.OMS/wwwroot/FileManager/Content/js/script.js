$(document).ready(function () {
    fm.init({
        treeSelector: "#tree-folder",
        folderExplore: "#explore .folder-explore",
        fileExplore: "#explore .file-explore",
        folderItem: ".jstree-node",
        fileItem: "#explore .file-explore .file-item",
        fileItemClassSelected: "file-item-selected",
        uploadChoose: "input[type='file']",
        uploadButton: "#btnSubmit",
        statusSelector: ".status",
        loading: "#loading",
        loadFileAndFolderOption: {            
            urlLoadFileAndFolder: "/api/LoadFile",
            pathParameter: "path"
        },
        uploadFileOption: {
            urlUploadFile: $("#hidUrlUploadFile").val(),
            folderParameter: "fo",
            waterMark: "false",
            uploadProject: $("#hidUploadProject").val(),
            uploadType: "upload"
        },
        fileActionOption: {
            urlFileAction: "/FileManager/Handler/FileAction.ashx",
            actionParameter: "action",
            fileParameter: "filename",
            folderParameter: "fo"
        },
        uploadFileCondition: {
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png|zip|rar|doc|docx|xls|srt)$/i,
            maxFileSize: 6, //MB
            limitUpload: 100
        },
        folderDefault: "",
        folderSeparator: "__-_-",
        createFileItem: function (file) {
            var html = "";
            var src = "/FileManager/content/images/no-image.png";

            if (/(\.|\/)(gif|jpe?g|png)$/i.exec(file.FullOriginalPath) != null) {
                src = file.FullOriginalPath;
            } else if (/(\.|\/)(srt)$/i.exec(file.FullOriginalPath) != null) {
                src = "/FileManager/content/images/subtitle.png";
            }

            html += "<div class = 'file-item' title = '" + file.Name + "' data-name = '" + file.Name + "' data-path = '" + file.Path + "' data-full-path = '" + file.FullPath + "' data-full-original-path = '" + file.FullOriginalPath + "' data-size = '" + file.Size + "' data-extension = '" + file.Extension + "'>";
            html += "<img src = '" + src + "' />";
            html += "<p>" + file.Name + "</p>";
            html += "</div>";

            return html;
        }
    });
});