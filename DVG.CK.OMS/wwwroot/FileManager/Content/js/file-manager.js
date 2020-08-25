(function (window, $) {
    if (window.fm) {
        return;
    }

    var fm = {
        version: "1.0.0",
        tree: null,
        data: null,
        action: {
            name: "",
            file: "",
            folder: ""
        },
        option: {
            treeSelector: "",
            folderExplore: "",
            fileExplore: "",
            fileItem: "",
            folderItem: "",
            fileItemClassSelected: "",
            uploadChoose: "",
            uploadButton: "",
            statusSelector: "",
            loading: "",
            loadFileAndFolderOption: {
                urlLoadFileAndFolder: "",
                pathParameter: ""
            },
            uploadFileOption: {
                urlUploadFile: "",
                folderParameter: "",
                waterMark: "",
                uploadProject: "",
                uploadType: ""
            },
            fileActionOption: {
                urlFileAction: "",
                actionParameter: "",
                fileParameter: "",
                folderParameter: ""
            },
            uploadFileCondition: {
                acceptFileTypes: "",
                maxFileSize: 0, //MB
                limitUpload: 0
            },
            folderDefault: "",
            folderSeparator: "",
            createFileItem: null
        }
    };
    var files, selectedFolder = "";

    fm.init = function (option) {
        for (var i in fm.option) {
            if (!option[i]) {
                continue;
            }

            fm.option[i] = option[i];
        }

        fm.tree = $(fm.option.treeSelector).jstree(
        {
            core:
                {
                    check_callback: true
                },
            plugins: ["unique", "dnd"]
        });

        if (!fm.option.folderDefault) {
        	var homnay = new Date();
        	var strhomnay = homnay.toISOString().substring(0, 10).replace(/-/g, "/");
        	fm.option.folderDefault = strhomnay;
        }

        selectedFolder = fm.option.folderDefault;

        fm.tree.jstree(true).create_node(fm.tree, { id: fm.option.folderDefault, text: fm.option.folderDefault, state: { opened: true, selected: true } });

        fm.tree.on("changed.jstree", function (e, data) {
            var selected = data.selected[0];
            var path = data.instance.get_node(selected).id;
            selectedFolder = path;

            fm.load(selected, path);
        });

        $(fm.option.uploadButton).click(function () {

            if (document.getElementById("fileUpload").value == "")
            {
                alert("Vui lòng chọn file!");
                return false;
            }

            //Vì quy định upload theo ngày tháng năm nên set lại folder được chọn thành thư mục gốc
            //Nếu muốn upload theo thư mục được chọn thì bỏ phần này đi
            selectedFolder = fm.option.folderDefault;
            var fileNames = new Array();

            $.each(files, function (key, value) {
                var fileName = value.name;

                if (fm.option.uploadFileCondition.acceptFileTypes.exec(fileName) == null) {
                    alert('Định dạng file không được chấp nhận!\n(' + fileName + ')');

                    flag = false;

                    return false;
                }

                if (value.size > Math.pow(1024, 2) * fm.option.uploadFileCondition.maxFileSize) {
                    alert('Kích thước không cho phép! Chỉ được upload file < ' + fm.option.uploadFileCondition.maxFileSize + 'MB\n(' + fileName + ')');

                    flag = false;

                    return false;
                }

                var data = new FormData();

                data.append('UploadUrl', fm.option.uploadFileOption.urlUploadFile);
                data.append('UploadProject', fm.option.uploadFileOption.uploadProject);
                data.append('UploadType', fm.option.uploadFileOption.uploadType);
                data.append('WaterMark', $("#chkWaterMark").prop("checked"));
                data.append('StringDecypt', $('#hfToken').val());
                data.append('Submit', 'Upload Image');
                data.append('fileToUpload', value);
                
                $.ajax({
                	url: "/Common/FileManagerUpload",
                    type: "POST",
                    data: data,
                    dataType: "json",
                    processData: false, // Don't process the files
                    contentType: false, // Set content type to false as jQuery will tell the server its a query string request
                    async: false,
                    success: function (Msg) {
                        console.log(Msg);
                        if (Msg.Error) {
                            alert("Không thể upload tệp tin: " + fileName);
                        }
                    },
                    error: function () {
                        alert("Error: " + Msg.Title);
                    }
                });
            });

            fm.load(fm.tree.jstree(true).get_node("#" + selectedFolder), selectedFolder);
        });

        $(fm.option.uploadChoose).change(function (e) {
            //files = e.target.files;
            files = this.files;
        });

        $(fm.option.folderExplore).niceScroll({ touchbehavior: false, /*cursorcolor: "#0000FF", */ cursoropacitymax: 0.6, cursorwidth: 5 });
        $(fm.option.fileExplore).niceScroll({ touchbehavior: false, /*cursorcolor: "#0000FF", */ cursoropacitymax: 0.6, cursorwidth: 5 });
        
        //fm.load(fm.tree, fm.option.folderDefault);
        fm.load(fm.tree.jstree(true).get_node("#" + fm.option.folderDefault), fm.option.folderDefault);

        return fm;
    };

    fm.load = function load(node, path) {
        path = path.replaceAll(fm.option.folderSeparator, "/");

        $.ajax({
            type: "POST",
            async: false,
            url: fm.option.loadFileAndFolderOption.urlLoadFileAndFolder + "?" + fm.option.loadFileAndFolderOption.pathParameter + "=" + path,
            //data: "{ " + fm.option.loadFileAndFolderOption.pathParameter + ": '" + path + "' }",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                fm.data = result;// = $.parseJSON(result.d);

                if (result.Result === false) {
                    alert(result.Message);

                    return fm;
                }

                for (var i = 1; i < 60; i++) {
                	var homnay = new Date();
                	var ngay = new Date(homnay.setDate(homnay.getDate() - i));
                	var ngaystring = ngay.toISOString().substring(0, 10).replace(/-/g, "/");
                	fm.tree.jstree(true).create_node(fm.tree, { id: ngaystring, text: ngaystring, state: { opened: false } });
                }

                var html = "";

                for (var i in result.FileInfos) {
                    var file = result.FileInfos[i];

                    html += fm.option.createFileItem(file);
                }

                $(fm.option.fileExplore).html(html);
                $(fm.option.fileItem)
                    .draggable()
                    .click(function () {
                        $(fm.option.fileItem).removeClass(fm.option.fileItemClassSelected);
                        $(this).addClass(fm.option.fileItemClassSelected);
                    })
                    .dblclick(function () {
                        try {
                            var name = $(this).attr("data-name");
                            var file = fm.findFileInfo(name);

                            window.parent.callback(file);
                        } catch (ex) { }
                    });
            },
            error: function () {
                alert("Error");
            }
        });

        return fm;
    };

    fm.fileAction = function (action) {
        var param = "?" + fm.option.fileActionOption.actionParameter + "=" + action.name;
        var file = action.file.replaceAll(fm.option.folderSeparator, "/");;

        param += "&" + fm.option.fileActionOption.fileParameter + "=" + file;

        if (action.name == "cut" || action.name == "copy") {
            var folder = action.folder.replaceAll(fm.option.folderSeparator, "/");

            param += "&" + fm.option.fileActionOption.folderParameter + "=" + folder;
        }

        $.ajax({
            type: "POST",
            async: false,
            url: fm.option.fileActionOption.urlFileAction + param,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (!result.Result) {
                    alert(result.Message);

                    return fm;
                }

                fm.load(fm.tree.jstree(true).get_node("#" + selectedFolder), selectedFolder);
            },
            error: function () {
                alert("Error");
            }
        });

        return fm;
    };

    fm.loading = function (isLoading) {
        var ls = $(fm.option.loading);
        var w = $(window);

        if (!isLoading) {
            ls.hide();

            return fm;
        }

        var top = (w.height() - ls.height()) / 2;
        var left = (w.width() - ls.width()) / 2;

        ls.css("top", top).css("left", left).show();

        return fm;
    };

    fm.findFileInfo = function (fileName) {
        if (!fm.data || !fm.data.FileInfos) {
            return null;
        }

        for (var i in fm.data.FileInfos) {
            var file = fm.data.FileInfos[i];

            if (file.Name.toLowerCase() == fileName.toLowerCase()) {
                return file;
            }
        }

        return null;
    };

    fm.addFileInfo = function (file) {
        if (!fm.data || !fm.data.FileInfos) {
            return;
        }

        fm.data.FileInfos.push(file);
    }

    String.prototype.trim = function (chars) {
        if (chars === undefined)
            chars = "\s";

        return this.replace(new RegExp("^[" + chars + "]+"), "").replace(new RegExp("[" + chars + "]+$"), "");
    };

    String.prototype.replaceAll = function (replaceFrom, replaceTo) {
        var text = this;

        if (!replaceFrom || !replaceTo || replaceFrom == replaceTo) {
            return text;
        }

        while (text.indexOf(replaceFrom) > -1) {
            text = text.replace(replaceFrom, replaceTo);
        }

        return text;
    };

    window.fm = fm;
})(window, $);