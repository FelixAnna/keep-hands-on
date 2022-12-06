import 'dart:convert';
import 'dart:io';
import 'package:signalr_demo_app/models/chat_message.dart';
import '../config/env.dart';
import '../models/msg_dto.dart';
import 'package:http/http.dart' as http;

class MessageService {
  final IAppEnv env;
  final http.Client client;
  MessageService(this.client, {required this.env});

  Future<List<ChatMessage>> getMessages(from, to) async {
    final response = await client.get(
      Uri.parse(Uri.encodeFull(env.userApiAddress +
          '/api/messages/user?from=' +
          from +
          "&to=" +
          to)),
    );
    var body = jsonDecode(response.body);

    var messages = body as List;
    List<ChatMessage> msgList = messages.map((tagJson) {
      final item = MsgDto.fromJson(tagJson);
      return ChatMessage(
          sender: item.From,
          messageContent: item.Content,
          messageType: to == item.To ? 'receiver' : 'sender');
    }).toList();

    if (response.statusCode == HttpStatus.ok) {
      return msgList;
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }

  Future<List<ChatMessage>> getGroupMessages(groupId, userId) async {
    final response = await client.get(
      Uri.parse(Uri.encodeFull(
          env.userApiAddress + '/api/messages/group?groupId=' + groupId)),
    );
    var body = jsonDecode(response.body);

    var messages = body as List;
    List<ChatMessage> msgList = messages.map((tagJson) {
      final item = MsgDto.fromJson(tagJson);
      return ChatMessage(
          sender: item.From,
          messageContent: item.Content,
          messageType: userId != item.From ? 'receiver' : 'sender');
    }).toList();

    if (response.statusCode == HttpStatus.ok) {
      return msgList;
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }

  Future<List<MsgDto>> loadUserMessages(targetId) async {
    final response = await client.get(
      Uri.parse(Uri.encodeFull(
          env.userApiAddress + '/api/messages/user?target=' + targetId)),
    );
    var body = jsonDecode(response.body);

    var messages = body as List;
    List<MsgDto> msgList = messages.map((tagJson) {
      final item = MsgDto.fromJson(tagJson);
      return item;
    }).toList();

    if (response.statusCode == HttpStatus.ok) {
      return msgList;
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }

  Future<List<MsgDto>> loadGroupMessages(groupId) async {
    final response = await client.get(
      Uri.parse(Uri.encodeFull(
          env.userApiAddress + '/api/messages/group?groupId=' + groupId)),
    );
    var body = jsonDecode(response.body);

    var messages = body as List;
    List<MsgDto> msgList = messages.map((tagJson) {
      final item = MsgDto.fromJson(tagJson);
      return item;
    }).toList();

    if (response.statusCode == HttpStatus.ok) {
      return msgList;
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }
}
