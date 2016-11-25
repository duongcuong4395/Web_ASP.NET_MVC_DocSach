
$(function () {

    setScreen(false);

    // Declare a proxy to reference the hub.
    var chatHub = $.connection.chatHub;

    registerClientMethods(chatHub);

    // Start Hub
    $.connection.hub.start().done(function () {

        registerEvents(chatHub)

    });

});

function setScreen(isLogin) {

    if (!isLogin) {

        $("#divChat").hide();
        $("#divLogin").show();
    }
    else {

        $("#divChat").show();
        $("#divLogin").hide();
    }

}

function registerEvents(chatHub) {

    $("#btnStartChat").click(function () {

        var name = $("#txtNickName").val();
        if (name.length > 0) {
            chatHub.server.connect(name);
        }
        else {
            alert("Điền tên của bạn.");
        }

    });


    $('#btnSendMsg').click(function () {

        var msg = $("#txtMessage").val();
        if (msg.length > 0) {

            var userName = $('#hdUserName').val();
            chatHub.server.sendMessageToAll(userName, msg);
            $("#txtMessage").val('');
        }
    });


    $("#txtNickName").keypress(function (e) {
        if (e.which == 13) {
            $("#btnStartChat").click();
        }
    });

    $("#txtMessage").keypress(function (e) {
        if (e.which == 13) {
            $('#btnSendMsg').click();
        }
    });


}

function registerClientMethods(chatHub) {

    // Calls when user successfully logged in
    chatHub.client.onConnected = function (id, userName, allUsers, messages) {

        setScreen(true);

        $('#hdId').val(id);
        $('#hdUserName').val(userName);
        $('#spanUser').html(userName);

        // Add All Users
        for (i = 0; i < allUsers.length; i++) {

            AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName);
        }

        // Add Existing Messages
        for (i = 0; i < messages.length; i++) {

            AddMessage(messages[i].UserName, messages[i].Message);
        }


    }

    // On New User Connected
    chatHub.client.onNewUserConnected = function (id, name) {

        AddUser(chatHub, id, name);
    }


    // On User Disconnected
    chatHub.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();

        var ctrId = 'private_' + id;
        $('#' + ctrId).remove();


        var disc = $('<div class="disconnect">"' + userName + '" đã đăng xuất.</div>');

        $(disc).hide();
        $('#divusers').prepend(disc);
        $(disc).fadeIn(200).delay(8000).fadeOut(200);

    }

    chatHub.client.messageReceived = function (userName, message) {

        AddMessage(userName, message);
    }


    chatHub.client.sendPrivateMessage = function (windowId, fromUserName, message) {

        var ctrId = 'private_' + windowId;


        if ($('#' + ctrId).length == 0) {

            createPrivateChatWindow(chatHub, windowId, ctrId, fromUserName);

        }


        $('#' + ctrId).find('#divMessage').append('<li class="left clearfix">' +
                                '<span class="chat-img pull-left">' +
                                    '<img src="http://placehold.it/50/55C1E7/fff" alt="User Avatar" class="img-circle">' +
                                '</span>' +
                                '<div class="chat-body clearfix">' +
                                    '<div >' +
                                        '<strong class="primary-font">' + fromUserName + '</strong>' +
                                    '</div>' +
                                    '<div style="max-width: 150px;" class = "khungTextChat">' +
                                        '<p class = "textChat" style="max-width: 140px;">' + message +
                                    '</div>' +
                                    '</p>' +
                                '</div>' +
                            '</li>');

        // set scrollbar
        var height = $('#' + ctrId).find('#divMessage')[0].scrollHeight;
        $('#' + ctrId).find('#divMessage').scrollTop(height);

    }

}

function AddUser(chatHub, id, name) {

    var userId = $('#hdId').val();

    var code = "";

    if (userId == id) {

        code = $('<div class="loginUser">' + "</div>");

    }
    else {


        code = $('<a id="' + id + '" class="list-group-item">' +
                                '<i class="fa fa-comment fa-fw"></i>' + name +
                                '<span class="pull-right text-muted small"><em>Đang online</em>' +
                                '</span>' +
                            '</a>');
        $(code).dblclick(function () {

            var id = $(this).attr('id');

            if (userId != id)
                OpenPrivateChatWindow(chatHub, id, name);

        });
    }

    $("#divusers").append(code);

}

function AddMessage(userName, message) {
    $('#divChatWindow').append('<div class="message"><span class="userName">' + userName + '</span>: ' + message + '</div>');

    var height = $('#divChatWindow')[0].scrollHeight;
    $('#divChatWindow').scrollTop(height);
}

function OpenPrivateChatWindow(chatHub, id, userName) {

    var ctrId = 'private_' + id;

    if ($('#' + ctrId).length > 0) return;

    createPrivateChatWindow(chatHub, id, ctrId, userName);

}

function createPrivateChatWindow(chatHub, userId, ctrId, userName) {
    var div = '<div id="' + ctrId + '" class="ui-widget-content draggable " rel="0">' +
                   '<div class="headerchat">' +
                      '<div  style="float:right;">' +
                          '<img id="imgDelete"  style="cursor:pointer;" src="/Images/Icon/remove.png"/>' +
                       '</div>' +

                       '<span class="selText" rel="0">' + userName + '</span>' +
                   '</div>' +
                   '<div id="divMessage" class="messageArea">' +

                   '</div>' +
                  '<div class="panel-footer">' +
                        '<div class="input-group">' +
                            '<input id="txtPrivateMessage" type="text" class="form-control input-sm" placeholder="Nhập tin nhắn...">' +
                            '<span class="input-group-btn">' +
                                '<button class="btn btn-warning btn-sm" id="btnSendMessage">' +
                                    'Gửi' +
                                '</button>' +
                            '</span>' +
                        '</div>' +
                    '</div>' +
                '</div>';
    var $div = $(div);

    // DELETE BUTTON IMAGE
    $div.find('#imgDelete').click(function () {
        $('#' + ctrId).remove();
    });

    // Send Button event
    $div.find("#btnSendMessage").click(function () {

        $textBox = $div.find("#txtPrivateMessage");
        var msg = $textBox.val();
        if (msg.length > 0) {

            chatHub.server.sendPrivateMessage(userId, msg);
            $textBox.val('');
        }
    });

    // Text Box event
    $div.find("#txtPrivateMessage").keypress(function (e) {
        if (e.which == 13) {
            $div.find("#btnSendMessage").click();
        }
    });

    AddDivToContainer($div);

}

function AddDivToContainer($div) {
    $('#divContainer').prepend($div);

    $div.draggable({

        handle: ".headerchat",
        stop: function () {

        }
    });
}
