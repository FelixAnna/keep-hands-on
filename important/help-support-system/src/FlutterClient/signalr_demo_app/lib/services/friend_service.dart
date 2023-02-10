import 'dart:convert';
import 'dart:io';
import '../config/env.dart';
import 'package:http/http.dart' as http;

class FriendService {
  final IAppEnv env;
  final http.Client client;
  FriendService(this.client, {required this.env});

  Future<bool> AddFriend(String friendId) async {
    var body = json.encode({"targetUserId": friendId});
    final response = await http.post(
      Uri.parse(env.userApiAddress + "/api/friends/"),
      headers: {
        HttpHeaders.contentTypeHeader: 'application/json',
      },
      encoding: Encoding.getByName('utf-8'),
      body: body,
    );

    if (response.statusCode == HttpStatus.ok) {
      return true;
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }
}
