import 'package:signalr_demo_app/models/chat_messages.dart';

import '../models/chat_message.dart';
import '../models/msg_dto.dart';
import '../utils/sqflite_service.dart';

class MessageStorageService {
  Future<List<ChatMessage>> loadMessage(String userId, String chatId) async {
    var localDb = await SQFliteService.getDatabase();
    final List<Map<String, dynamic>> maps = await localDb.query(
      "Messages",
      where: "chatId=?",
      whereArgs: [chatId],
      orderBy: "msgTime desc",
      limit: 30,
      offset: 0,
    );
    var rawMsgs = List.generate(maps.length, (i) {
      return MsgDto(
          Id: maps[i]['id'],
          From: maps[i]['sender'],
          To: maps[i]['target'],
          Content: maps[i]['content'],
          MsgTime: DateTime.parse(maps[i]['msgTime']));
    });

    rawMsgs.sort((a, b) =>
        a.MsgTime!.microsecondsSinceEpoch - b.MsgTime!.microsecondsSinceEpoch);

    return rawMsgs
        .map((e) => ChatMessage(
            id: e.Id,
            sender: e.From,
            messageContent: e.Content,
            messageType: e.From == userId ? "sender" : "receiver"))
        .toList();
  }

  Future saveMessage(String chatId, ChatMessages chatMessages) async {
    var localDb = await SQFliteService.getDatabase();
    var batch = localDb.batch();
    batch.insert("Chats", chatMessages.toMap());
    for (var msg in chatMessages.Messages) {
      batch.insert("Messages", msg.toMap(chatId));
    }

    await batch.commit();
  }

  Future saveNewMessage(
      String chatId, ChatMessages chatMessages, MsgDto newMsg) async {
    var localDb = await SQFliteService.getDatabase();
    var batch = localDb.batch();
    batch.insert("Chats", chatMessages.toMap());
    batch.insert("Messages", newMsg.toMap(chatId));

    await batch.commit();
  }
}
