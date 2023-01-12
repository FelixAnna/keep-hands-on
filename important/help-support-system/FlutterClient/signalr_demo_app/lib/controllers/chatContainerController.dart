import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/baseController.dart';
import 'package:signalr_demo_app/controllers/chatDetailController.dart';
import 'package:signalr_demo_app/models/chat_messages.dart';
import 'package:signalr_demo_app/models/msg_dto.dart';
import 'package:signalr_demo_app/services/user_service.dart';
import '../models/contact.dart';
import '../services/hub_service.dart';
import '../services/message_service.dart';
import '../services/message_storage_service.dart';
import 'groupChatDetailController.dart';

class ChatContainerController extends BaseController {
  List<ChatDetailController> personFollowers = [];
  List<GroupChatDetailController> groupFollowers = [];

  var UserContact = Contact("", [], []).obs;
  var ChatMsgs = new Map<String, ChatMessages>().obs;

  late HubService hubService;
  late UserService userService;
  late MessageService msgService;
  late MessageStorageService messageStorageService;

  @override
  void onInit() async {
    super.onInit();
    await initial();
  }

  initial() async {
    messageStorageService = Get.find();
    if (!await loadUserFromCache()) {
      Get.toNamed("/login");
      return;
    }

    //load contact
    await loadContactAndMessages();

    // initial connection
    await initialHubConnect();

    print("chat ready");
  }

  loadContactAndMessages() async {
    userService = Get.find();
    var contact = await userService.getCurrentContacts();

    for (var user in contact.Friends) {
      setChatMsgs(user.UserId, []);
    }

    for (var group in contact.Groups) {
      setChatMsgs(group.GroupId, []);
    }

    // initial contact
    UserContact.value = contact;

    //load history messages for each contact
    msgService = Get.find();
    var waitList = Map<String, Future<List<MsgDto>>>();
    for (var user in contact.Friends) {
      waitList[user.UserId] = msgService.loadUserMessages(user.UserId);
    }

    for (var group in contact.Groups) {
      waitList[group.GroupId] = msgService.loadGroupMessages(group.GroupId);
    }

    for (var chatId in waitList.keys) {
      setChatMsgs(chatId, await waitList[chatId]!);
    }

    //save to local db
    for (var chatId in ChatMsgs.keys) {
      await messageStorageService.clearMessage(chatId);
      await messageStorageService.saveMessage(chatId, ChatMsgs[chatId]!);
    }
  }

  initialHubConnect() async {
    hubService = Get.find();
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

  mergeMsg(String from, String to, String msg) async {
    var chatId = Profile.UserId == to ? from : to;
    if (!ChatMsgs.containsKey(chatId)) {
      setChatMsgs(chatId, []);
    }

    var newMsge = MsgDto(
        Id: -1,
        From: from,
        To: to,
        Content: msg,
        MsgTime: DateTime.now().toUtc());
    var messages = ChatMsgs[chatId]!.Messages;
    messages.add(newMsge);
    setChatMsgs(chatId, messages);

    //notify and update all chat detail page
    notify(chatId, newMsge);

    //persist new message
    await messageStorageService.saveNewMessage(
        chatId, ChatMsgs[chatId]!, newMsge);
  }

  setChatMsgs(String chatId, List<MsgDto> messages) {
    //TODO need lock here
    if (messages.length > 0) {
      var last = messages.last;
      ChatMsgs[chatId] = ChatMessages(
          OwnerId: Profile.UserId,
          ChatId: chatId,
          LatestMsge: last.Content,
          UnreadCount: messages.length, //TODO: how to get the unread count
          Time: last.MsgTime!.toUtc(),
          Messages: messages);
    } else {
      ChatMsgs[chatId] = ChatMessages(
        OwnerId: Profile.UserId,
        ChatId: chatId,
        LatestMsge: "",
        UnreadCount: 0,
        Messages: [],
        Time: null,
      );
    }
  }

  notify(String chatId, MsgDto newMsge) {
    if (personFollowers.any((e) => e.chatId == chatId)) {
      var follower = personFollowers.where((e) => e.chatId == chatId).first;
      follower.updateMsg(newMsge);
      return;
    }

    if (groupFollowers.any((e) => e.chatId == chatId)) {
      var follower = groupFollowers.where((e) => e.chatId == chatId).first;
      follower.updateMsg(newMsge);
    }
  }

  addPersonFollower(ChatDetailController controller) {
    if (!personFollowers.any((e) => controller.chatId == e.chatId)) {
      this.personFollowers.add(controller);
    }
  }

  addGroupFollower(GroupChatDetailController controller) {
    if (!groupFollowers.any((e) => controller.chatId == e.chatId)) {
      this.groupFollowers.add(controller);
    }
  }
}
