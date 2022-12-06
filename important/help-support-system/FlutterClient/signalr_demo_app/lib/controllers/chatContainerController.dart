import 'dart:convert';

import 'package:get/get.dart';
import 'package:signalr_demo_app/models/chat_messages.dart';
import 'package:signalr_demo_app/models/msg_dto.dart';
import 'package:signalr_demo_app/services/user_service.dart';
import '../models/contact.dart';
import '../models/login_response.dart';
import '../services/auth_storage_service.dart';
import '../services/hub_service.dart';
import '../services/message_service.dart';

class ChatContainerController extends GetxController {
  late User Profile;
  late Contact contact;
  late List<ChatMessages> ChatMsgs;

  late HubService hubService;
  late UserService userService;
  late MessageService msgService;
  late AuthStorageService authStoreService;

  @override
  void onInit() async {
    super.onInit();
    await initial();
  }

  initial() async {
    var loginCache = authStoreService.GetLoginCache();
    Profile = User.fromJson(jsonDecode(loginCache[1]));

    hubService = Get.find<HubService>();

    // initial connection
    await hubService.initial();

    // subscribe to messages
    hubService.subscribe(
        listeningMessage: handleNewUserMessage, listenMethod: "ReceiveMessage");
    hubService.subscribe(
        listeningMessage: handleNewGroupMessage,
        listenMethod: "ReceiveGroupMessage");

    // start connection
    hubService.start(callback: () => Get.toNamed("/login"));

    //load contact
    userService = Get.find<UserService>();
    contact = await userService.getCurrentContacts();

    //load history messages for each contact
    msgService = Get.find<MessageService>();
    for (var user in contact.Friends) {
      var messages = await msgService.loadUserMessages(user.UserId);
      ChatMsgs.add(ChatMessages(
          ChatId: user.UserId,
          LatestMsge: messages.last.Content,
          UnreadCount: messages.length, //TODO: how to get the unread count
          Time: messages.last.MsgTime,
          Messages: messages));
    }

    for (var group in contact.Groups) {
      var messages = await msgService.loadGroupMessages(group.GroupId);
      ChatMsgs.add(ChatMessages(
          ChatId: group.GroupId,
          LatestMsge: messages.last.Content,
          UnreadCount: messages.length, //TODO: how to get the unread count
          Time: messages.last.MsgTime,
          Messages: messages));
    }
  }

  handleNewUserMessage(List<Object?>? parameters) {
    var fromUser = parameters?.elementAt(0).toString();
    var toUserId = parameters?.elementAt(1).toString();
    var msg = parameters?.elementAt(2).toString();

    mergeMsg(fromUser!, toUserId!, msg!);
  }

  handleNewGroupMessage(List<Object?>? parameters) {
    var fromUser = parameters?.elementAt(0).toString();
    var groupId = parameters?.elementAt(1).toString();
    var msg = parameters?.elementAt(2).toString();

    mergeMsg(fromUser!, groupId!, msg!);
  }

  mergeMsg(String from, String to, String msg) {
    var chatId = Profile.UserId == to ? from : to;
    var chatMsg = getChatSummary(chatId);

    chatMsg.LatestMsge = msg;
    chatMsg.UnreadCount++;
    chatMsg.Messages.add(MsgDto(
      Id: -1,
      From: from,
      To: to,
      Content: msg,
      MsgTime: null,
    ));
  }

  ChatMessages getChatSummary(String chatId) {
    if (!ChatMsgs.any((element) => element.ChatId == chatId)) {
      ChatMsgs.add(ChatMessages(
        ChatId: chatId,
        LatestMsge: "",
        UnreadCount: 0,
        Messages: [],
        Time: DateTime.now().toUtc(),
      ));
    }

    var chatMsg = ChatMsgs.firstWhere((element) => element.ChatId == chatId);

    return chatMsg;
  }
}
