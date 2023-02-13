import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/baseController.dart';
import 'package:signalr_demo_app/controllers/chatContainerController.dart';
import 'package:signalr_demo_app/models/contact.dart';
import 'package:signalr_demo_app/services/group_member_service.dart';

import '../models/chat_message.dart';
import '../models/group_member.dart';
import '../models/msg_dto.dart';
import '../services/hub_service.dart';
import '../services/message_storage_service.dart';

class GroupChatDetailController extends BaseController {
  final String chatId;
  final String name;
  GroupChatDetailController({required this.chatId, required this.name});

  RxList messages = [].obs;
  var groupMembers = GroupMembers([], GroupInfo(GroupId: "", Name: "")).obs;
  late ChatContainerController containerController;
  late MessageStorageService messageStorageService;
  late GroupMemberService groupMemberService;

  final messageController = TextEditingController();
  final scrollController = ScrollController();

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

    //load group member
    groupMemberService = Get.find();
    groupMembers.value =
        await groupMemberService.getGroupMembersInfo(this.chatId);

    //load history
    messages.value =
        await messageStorageService.loadMessage(Profile.UserId, chatId);
  }

  sendMsg() async {
    var message = messageController.text;
    if (message.length <= 0) {
      return;
    }

    messageController.clear();
    await HubService.hubConnection!
        .invoke("SendToGroup", args: <Object>[chatId, message])
        .then((value) => {print("Message sent")})
        .onError((error, stackTrace) => {print("Failed to send message")});
  }

  updateMsg(MsgDto msg) {
    messages.add(ChatMessage(
        id: msg.Id,
        sender: msg.From,
        messageContent: msg.Content,
        messageType: msg.From == Profile.UserId ? "sender" : "receiver"));
    update();
    scrollToBottom();
  }

  subscribe(ChatContainerController containerController) {
    this.containerController = containerController;
    containerController.addGroupFollower(this);
  }

  scrollToBottom() {
    if (scrollController.hasClients)
      scrollController.jumpTo(scrollController.position.maxScrollExtent + 60);
  }

  String getSenderName(senderId) {
    return groupMembers.value.Members
        .firstWhere(
          (element) => element.UserId == senderId,
          orElse: () => MemberInfo(
              UserId: senderId,
              NickName: "unknown",
              AvatarUrl: "unknown",
              Email: "unknown"),
        )
        .NickName;
  }

  String getSenderAvatarUrl(senderId) {
    return groupMembers.value.Members
        .firstWhere(
          (element) => element.UserId == senderId,
          orElse: () => MemberInfo(
              UserId: senderId,
              NickName: "unknown",
              AvatarUrl: "unknown",
              Email: "unknown"),
        )
        .AvatarUrl;
  }
}
