import 'dart:convert';
import 'dart:io';
import '../config/env.dart';
import '../models/msg_dto.dart';
import 'package:http/http.dart' as http;

class MessageService {
  final IAppEnv env;
  final http.Client client;
  MessageService(this.client, {required this.env});

  Future<List<MsgDto>> loadUserMessages(targetId) async {
    final response = await client.get(
      Uri.parse(Uri.encodeFull(
          env.messageApiAddress + '/api/messages/user?from=' + targetId)),
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
          env.messageApiAddress + '/api/messages/group?groupId=' + groupId)),
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
