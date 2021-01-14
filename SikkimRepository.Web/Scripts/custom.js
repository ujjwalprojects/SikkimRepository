$(function () {

    ShowMessageBox();

    //**************** JS to show loading progress during ajax call *********************//
    $(document).ajaxStart(function () {
        $("#ajaxLoading").css("display", "block");
        $("#ajaxLoading").css("top", $(window).height() / 1.8);
        $("#ajaxLoading").css("left", $(window).width() / 2);
        $("#ajaxLoading").css("position", "fixed");
    });
    $(document).ajaxComplete(function () {
        $("#ajaxLoading").css("display", "none");

    });

    //$('#side-menu').metisMenu();

   //******************************** JS for Grid paging *********************************//
    var getPage = function () {
        var $a = $(this);

        if ($a.attr("href").trim() == undefined || $a.attr("href").trim() == "") {
            return;
        }
        var options = {
            url: $a.attr("href")
            , data: $("form").serialize()
            , type: "get"
        }

        $.ajax(options).done(function (data) {
            var $target = $($a.parents("div.ns-grid-pager").attr("data-otf-target"));
            $target.replaceWith(data);
        });
        return false;
    };
    $(".container-fluid").on("click", "a.ns-page-link", getPage);

    var getPageForDDL = function () {
        var TargetURL = $(this).parent().attr("data-otf-actionlink");
        TargetURL = TargetURL + "?PageSize=" + $('.page-size').val() + "&PageNo=" + $(this).val()
        var options = {
            url: TargetURL
            , data: $("form").serialize()
            , type: "get"
        }
        var target = $(this).parent().attr("data-otf-target");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
    };
    $(".container-fluid").on("change", ".page-number", getPageForDDL);

    var getPageSizeForDDL = function () {
        var TargetURL = $(this).parent().attr("data-otf-actionlink");
        TargetURL = TargetURL + "?PageSize=" + $(this).val()
        var options = {
            url: TargetURL
            , data: $("form").serialize()
            , type: "get"
        }
        var target = $(this).parent().attr("data-otf-target");
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
    };
    $(".container-fluid").on("change", ".page-size", getPageSizeForDDL);

    $(".container-fluid").on("click", ".delete", ShowWarningMessageBox);
    $(".container-fluid").on("click", ".publish", ShowPublishMessageBox);
    $(".container-fluid").on("click", ".status", ShowStatusMessageBox);

    $('#search').on("click", function () {
        $('.clear-result').removeClass('hidden');
    });
    $('.clear-result').on("click", function () {
        $(this).addClass('hidden');
    });

    var ajaxFormSubmit = function () {
        var $form = $(this);
        var options = {
            url: $form.attr("action")
            , type: $form.attr("method")
            , data: $form.serialize()
        }

        var target = $($form.attr("data-otf-target"));
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
        });
        return false;
    };
    $("form[data-otf-ajax='true']").submit(ajaxFormSubmit);

    //var url = window.location.href;
    //var pathArray = location.pathname.split('/');
    //var path = pathArray[1];
    //// passes on every "a" tag
    //$("#dock a").each(function () {
    //    // checks if its the same on the address bar
    //    var href = $(this).attr('href');
    //    if (href.indexOf(path) >= 0) {
    //        $(this).parents("li").addClass("active");
    //    }
    //    //if (url == (this.href)) {
    //    //    $(this).parents("li").addClass("active");
    //    //}
    //});

});

function CustomAjaxFormSubmit(sender, url) {
    if (url == "#")
    { return false; }
    var $form = $('a[href="' + decodeURI(url) + '"]').closest('form')
    if ($form.attr("data-otf-ajax") == 'true') {
        var options = {
            url: decodeURI(url)
            , type: $form.attr("method")
            , data: $form.serialize()
        }
        var target = $($form.attr("data-otf-target"));
        $.ajax(options).done(function (data) {
            $(target).replaceWith(data);
            //location.reload();
            ShowMessageBox();
        });
        return false;
    }
    else {
        return true;
    }
};

function ShowMessageBox() {
    var msgId = $('#ErrMsgHiddenField').val();
    if (msgId != null && msgId.toString().trim() != "") {
        //var arr = msgId.split('$');
        //if (parseInt(arr[0]) == 0) {
        //    $('#myLabel').html('<b class="text-success">Success</b>');
        //    $('#Msg').text(arr[1]);
        //}
        //else {
            $('#myLabel').html('<b class="text-danger">Information</b>');
            $('#Msg').text(msgId);
        //}
        $('#myErroMsgModalYesButton').addClass('hidden');
        $('#myErroMsgModal').show();
    }
}
var ShowPublishMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myErroModalLabel').text('Information');
        //$('#myErroMsgModalNoButton').val("Cancel");
        $('#Msg').text('Are you sure you want to Publish this record..?');
        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModalNoButton').val('Cancel');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}

var ShowWarningMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myLabel').html('<b class="text-info">Information</b>');

        $('#Msg').text('Are you sure that you want to delete the record..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}

var ShowStatusMessageBox = function (e) {
    if ($(this).text() != "Cancel") {
        // Set the sender infromation in hidden field and its closest form
        $("#eventSender").val(($(this).attr('href')) + '|' + $($(this).closest('form')));

        $('#myLabel').html('<b class="text-info">Information</b>');

        $('#Msg').text('Do you want to change the status of the user..?');

        $('#myErroMsgModalYesButton').removeClass('hidden');
        $('#myErroMsgModal').show();
        e.preventDefault();
    }
}

// Close message box
function CloseMyModal() {
    $('#myErroMsgModalYesButton').addClass('hidden');
    $("#myErroMsgModal").hide();
}
// close message box and procceed for intended action.
function OkMyModal() {
    $("#myErroMsgModal").hide();
    // Retrieve the sender infromation from hidden field and pass it to the function
    CustomAjaxFormSubmit($("#eventSender").val().split('|')[1], $("#eventSender").val().split('|')[0]);
}


