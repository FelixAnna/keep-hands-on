import 'package:get/get.dart';
import 'package:signalr_demo_app/models/chat_messages.dart';
import 'package:signalr_demo_app/models/msg_dto.dart';
import 'package:signalr_demo_app/services/user_service.dart';
import '../models/contact.dart';
import '../services/auth_storage_service.dart';
import '../services/hub_service.dart';
import '../services/message_service.dart';
import 'profileController.dart';

class ChatContainerController extends ProfileController {
  var UserContact = Contact("", [], []).obs;
  var ChatMsgs = new Map<String, ChatMessages>().obs;

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
    if (Token == '') {
      Get.toNamed("/login");
      return;
    }

    //load contact
    await loadContactAndMessages();

    // initial connection
    await initialHubConnect();
  }

  initialHubConnect() async {
    hubService = Get.find<HubService>();
    await hubService.initial();

    // subscribe to messages
    hubService.subscribe(
        listeningMessage: handleNewUserMessage, listenMethod: "ReceiveMessage");
    hubService.subscribe(
        listeningMessage: handleNewGroupMessage,
        listenMethod: "ReceiveGroupMessage");

    // start connection
    await hubService.start(callback: () => Get.toNamed("/login"));
  }

  loadContactAndMessages() async {
    userService = Get.find<UserService>();
    var contact = await userService.getCurrentContacts();

    //load history messages for each contact
    msgService = Get.find<MessageService>();
    for (var user in contact.Friends) {
      var messages = await msgService.loadUserMessages(user.UserId);
      setChatMsgs(user.UserId, messages);
    }

    for (var group in contact.Groups) {
      var messages = await msgService.loadGroupMessages(group.GroupId);
      setChatMsgs(group.GroupId, messages);
    }

    // update contact after all done
    UserContact.value = contact;
  }

  setChatMsgs(String chatId, List<MsgDto> messages) {
    //TODO need lock here
    if (messages.length > 0) {
      var last = messages.last;
      ChatMsgs[chatId] = ChatMessages(
          ChatId: chatId,
          LatestMsge: last.Content,
          UnreadCount: messages.length, //TODO: how to get the unread count
          Time: last.MsgTime!.toUtc(),
          Messages: messages);
    } else {
      ChatMsgs[chatId] = ChatMessages(
        ChatId: chatId,
        LatestMsge: "",
        UnreadCount: 0,
        Messages: [],
        Time: null,
      );
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
    if (!ChatMsgs.containsKey(chatId)) {
      setChatMsgs(chatId, []);
    }

    var chatMsg = ChatMsgs[chatId]!;
    var utcNow = DateTime.now().toUtc();
    chatMsg.LatestMsge = msg;
    chatMsg.Time = utcNow;
    chatMsg.UnreadCount++;
    chatMsg.Messages.add(MsgDto(
      Id: -1,
      From: from,
      To: to,
      Content: msg,
      MsgTime: utcNow,
    ));

    ChatMsgs.update(chatId, (value) => chatMsg);
  }
}
