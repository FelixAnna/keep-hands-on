import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/chatContainerController.dart';

import '../models/chat_message.dart';
import '../models/msg_dto.dart';
import '../services/hub_service.dart';
import '../services/message_storage_service.dart';
import 'baseController.dart';

class ChatDetailController extends BaseController {
  final String chatId;
  final String name;
  ChatDetailController({required this.chatId, required this.name});

  RxList messages = [].obs;
  late ChatContainerController containerController;
  late MessageStorageService messageStorageService;

  var messageController = TextEditingController();
  var scrollController = ScrollController();

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
        .invoke("SendToUser", args: <Object>[chatId, message])
        .then((value) => {print("Message sent")})
        .onError((error, stackTrace) => {print("Failed to send message")});

    containerController.mergeMsg(Profile.UserId, chatId, message);
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
    containerController.addPersonFollower(this);
  }

  scrollToBottom() {
    if (scrollController.hasClients)
      scrollController.jumpTo(scrollController.position.maxScrollExtent + 60);
  }
}
