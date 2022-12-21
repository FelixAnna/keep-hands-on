import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/controllers/chatContainerController.dart';
import 'package:sqflite/sqflite.dart';

import '../models/chat_message.dart';
import '../models/msg_dto.dart';
import '../services/hub_service.dart';
import '../utils/sqflite_service.dart';
import 'baseController.dart';

class ChatDetailController extends BaseController {
  final String chatId;
  final String name;
  ChatDetailController({required this.chatId, required this.name});

  late Database localDb;

  String message = "";
  List<MsgDto> rawMsgs = [];
  RxList messages = [].obs;
  final messageController = TextEditingController();
  late ChatContainerController containerController;

  ScrollController scrollController = ScrollController();

  late HubService hubService;

  @override
  void onInit() async {
    super.onInit();
    await initial();
  }

  initial() async {
    localDb = await SQFliteService.getDatabase();
    if (!await loadUserFromCache()) {
      Get.toNamed("/login");
      return;
    }

    //load history
    await loadMessages();
    scrollToBottom();
  }

  Future<void> loadMessages() async {
    final List<Map<String, dynamic>> maps = await localDb.query(
      "Messages",
      where: "chatId=?",
      whereArgs: [chatId],
      orderBy: "msgTime desc",
      limit: 30,
      offset: 0,
    );
    rawMsgs = List.generate(maps.length, (i) {
      return MsgDto(
          Id: maps[i]['id'],
          From: maps[i]['sender'],
          To: maps[i]['target'],
          Content: maps[i]['content'],
          MsgTime: DateTime.parse(maps[i]['msgTime']));
    });

    rawMsgs.sort((a, b) =>
        a.MsgTime!.microsecondsSinceEpoch - b.MsgTime!.microsecondsSinceEpoch);

    messages.value = rawMsgs
        .map((e) => ChatMessage(
            id: e.Id,
            sender: e.From,
            messageContent: e.Content,
            messageType: e.From == Profile.UserId ? "sender" : "receiver"))
        .toList();
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
    containerController.addP2pFollower(this);
  }

  scrollToBottom() {
    scrollController.jumpTo(scrollController.position.maxScrollExtent + 60);
  }
}
